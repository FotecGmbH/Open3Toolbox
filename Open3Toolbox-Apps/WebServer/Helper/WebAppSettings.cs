// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       20.07.2021 11:04
// Developer:     Istvan Galfi
// Project:       WebServer
// 
// Released under MIT

namespace WebServer.Helper
{
    /// <summary>
    ///     Einstellungen für die WebApplikation
    /// </summary>
    public class WebAppSettings
    {
        #region Properties

        /// <summary>
        ///     Secret für den JWT (JSON Web Token)
        /// </summary>
        public string Secret { get; set; }

        #endregion
    }
}