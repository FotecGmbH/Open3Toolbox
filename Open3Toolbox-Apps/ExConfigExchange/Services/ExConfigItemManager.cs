// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Entwickler      Istvan Galfi
// Projekt         Dataskop

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ExConfigExchange.Annotations;
using ExConfigExchange.JsonUtils;
using ExConfigExchange.Models;
using ExConfigExchange.Models.Interfaces;
using ExConfigExchange.Services.Interfaces;
using System.Collections.ObjectModel;
using Biss.Serialize;
using IX.Observable;

namespace ExConfigExchange.Services
{
    /// <summary>
    /// Diese Klasse dient als generelle Verwalter von <see cref="IExConfigItem"/> Instanzen.
    /// </summary>
    /// <seealso cref="ExConfigExchange.Services.Interfaces.IExConfigItemManager" />
    public class ExConfigItemManager : IExConfigItemManager
    {
        /// <summary>
        /// Ersatz für den Fehlende <see cref="Type"/> basierten <see cref="switch"/> statement für einfache <see cref="Type"/>.
        /// </summary>
        private static Dictionary<Type, Func<string, MemberInfo, object, bool, IExConfigItem>> _basicTypeMapper =
            new Dictionary<Type, Func<string, MemberInfo, object, bool, IExConfigItem>>()
            {
                { typeof(string), ExConfigItemFactory.GetString },
                { typeof(Uri), ExConfigItemFactory.GetUrl },
                { typeof(byte), ExConfigItemFactory.GetByte },
                { typeof(int), ExConfigItemFactory.GetInt },
                { typeof(long), ExConfigItemFactory.GetLong },
                { typeof(float), ExConfigItemFactory.GetFloat },
                { typeof(double), ExConfigItemFactory.GetDouble },
                { typeof(bool), ExConfigItemFactory.GetBool },
            };

        /// <summary>
        /// Der <see cref="ExObjectConfigItem"/> Vorlagen cacher.
        /// </summary>
        private readonly IExObjectConfigItemTemplateCacher _templateCacher;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExConfigItemManager"/> class.
        /// </summary>
        /// <param name="templateCacher">Der <see cref="ExObjectConfigItem"/> Vorlagen cacher.</param>
        /// <exception cref="System.ArgumentNullException">templateCacher</exception>
        public ExConfigItemManager(IExObjectConfigItemTemplateCacher templateCacher)
        {
            this._templateCacher = templateCacher ?? throw new ArgumentNullException(nameof(templateCacher));
        }

        /// <summary>
        /// Diese Methode ist der Rekursive Version vom <see cref="GetIExConfigItemFrom(string, Type, object, bool)"/> für die Modellierung vom komplexe <see cref="Type"/>s.
        /// </summary>
        /// <param name="displayKey">Der display key.</param>
        /// <param name="type">Der zu modelierende komplexe <see cref="Type"/> (z.B.s.: <see cref="class"/>, <see cref="interface"/>).</param>
        /// <param name="defaultValue">Der default value.</param>
        /// <param name="readOnly">Falls <c>true</c> [read only].</param>
        /// <param name="implementationSearchForType">Wird für Self-referenzen bei Interfaces verwendet.</param>
        /// <returns>Die Entsprechende <see cref="IExConfigItem"/> Implementation.</returns>
        /// <exception cref="System.NotImplementedException">Handling of type: {type} not yet implemented.</exception>
        private IExConfigItem GetIExConfigItemFromComplexType(string displayKey, Type type, object defaultValue = null, bool readOnly = false, Type implementationSearchForType = null)
        {
            // Warning: Here the order of the if statements matters!
            if (typeof(Enum).IsAssignableFrom(type))
            {
                var enumConfig = new ExEnumConfigItem() { DisplayKey = displayKey, Value = new List<ExEnumItemConfigItem>(), ReadOnly = readOnly };
                foreach (var value in Enum.GetValues(type))
                {
                    var enumItem = new ExEnumItemConfigItem()
                    {
                        DisplayKey = value.GetType().GetDisplayKey() ?? value.ToString(),
                        Value = (int)value,
                    };
                    if (
                        defaultValue != null
                        && value.ToString() == defaultValue.ToString()
                    )
                        enumConfig.Selected = enumItem;

                    enumConfig.Value.Add(enumItem);
                }

                return enumConfig;
            }

            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                ExCollectionConfigItem collection = new ExCollectionConfigItem() { DisplayKey = displayKey, Value = new ObservableCollection<IExConfigItem>(), ReadOnly = readOnly };
                var innerType = type.GetGenericArguments().First();
                collection.ItemTemplate = _basicTypeMapper.ContainsKey(innerType) ? _basicTypeMapper[innerType]("Collection Item", innerType, null, false) : this.GetIExConfigItemFromComplexType("Collection Item", innerType);
                if (defaultValue != null)
                    foreach (var item in defaultValue as IEnumerable)
                    {
                        if (item is null)
                            continue;

                        collection.Value.Add(_basicTypeMapper.ContainsKey(item.GetType()) ? _basicTypeMapper[item.GetType()]("Collection Item", item.GetType(), item, false) : this.GetIExConfigItemFromComplexType("Collection Item", item.GetType(), item));
                    }
                return collection;
            }

            if (
                type.IsInterface
                || type.IsAbstract
                || type.ShouldBeTreatedAsInterface()
            )
            {
                var interfaceConfig = new ExObjectConfigItem() { DisplayKey = type.GetDisplayKey() ?? displayKey, IsInterface = true, InterfaceTypeName = type.FullName };
                if (!readOnly && !(implementationSearchForType != null && implementationSearchForType.IsAssignableFrom(type)))
                {
                    if (!_templateCacher.ContainsImplementationTemplatesFor(interfaceConfig.InterfaceTypeName))
                        _templateCacher.AddImplementationTemplatesFor(interfaceConfig.InterfaceTypeName, new ConcurrentBag<ExObjectConfigItem>());

                    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    //assemblies = assemblies.Where(a => !a.FullName.Contains("FluentValidation")).ToArray();
                    
                    //var types = assemblies.SelectMany(a => a.GetTypes());

                    List<Type> types = new List<Type>(); 

                    foreach (var assembly in assemblies)
                    {
                        try
                        {
                            types.AddRange(assembly.GetTypes());
                        }
                        catch (Exception e)
                        {
                        }
                        
                    }












                    var implementations = types.Where(t => !(t.IsInterface || t.IsAbstract || t.ShouldBeTreatedAsInterface()) && type.IsAssignableFrom(t)).ToList();
                    foreach (var implementation in implementations)
                    {
                        if (_templateCacher.GetImplementationTemplatesFor(interfaceConfig.InterfaceTypeName).Where(i => i.TypeFullname == implementation.FullName && i.AssemblyFullName == implementation.Assembly.FullName).Count() == 0)
                        {
                            if (GetIExConfigItemFromComplexType(implementation.GetDisplayKey() ?? implementation.Name, implementation, null, false, type) is ExObjectConfigItem template)
                            {
                                template.InterfaceTypeName = interfaceConfig.InterfaceTypeName;
                                _templateCacher.AddObjectTemplateFor(implementation.FullName, template);
                                _templateCacher.AddImplementationTemplateFor(interfaceConfig.InterfaceTypeName, template);
                            }
                        }
                    }
                }

                if (defaultValue is null)
                    return interfaceConfig;
                else
                    return this.GetIExConfigItemFromComplexType(displayKey, defaultValue.GetType(), defaultValue, readOnly);
            }

            if (
                type.IsClass
                && type != typeof(Delegate)
            )
            {
                if (_templateCacher.ContainsObjectTemplateFor(type.FullName))
                {
                    if (defaultValue is null)
                        return _templateCacher.GetObjectTemplateFor(type.FullName);
                    else
                    {
                        var obj = _templateCacher.GetObjectTemplateFor(type.FullName);
                        obj.Accept(new ExConfigItemSetterVisitor(_templateCacher), () => defaultValue);
                        return obj;
                    }
                }

                var objectConfig = new ExObjectConfigItem() { DisplayKey = type.GetDisplayKey() ?? displayKey, AssemblyFullName = type.Assembly.FullName, TypeFullname = type.FullName, ReadOnly = readOnly, Value = new ObservableDictionary<string, IExConfigItem>() };
                object instance = null;
                try
                {
                    instance = defaultValue is null ? Activator.CreateInstance(type) : defaultValue;
                }
                catch (Exception e)
                {
                    // Logging.Log.LogError($"Error: {e}");
                }

                foreach (var prop in type.GetProperties())
                {
                    IExConfigItem configItem = null;
                    if (!prop.ShouldLeaveNull())
                    {
                        Type configureAs = prop.ShouldConfigureAs();
                        Type propType = configureAs is null ? prop.PropertyType : configureAs;
                        bool propReadOnly = !prop.CanWrite || prop.IsReadOnly();
                        object newDefaultValue = instance != null ? prop.GetValue(instance) : null;

                        try
                        {
                            // ## Fusch ##
                            if (configureAs != null && newDefaultValue != null && newDefaultValue is string)
                                newDefaultValue = IDeserializableJsonExtensions.FromJson((newDefaultValue as string), propType);
                        }
                        catch (Exception e)
                        {
                            // Logging.Log.LogError($"Error: {e}");
                            newDefaultValue = null;
                        }

                        if (_basicTypeMapper.ContainsKey(propType))
                            configItem = _basicTypeMapper[propType](prop.GetDisplayKey() ?? prop.Name, prop, newDefaultValue, propReadOnly);
                        else
                        {
                            configItem = GetIExConfigItemFromComplexType(prop.GetDisplayKey() ?? prop.Name, propType, newDefaultValue, propReadOnly, implementationSearchForType);
                            configItem.Hidden = prop.IsHidden();
                            if (configItem is ExObjectConfigItem t && configureAs != null)
                            {
                                t.HadConfigureAsAttribute = true;
                                t.ImplementationRequired = prop.IsImplementationRequired();
                            }
                        }
                    }

                    string key = prop.GetJsonPropertyName();
                    string trueKey = key is null ? prop.Name : key;
                    objectConfig.Value.Add(trueKey, configItem);

                    if (objectConfig.DisplayNameKey is null && prop.GetCustomAttribute<DisplayNamePropertyAttribute>() != null)
                        objectConfig.DisplayNameKey = trueKey;
                }

                foreach (var field in type.GetFields())
                {
                    IExConfigItem configItem = null;
                    if (!field.ShouldLeaveNull())
                    {
                        Type configureAs = field.ShouldConfigureAs();
                        Type fieldType = configureAs is null ? field.FieldType : configureAs;
                        bool fieldReadOnly = field.IsInitOnly || field.IsLiteral || field.IsReadOnly();
                        object newDefaultValue = instance != null ? field.GetValue(instance) : null;
                        var constDefaultValue = field.IsLiteral ? field.GetRawConstantValue() : null;
                        newDefaultValue = constDefaultValue is null ? newDefaultValue : constDefaultValue;

                        try
                        {
                            // ## Fusch ##
                            if (configureAs != null && newDefaultValue != null && newDefaultValue is string)
                                newDefaultValue = IDeserializableJsonExtensions.FromJson((newDefaultValue as string), fieldType);
                        }
                        catch (Exception e)
                        {
                            // Logging.Log.LogError($"Error: {e}");
                        }

                        if (_basicTypeMapper.ContainsKey(fieldType))
                            configItem = _basicTypeMapper[fieldType](field.GetDisplayKey() ?? field.Name, field, newDefaultValue, fieldReadOnly);
                        else
                        {
                            configItem = GetIExConfigItemFromComplexType(field.GetDisplayKey() ?? field.Name, fieldType, newDefaultValue, fieldReadOnly, implementationSearchForType);
                            configItem.Hidden = field.IsHidden();
                            if (configItem is ExObjectConfigItem t && configureAs != null)
                            {
                                t.HadConfigureAsAttribute = true;
                                t.ImplementationRequired = field.IsImplementationRequired();
                            }
                        }
                    }

                    string key = field.GetJsonPropertyName();
                    objectConfig.Value.Add(key is null ? field.Name : key, configItem);
                }

                return objectConfig;
            }

            throw new NotImplementedException($"Handling of type: {type} not yet implemented.");
        }

        #region Interface Implementations

        /// <summary>
        /// Diese Methode Convertiert Existierende Modelle zum <see cref="IExConfigItem" />s.
        /// Es kann auch dafür eine bereits konfigurierte Modell verwendet werden <param name="defaultValue" />, in diesem Fall werden dessen werte übernommen.
        /// </summary>
        /// <param name="displayKey">Der display key.</param>
        /// <param name="type">Der zu modellierende type.</param>
        /// <param name="defaultValue">Der default value, aus diesem werden die Werte des <see cref="IExConfigItem" />s extrahiert falls vorhanden.</param>
        /// <param name="readOnly">Wenn <c>true</c> [read only].</param>
        /// <returns>
        /// Einen <see cref="IExConfigItem" /> der den angegebenen <paramref name="type" /> modelliert.
        /// </returns>
        public IExConfigItem GetIExConfigItemFrom(string displayKey, Type type, object defaultValue = null, bool readOnly = false)
        {
            if (_basicTypeMapper.ContainsKey(type))
                return _basicTypeMapper[type](displayKey, type, defaultValue, readOnly);

            return this.GetIExConfigItemFromComplexType(displayKey, type, defaultValue, readOnly);
        }

        /// <summary>
        /// Mit dieser Methode kann man die Implementationsvorlage für einen Interface, abstract Klasse oder Klasse der mit der <see cref="ExConfigExchange.Annotations.InterfaceAttribute" /> markiert wurde, abgefragt werden.
        /// </summary>
        /// <param name="interfaceType">
        ///   <see cref="Type.FullName" /> vom <see cref="interface" />.</param>
        /// <returns>Eine Sammlung von implementationen.</returns>
        public IEnumerable<ExObjectConfigItem> GetTemplatesFor(string interfaceType)
        {
            foreach (var implementation in _templateCacher.GetImplementationTemplatesFor(interfaceType))
                yield return BissDeserialize.FromJson<ExObjectConfigItem>(implementation.ToJson());
        }

        #endregion
    }
}
