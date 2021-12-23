// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       20.07.2021 11:04
// Developer:     Istvan Galfi
// Project:       BaseApp.View
// 
// Released under MIT

using System;
using System.Linq;
using System.Threading.Tasks;
using Biss.Serialize;
using ExConfigExchange.Models;
using ExConfigExchange.Models.Interfaces;
using ExConfigExchange.Services;
using ExConfigExchange.Services.Interfaces;
using Xamarin.Forms;

namespace BaseApp.ViewGenerator.ConfigurationTool
{
    /// <summary>
    ///     Diese Implementierung vom <see cref="IExConfigVisitor{TOut}" /> dient als einen Renderer für
    ///     <see cref="IExConfigItem" /> Implementationen.
    /// </summary>
    /// <seealso cref="ExConfigExchange.Services.Interfaces.IExConfigVisitor&lt;Xamarin.Forms.View&gt;" />
    public class ExConfigItemUIVisitor : IExConfigVisitor<Xamarin.Forms.View>
    {
        /// <summary>
        ///     Der interface resolver.
        /// </summary>
        private readonly Func<ExObjectConfigItem, Task<ExObjectConfigItem>> _interfaceResolver;

        /// <summary>
        ///     Der collection template visitor
        /// </summary>
        private readonly ExCollectionConfigItemTemplateVisitor collectionTemplateVisitor;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="interfaceResolver">The interface resolver.</param>
        /// <exception cref="System.ArgumentNullException">interfaceResolver</exception>
        public ExConfigItemUIVisitor(Func<ExObjectConfigItem, Task<ExObjectConfigItem>> interfaceResolver)
        {
            _interfaceResolver = interfaceResolver ?? throw new ArgumentNullException(nameof(interfaceResolver));
            collectionTemplateVisitor = new ExCollectionConfigItemTemplateVisitor(interfaceResolver);
        }

        /// <summary>
        ///     Holt den Title/angezeigte Name vom <see cref="IExConfigItem" /> Instanz.
        ///     Sollte in der Zukunft zusammen mit einem Resolver verwendet werden sodass <see cref="IExConfigItem.DisplayKey" />s
        ///     zu deren Übersetzte Version gemappt werden.
        /// </summary>
        /// <param name="field">Der field.</param>
        /// <returns>Der übersetzte Title/angezeigte Name.</returns>
        private string GetTitle(IExConfigItem field)
        {
            return field.DisplayKey; // Later this should try to call a translator
        }

        /// <summary>
        ///     Convinience Method.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        private StackLayout GetLayout(IExConfigItem field) =>
            new StackLayout
            {
                Margin = new Thickness(2),
                Children =
                {
                    new Label {Text = GetTitle(field), FontSize = 18, TextColor = Color.Black},
                }
            };

        /// <summary>
        ///     Convinience Method.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>An Entry</returns>
        private Entry GetEntry(IExConfigItem field) =>
            new Entry {IsEnabled = !field.ReadOnly, TextColor = Color.Black};

        /// <summary>
        ///     Setzt den Binding mit einem Bindable.
        /// </summary>
        /// <typeparam name="T">To which Type should be binded</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="prop">The property.</param>
        /// <param name="way">The way.</param>
        /// <param name="bindable">The bindable.</param>
        private void SetBindingWithBindable<T>(BindableObject obj, BindableProperty prop, BindingMode way, Bindable<T> bindable)
        {
            obj.SetBinding(prop, new Binding(nameof(Bindable<T>.Proxy), way)
                                 {
                                     Source = bindable,
                                 });
        }

        #region Interface Implementations

        /// <summary>
        ///     Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExStringConfigItem" /> instanz.
        /// </summary>
        /// <param name="exStringConfigItem">
        ///     Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom
        ///     Type: <see cref="T:System.String" />.
        /// </param>
        /// <param name="optionalCall">
        ///     Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind
        ///     <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.
        /// </param>
        /// <returns>
        ///     Der <see cref="Xamarin.Forms.View" /> Element, das dieser <see cref="IExConfigItem" /> Implementierung darstellen
        ///     soll.
        /// </returns>
        public Xamarin.Forms.View Visit(ExStringConfigItem exStringConfigItem, Func<Xamarin.Forms.View> optionalCall = null!)
        {
            if (exStringConfigItem.Hidden)
            {
                return new StackLayout {IsVisible = false};
            }

            var entry = GetEntry(exStringConfigItem);
            var bindable = new Bindable<string>(
                v =>
                {
                    // Validate me in the future
                    exStringConfigItem.Value = v;
                },
                () => exStringConfigItem.Value);
            SetBindingWithBindable(entry, Entry.TextProperty, BindingMode.TwoWay, bindable);
            var layout = GetLayout(exStringConfigItem);
            layout.Children.Add(entry);

            var optional = optionalCall?.Invoke();
            if (!(optional is null))
            {
                layout.Children.Add(optional);
            }

            return layout;
        }

        /// <summary>
        ///     Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExUrlConfigItem" /> instanz.
        /// </summary>
        /// <param name="exUrlConfigItem">
        ///     Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type:
        ///     <see cref="T:System.Uri" />.
        /// </param>
        /// <param name="optionalCall">
        ///     Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind
        ///     <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.
        /// </param>
        /// <returns>
        ///     Der <see cref="Xamarin.Forms.View" /> Element, das dieser <see cref="IExConfigItem" /> Implementierung darstellen
        ///     soll.
        /// </returns>
        public Xamarin.Forms.View Visit(ExUrlConfigItem exUrlConfigItem, Func<Xamarin.Forms.View> optionalCall = null!)
        {
            if (exUrlConfigItem.Hidden)
            {
                return new StackLayout {IsVisible = false};
            }

            var entry = GetEntry(exUrlConfigItem);
            var bindable = new Bindable<string>(
                v =>
                {
                    if (!string.IsNullOrWhiteSpace(v) && Uri.IsWellFormedUriString(v, UriKind.Absolute))
                    {
                        exUrlConfigItem.Value = new Uri(v);
                    }
                },
                () => exUrlConfigItem.Value.ToString());
            SetBindingWithBindable(entry, Entry.TextProperty, BindingMode.TwoWay, bindable);
            var layout = GetLayout(exUrlConfigItem);
            layout.Children.Add(entry);

            var optional = optionalCall?.Invoke();
            if (!(optional is null))
            {
                layout.Children.Add(optional);
            }

            return layout;
        }

        /// <summary>
        ///     Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExByteConfigItem" /> instanz.
        /// </summary>
        /// <param name="exByteConfigItem">
        ///     Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom
        ///     Type: <see cref="T:System.Byte" />.
        /// </param>
        /// <param name="optionalCall">
        ///     Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind
        ///     <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.
        /// </param>
        /// <returns>
        ///     Der <see cref="Xamarin.Forms.View" /> Element, das dieser <see cref="IExConfigItem" /> Implementierung darstellen
        ///     soll.
        /// </returns>
        public Xamarin.Forms.View Visit(ExByteConfigItem exByteConfigItem, Func<Xamarin.Forms.View> optionalCall = null!)
        {
            if (exByteConfigItem.Hidden)
            {
                return new StackLayout {IsVisible = false};
            }

            var entry = GetEntry(exByteConfigItem);
            var bindable = new Bindable<string>(
                v =>
                {
                    if (!string.IsNullOrWhiteSpace(v) && byte.TryParse(v, out var val))
                    {
                        exByteConfigItem.Value = val;
                    }
                },
                () => exByteConfigItem.Value.ToString());
            SetBindingWithBindable(entry, Entry.TextProperty, BindingMode.TwoWay, bindable);
            var layout = GetLayout(exByteConfigItem);
            layout.Children.Add(entry);

            var optional = optionalCall?.Invoke();
            if (!(optional is null))
            {
                layout.Children.Add(optional);
            }

            return layout;
        }

        /// <summary>
        ///     Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExIntConfigItem" /> instanz.
        /// </summary>
        /// <param name="exIntConfigItem">
        ///     Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom Type:
        ///     <see cref="T:System.Int32" />.
        /// </param>
        /// <param name="optionalCall">
        ///     Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind
        ///     <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.
        /// </param>
        /// <returns>
        ///     Der <see cref="Xamarin.Forms.View" /> Element, das dieser <see cref="IExConfigItem" /> Implementierung darstellen
        ///     soll.
        /// </returns>
        public Xamarin.Forms.View Visit(ExIntConfigItem exIntConfigItem, Func<Xamarin.Forms.View> optionalCall = null!)
        {
            if (exIntConfigItem.Hidden)
            {
                return new StackLayout {IsVisible = false};
            }

            var entry = GetEntry(exIntConfigItem);
            var bindable = new Bindable<string>(
                v =>
                {
                    if (!string.IsNullOrWhiteSpace(v) && int.TryParse(v, out var val))
                    {
                        exIntConfigItem.Value = val;
                    }
                },
                () => exIntConfigItem.Value.ToString());
            SetBindingWithBindable(entry, Entry.TextProperty, BindingMode.TwoWay, bindable);
            var layout = GetLayout(exIntConfigItem);
            layout.Children.Add(entry);

            var optional = optionalCall?.Invoke();
            if (!(optional is null))
            {
                layout.Children.Add(optional);
            }

            return layout;
        }

        /// <summary>
        ///     Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExLongConfigItem" /> instanz.
        /// </summary>
        /// <param name="exLongConfigItem">
        ///     Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom
        ///     Type: <see cref="T:System.Int64" />.
        /// </param>
        /// <param name="optionalCall">
        ///     Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind
        ///     <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.
        /// </param>
        /// <returns>
        ///     Der <see cref="Xamarin.Forms.View" /> Element, das dieser <see cref="IExConfigItem" /> Implementierung darstellen
        ///     soll.
        /// </returns>
        public Xamarin.Forms.View Visit(ExLongConfigItem exLongConfigItem, Func<Xamarin.Forms.View> optionalCall = null!)
        {
            if (exLongConfigItem.Hidden)
            {
                return new StackLayout {IsVisible = false};
            }

            var entry = GetEntry(exLongConfigItem);
            var bindable = new Bindable<string>(
                v =>
                {
                    if (!string.IsNullOrWhiteSpace(v) && int.TryParse(v, out var val))
                    {
                        exLongConfigItem.Value = val;
                    }
                },
                () => exLongConfigItem.Value.ToString());
            SetBindingWithBindable(entry, Entry.TextProperty, BindingMode.TwoWay, bindable);
            var layout = GetLayout(exLongConfigItem);
            layout.Children.Add(entry);

            var optional = optionalCall?.Invoke();
            if (!(optional is null))
            {
                layout.Children.Add(optional);
            }

            return layout;
        }

        /// <summary>
        ///     Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExFloatConfigItem" /> instanz.
        /// </summary>
        /// <param name="exFloatConfigItem">
        ///     Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom
        ///     Type: <see cref="T:System.Single" />.
        /// </param>
        /// <param name="optionalCall">
        ///     Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind
        ///     <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.
        /// </param>
        /// <returns>
        ///     Der <see cref="Xamarin.Forms.View" /> Element, das dieser <see cref="IExConfigItem" /> Implementierung darstellen
        ///     soll.
        /// </returns>
        public Xamarin.Forms.View Visit(ExFloatConfigItem exFloatConfigItem, Func<Xamarin.Forms.View> optionalCall = null!)
        {
            if (exFloatConfigItem.Hidden)
            {
                return new StackLayout {IsVisible = false};
            }

            var entry = GetEntry(exFloatConfigItem);
            var bindable = new Bindable<string>(
                v =>
                {
                    if (!string.IsNullOrWhiteSpace(v) && float.TryParse(v, out var val))
                    {
                        exFloatConfigItem.Value = val;
                    }
                },
                () => exFloatConfigItem.Value.ToString());
            SetBindingWithBindable(entry, Entry.TextProperty, BindingMode.TwoWay, bindable);
            var layout = GetLayout(exFloatConfigItem);
            layout.Children.Add(entry);

            var optional = optionalCall?.Invoke();
            if (!(optional is null))
            {
                layout.Children.Add(optional);
            }

            return layout;
        }

        /// <summary>
        ///     Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExDoubleConfigItem" /> instanz.
        /// </summary>
        /// <param name="exDoubleConfigItem">
        ///     Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom
        ///     Type: <see cref="T:System.Double" />.
        /// </param>
        /// <param name="optionalCall">
        ///     Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind
        ///     <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.
        /// </param>
        /// <returns>
        ///     Der <see cref="Xamarin.Forms.View" /> Element, das dieser <see cref="IExConfigItem" /> Implementierung darstellen
        ///     soll.
        /// </returns>
        public Xamarin.Forms.View Visit(ExDoubleConfigItem exDoubleConfigItem, Func<Xamarin.Forms.View> optionalCall = null!)
        {
            if (exDoubleConfigItem.Hidden)
            {
                return new StackLayout {IsVisible = false};
            }

            var entry = GetEntry(exDoubleConfigItem);
            var bindable = new Bindable<string>(
                v =>
                {
                    if (!string.IsNullOrWhiteSpace(v) && double.TryParse(v, out var val))
                    {
                        exDoubleConfigItem.Value = val;
                    }
                },
                () => exDoubleConfigItem.Value.ToString());
            SetBindingWithBindable(entry, Entry.TextProperty, BindingMode.TwoWay, bindable);
            var layout = GetLayout(exDoubleConfigItem);
            layout.Children.Add(entry);

            var optional = optionalCall?.Invoke();
            if (!(optional is null))
            {
                layout.Children.Add(optional);
            }

            return layout;
        }

        /// <summary>
        ///     Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExBoolConfigItem" /> instanz.
        /// </summary>
        /// <param name="exBoolConfigItem">
        ///     Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom
        ///     Type: <see cref="T:System.Boolean" />.
        /// </param>
        /// <param name="optionalCall">
        ///     Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind
        ///     <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.
        /// </param>
        /// <returns>
        ///     Der <see cref="Xamarin.Forms.View" /> Element, das dieser <see cref="IExConfigItem" /> Implementierung darstellen
        ///     soll.
        /// </returns>
        public Xamarin.Forms.View Visit(ExBoolConfigItem exBoolConfigItem, Func<Xamarin.Forms.View> optionalCall = null!)
        {
            if (exBoolConfigItem.Hidden)
            {
                return new StackLayout {IsVisible = false};
            }

            var checkBox = new CheckBox {IsEnabled = !exBoolConfigItem.ReadOnly};
            var bindable = new Bindable<bool>(
                v => exBoolConfigItem.Value = v,
                () => exBoolConfigItem.Value);
            SetBindingWithBindable(checkBox, CheckBox.IsCheckedProperty, BindingMode.TwoWay, bindable);
            var layout = GetLayout(exBoolConfigItem);
            layout.Children.Add(checkBox);

            var optional = optionalCall?.Invoke();
            if (!(optional is null))
            {
                layout.Children.Add(optional);
            }

            return layout;
        }

        /// <summary>
        ///     Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExEnumConfigItem" /> instanz.
        /// </summary>
        /// <param name="exEnumConfigItem">
        ///     Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom
        ///     Type: <see cref="!:enum" />.
        /// </param>
        /// <param name="optionalCall">
        ///     Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind
        ///     <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.
        /// </param>
        /// <returns>
        ///     Der <see cref="Xamarin.Forms.View" /> Element, das dieser <see cref="IExConfigItem" /> Implementierung darstellen
        ///     soll.
        /// </returns>
        public Xamarin.Forms.View Visit(ExEnumConfigItem exEnumConfigItem, Func<Xamarin.Forms.View> optionalCall = null!)
        {
            if (exEnumConfigItem.Hidden)
            {
                return new StackLayout {IsVisible = false};
            }

            var picker = new Picker {IsEnabled = !exEnumConfigItem.ReadOnly};
            picker.ItemsSource = exEnumConfigItem.Value.Select(v => v.DisplayKey).ToList();
            picker.SelectedItem = exEnumConfigItem.Selected is null ? exEnumConfigItem.Value.First().DisplayKey : exEnumConfigItem.Selected.DisplayKey;
            var bindable = new Bindable<string>(
                v => exEnumConfigItem.Selected = exEnumConfigItem.Value.First(i => i.DisplayKey == v),
                () => exEnumConfigItem.Selected is null ? exEnumConfigItem.Value.First().DisplayKey : exEnumConfigItem.Selected.DisplayKey);
            SetBindingWithBindable(picker, Picker.SelectedItemProperty, BindingMode.TwoWay, bindable);
            var layout = GetLayout(exEnumConfigItem);
            layout.Children.Add(picker);

            return layout;
        }

        /// <summary>
        ///     Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExObjectConfigItem" /> instanz.
        /// </summary>
        /// <param name="exObjectConfigItem">
        ///     Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung vom
        ///     Type: <see cref="T:System.Object" />.
        /// </param>
        /// <param name="optionalCall">
        ///     Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind
        ///     <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.
        /// </param>
        /// <returns>
        ///     Der <see cref="Xamarin.Forms.View" /> Element, das dieser <see cref="IExConfigItem" /> Implementierung darstellen
        ///     soll.
        /// </returns>
        public Xamarin.Forms.View Visit(ExObjectConfigItem exObjectConfigItem, Func<Xamarin.Forms.View> optionalCall = null!)
        {
            if (exObjectConfigItem.Hidden)
            {
                return new StackLayout {IsVisible = false};
            }

            // Needs a bindable in order to update...
            if (exObjectConfigItem.IsInterface)
            {
                var button = new Button {IsEnabled = !exObjectConfigItem.ReadOnly, Text = $"Show options for: {GetTitle(exObjectConfigItem)}"};
                button.Clicked += async (s, e) =>
                {
                    var hadConfigureAs = exObjectConfigItem.HadConfigureAsAttribute;
                    var newInstance = await _interfaceResolver(exObjectConfigItem);
                    if (!(newInstance is null))
                    {
                        BissClone.Clone(newInstance, exObjectConfigItem);
                    }

                    exObjectConfigItem.HadConfigureAsAttribute = hadConfigureAs;
                };
                return button;
            }

            var layout = GetLayout(exObjectConfigItem);

            foreach (var field in exObjectConfigItem.Value.Values)
            {
                if (field is null)
                {
                    continue;
                }

                layout.Children.Add(field.Accept(this));
            }

            var optional = optionalCall?.Invoke();
            if (!(optional is null))
            {
                layout.Children.Add(optional);
            }

            var frame = new Frame();
            frame.Content = layout;

            return frame;
        }

        /// <summary>
        ///     Besucht den spezifizierten <see cref="T:ExConfigExchange.Models.ExCollectionConfigItem" /> instanz.
        /// </summary>
        /// <param name="exCollectionConfigItem">
        ///     Der <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> Abbildung
        ///     vom Type: <see cref="T:System.Collections.IEnumerable" />.
        /// </param>
        /// <param name="optionalCall">
        ///     Mit dieser <see cref="!:Func" /> kann man werte vom Eltern zum Kind
        ///     <see cref="T:ExConfigExchange.Models.Interfaces.IExConfigItem" /> verpasst werden.
        /// </param>
        /// <returns>
        ///     Der <see cref="Xamarin.Forms.View" /> Element, das dieser <see cref="IExConfigItem" /> Implementierung darstellen
        ///     soll.
        /// </returns>
        public Xamarin.Forms.View Visit(ExCollectionConfigItem exCollectionConfigItem, Func<Xamarin.Forms.View> optionalCall = null!)
        {
            /// <see cref="ExObjectConfigItem"/>-Collections werden anders behandelt.
            if (exCollectionConfigItem.Hidden || exCollectionConfigItem.ItemTemplate is ExObjectConfigItem)
            {
                return new StackLayout {IsVisible = false};
            }

            var layout = GetLayout(exCollectionConfigItem);
            var addButton = new Button {IsVisible = !exCollectionConfigItem.ReadOnly, Text = "+", FontSize = 24, FontAttributes = FontAttributes.Bold};
            addButton.Clicked += async (s, e) =>
            {
                var toAdd = await exCollectionConfigItem.ItemTemplate.Accept(collectionTemplateVisitor);
                exCollectionConfigItem.Value.Add(toAdd);
                var container = new StackLayout();

                Xamarin.Forms.View DeleteCall()
                {
                    var deleteButton = new Button {Text = "-", FontSize = 24, FontAttributes = FontAttributes.Bold};
                    deleteButton.Clicked += (s, e) =>
                    {
                        exCollectionConfigItem.Value.Remove(toAdd);
                        layout.Children.Remove(container);
                    };
                    return deleteButton;
                }

                ;

                var viewItem = toAdd.Accept(this, DeleteCall);
                container.Children.Add(viewItem);
                layout.Children.Add(container);
            };
            layout.Children.Add(addButton);

            var optional = optionalCall?.Invoke();
            if (!(optional is null))
            {
                layout.Children.Add(optional);
            }

            foreach (var field in exCollectionConfigItem.Value)
            {
                if (field is null)
                {
                    continue;
                }

                var container = new StackLayout();

                Xamarin.Forms.View DeleteCall()
                {
                    var deleteButton = new Button {Text = "-", FontSize = 24, FontAttributes = FontAttributes.Bold};
                    deleteButton.Clicked += (s, e) =>
                    {
                        exCollectionConfigItem.Value.Remove(field);
                        layout.Children.Remove(container);
                    };
                    return deleteButton;
                }

                var viewItem = field.Accept(this, DeleteCall);
                container.Children.Add(viewItem);
                layout.Children.Add(container);
            }

            var frame = new Frame();
            frame.Content = layout;

            return frame;
        }

        #endregion
    }
}