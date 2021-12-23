// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Erstversion vom 18.02.2021 15:00
// Entwickler       Matthias Mandl, Sebastian Szvetecz
// Projekt          Dataskop

namespace ExConfigExchange.JsonUtils
{

    /// <summary>
    /// Dieses Attribute mapt Kind klassen einer (abstract) Klasse/Interface zu deren konkrete implementationen bei der Serializierung.
    /// Bei serializierung wird der <see cref="ExJsonInheritanceConverter{TBase}"/> verwendet.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.3.7.0 (Newtonsoft.Json v12.0.0.0)")]
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Interface, AllowMultiple = true)]
    internal class ExJsonInheritanceAttribute : System.Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExJsonInheritanceAttribute"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="type">The type.</param>
        public ExJsonInheritanceAttribute(string key, System.Type type)
        {
            Key = key;
            Type = type;
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key { get; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public System.Type Type { get; }
    }

}
