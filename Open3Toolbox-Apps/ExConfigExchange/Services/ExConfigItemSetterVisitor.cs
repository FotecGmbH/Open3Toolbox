// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Entwickler      Istvan Galfi
// Projekt         Dataskop

using ExConfigExchange.JsonUtils;
using ExConfigExchange.Models;
using ExConfigExchange.Models.Interfaces;
using ExConfigExchange.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Biss.Serialize;
using IX.System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Biss.Interfaces;
using Newtonsoft.Json;
using OpCodesDllsLibrary;

namespace ExConfigExchange.Services
{
    /// <summary>
    /// Diese Klasse ist für den Setzen vom Werten eines bereits vorhandenen <see cref="IExConfigItem"/> Implementation mit Hilfe eines default values.
    /// </summary>   
    /// <seealso cref="object" />
    public class ExConfigItemSetterVisitor : IExConfigVisitor<object>
    {
        /// <summary>
        /// Der <see cref="ExObjectConfigItem"/> Vorlagen cacher.
        /// </summary>
        private readonly IExObjectConfigItemTemplateCacher _templateCacher;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExConfigItemSetterVisitor"/> class.
        /// </summary>
        /// <param name="templateCacher">The template cacher.</param>
        /// <exception cref="System.ArgumentNullException">templateCacher</exception>
        public ExConfigItemSetterVisitor(IExObjectConfigItemTemplateCacher templateCacher)
        {
            this._templateCacher = templateCacher ?? throw new ArgumentNullException(nameof(templateCacher));
        }
        
        #region Interface Implementations

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExStringConfigItem" /> instanz.
        /// </summary>
        /// <param name="exStringConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.String" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Immer <see cref="null"/>.
        /// </returns>
        public object Visit(ExStringConfigItem exStringConfigItem, Func<object> optionalCall = null)
        {
            exStringConfigItem.Value = optionalCall() as string;
            return null;
        }

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExUrlConfigItem" /> instanz.
        /// </summary>
        /// <param name="exUrlConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Uri" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Immer <see cref="null"/>.
        /// </returns>
        public object Visit(ExUrlConfigItem exUrlConfigItem, Func<object> optionalCall = null)
        {
            exUrlConfigItem.Value = optionalCall() as Uri;
            return null;
        }

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExByteConfigItem" /> instanz.
        /// </summary>
        /// <param name="exByteConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Byte" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Immer <see cref="null"/>.
        /// </returns>
        public object Visit(ExByteConfigItem exByteConfigItem, Func<object> optionalCall = null)
        {
            exByteConfigItem.Value = Convert.ToByte(optionalCall());
            return null;
        }

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExIntConfigItem" /> instanz.
        /// </summary>
        /// <param name="exIntConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Int32" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Immer <see cref="null"/>.
        /// </returns>
        public object Visit(ExIntConfigItem exIntConfigItem, Func<object> optionalCall = null)
        {
            exIntConfigItem.Value = Convert.ToInt32(optionalCall());
            return null;
        }

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExLongConfigItem" /> instanz.
        /// </summary>
        /// <param name="exLongConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Int64" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Immer <see cref="null"/>.
        /// </returns>
        public object Visit(ExLongConfigItem exLongConfigItem, Func<object> optionalCall = null)
        {
            exLongConfigItem.Value = Convert.ToInt64(optionalCall());
            return null;
        }

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExFloatConfigItem" /> instanz.
        /// </summary>
        /// <param name="exFloatConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Single" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Immer <see cref="null"/>.
        /// </returns>
        public object Visit(ExFloatConfigItem exFloatConfigItem, Func<object> optionalCall = null)
        {
            exFloatConfigItem.Value = Convert.ToSingle(optionalCall());
            return null;
        }

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExDoubleConfigItem" /> instanz.
        /// </summary>
        /// <param name="exDoubleConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Double" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Immer <see cref="null"/>.
        /// </returns>
        public object Visit(ExDoubleConfigItem exDoubleConfigItem, Func<object> optionalCall = null)
        {
            exDoubleConfigItem.Value = Convert.ToDouble(optionalCall());
            return null;
        }

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExBoolConfigItem" /> instanz.
        /// </summary>
        /// <param name="exBoolConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Boolean" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Immer <see cref="null"/>.
        /// </returns>
        public object Visit(ExBoolConfigItem exBoolConfigItem, Func<object> optionalCall = null)
        {
            exBoolConfigItem.Value = Convert.ToBoolean(optionalCall());
            return null;
        }

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExEnumConfigItem" /> instanz.
        /// </summary>
        /// <param name="exEnumConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="!:enum" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Immer <see cref="null"/>.
        /// </returns>
        public object Visit(ExEnumConfigItem exEnumConfigItem, Func<object> optionalCall = null)
        {
            int value = (int)optionalCall();
            exEnumConfigItem.Selected = exEnumConfigItem.Value.First(i => i.Value == value);
            return null;
        }

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExObjectConfigItem" /> instanz.
        /// </summary>
        /// <param name="exObjectConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Object" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Immer <see cref="null"/>.
        /// </returns>
        public object Visit(ExObjectConfigItem exObjectConfigItem, Func<object> optionalCall = null)
        {
            
            if (optionalCall() is null)
                return null;

            var obj = optionalCall();
            var objType = obj.GetType();
            
            foreach (var prop in objType.GetProperties())
            {
                if (
                    !prop.CanWrite
                    || prop.IsReadOnly()
                    )
                    continue;

                var key = prop.GetJsonPropertyName() ?? prop.Name;
                var config = exObjectConfigItem.Value[key];
                var propObj = prop.GetValue(obj);
                var configureAs = prop.ShouldConfigureAs();
                
                if (configureAs != null)
                {
                    try
                    {
                        propObj = IDeserializableJsonExtensions.FromJson((propObj as string), configureAs);
                    }
                    catch (Exception e)
                    {

                        List<IopCodesChip> chips = new List<IopCodesChip>();

                        try
                        {
                            chips.AddRange( GetOpCodeChips(System.IO.Directory.GetCurrentDirectory() + "\\bin\\Debug\\netstandard2.1\\OpCodeDlls"));
                        }
                        catch (Exception exception)
                        {
                            
                        }

                        try
                        {
                            chips.AddRange(GetOpCodeChips(System.IO.Directory.GetCurrentDirectory() + "\\bin\\Debug\\net5.0\\OpCodeDlls"));
                        }
                        catch (Exception e1)
                        {
                        }

                        
                        chips.ForEach(chip =>
                        {
                            if ((propObj as string) != null)
                            {
                                if ((propObj as string).Contains("\"ChipType\": \"" + chip.ChipType))
                                    propObj = BissDeserialize.FromJson(propObj as string, chip.GetType());
                            }
                        });
                        
                        
                    }
                }
                    
                if (configureAs != null && (configureAs.IsInterface || configureAs.ShouldBeTreatedAsInterface()))
                {
                    var tempConfig = _templateCacher.GetObjectTemplateFor(propObj.GetType().FullName);
                    tempConfig.HadConfigureAsAttribute = true;
                    config = tempConfig;
                    exObjectConfigItem.Value[key] = config;
                }

                config.Accept(this, () => propObj);
            }

            foreach (var field in objType.GetFields())
            {
                if (
                    field.IsLiteral
                    || field.IsInitOnly
                    || field.IsReadOnly()
                    )
                    continue;

                var key = field.GetJsonPropertyName() ?? field.Name;
                var config = exObjectConfigItem.Value[key];
                var fieldObj = field.GetValue(obj);
                var configureAs = field.ShouldConfigureAs();
                if (configureAs != null)
                    fieldObj = IDeserializableJsonExtensions.FromJson((fieldObj as string), configureAs);
                if (configureAs != null && (configureAs.IsInterface || configureAs.ShouldBeTreatedAsInterface()))
                {
                    var tempConfig = _templateCacher.GetObjectTemplateFor(fieldObj.GetType().FullName);
                    tempConfig.HadConfigureAsAttribute = true;
                    config = tempConfig;
                    exObjectConfigItem.Value[key] = config;
                }

                config.Accept(this, () => fieldObj);
            }

            return null;
        }

        /// <summary>
        /// Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExCollectionConfigItem" /> instanz.
        /// </summary>
        /// <param name="exCollectionConfigItem">Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type: <see cref="T:System.Collections.IEnumerable" />.</param>
        /// <param name="optionalCall">Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.</param>
        /// <returns>
        /// Immer <see cref="null"/>.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object Visit(ExCollectionConfigItem exCollectionConfigItem, Func<object> optionalCall = null)
        {
            if (exCollectionConfigItem.ReadOnly)
                return null;

            if (exCollectionConfigItem.ItemTemplate is ExObjectConfigItem template && template.IsInterface)
            {
                throw new NotImplementedException();
                return null;
            }

            exCollectionConfigItem.Value.Clear();

            

            try
            {
                foreach (var item in (IEnumerable<object>)optionalCall())
                {
                    var clone = Biss.Serialize.BissDeserialize.FromJson<IExConfigItem>(exCollectionConfigItem.ItemTemplate.ToJson());
                    clone.Accept(this, () => item);
                    exCollectionConfigItem.Value.Add(clone);
                }
            }
            catch (Exception e)   /////////////////////////////// Matt
            {
                var obj =optionalCall.Invoke();
                var type = obj.GetType();
                var count = type.GetProperty("Count");
                var cou = (int)count.GetValue(obj);
                if(!(cou == 0))
                {
                    try
                    {
                        foreach (var item in (IEnumerable<object>)optionalCall())
                        {
                            var clone = BissDeserialize.FromJson<IExConfigItem>(exCollectionConfigItem.ItemTemplate.ToJson());
                            clone.Accept(this, () => item);
                            exCollectionConfigItem.Value.Add(clone);
                        }
                    }
                    catch (Exception exception)
                    {
                        if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            Type itemType = type.GetGenericArguments()[0]; // use this...

                            if (itemType == typeof(byte))
                            {
                                IList<byte> coll = (IList<byte>) obj;
                                foreach (var item in coll)
                                {
                                    var clone = BissDeserialize.FromJson<IExConfigItem>(exCollectionConfigItem.ItemTemplate.ToJson());
                                    clone.Accept(this, () => item);
                                    exCollectionConfigItem.Value.Add(clone);
                                }
                            }
                            else
                            {
                                throw exception;
                            }
                        }
                        else
                        {
                            throw exception;
                        }



                    }
                    
                }
            }
            

            return null;
        }
        #endregion


        public static List<IopCodesChip> GetOpCodeChips(string path)
        {
            List<IopCodesChip> opCodesChips = new List<IopCodesChip>();
            List<Assembly> assemblies = new List<Assembly>();

            string[] filepaths = System.IO.Directory.GetFiles(path);

            filepaths.ToList().Where(file => file.EndsWith("dll", StringComparison.CurrentCulture) && !file.EndsWith("ConfigLibrary.dll", StringComparison.CurrentCulture) && !file.EndsWith("ExchangeLibrary.dll", StringComparison.CurrentCulture))
                .ToList().ForEach(filepath => assemblies.Add(Assembly.LoadFile(filepath)));

            assemblies.ForEach(assembly => (assembly.GetTypes()).ToList().ForEach(type =>
            {
                if (type.GetInterfaces().Contains(typeof(IopCodesChip)))
                    opCodesChips.Add((IopCodesChip)Activator.CreateInstance(type));
            }));

            return opCodesChips;
        }
    }
    
}
