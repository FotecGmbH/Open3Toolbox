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

using System;

namespace WebServer.Models
{
    /// <summary>
    ///     <para>Login Model</para>
    ///     Klasse LoginModel. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class LoginModel
    {
        #region Properties

        /// <summary>
        ///     Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     Ob eingeloggt
        /// </summary>
        public bool IsLoggedIn { get; set; }

        #endregion
    }
}