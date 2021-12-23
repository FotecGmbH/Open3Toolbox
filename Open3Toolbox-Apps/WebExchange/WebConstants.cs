// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       20.07.2021 11:04
// Developer:     Istvan Galfi
// Project:       WebExchange
// 
// Released under MIT

using System.Diagnostics.CodeAnalysis;
using Biss.CsBuilder.Sql;
using Biss.Email;

namespace WebExchange
{
    /// <summary>
    ///     <para>Konstanten für WebProjekte</para>
    ///     Klasse Constants. (C) 2020 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    [SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "<Pending>")]
    public static class WebConstants
    {
        /// <summary>
        ///     Aktuelle WebSettings
        /// </summary>
        public static WebSettings CurrentWebSettings = WebSettings.Current();

        /// <summary>
        ///     DB Connection String
        /// </summary>
        public static string ConnectionString = string.IsNullOrEmpty(WebSettings.Current().ConnectionString)
            ? new CsBuilderSql(WebSettings.Current().ConnectionStringDbServer, WebSettings.Current().ConnectionStringDb, WebSettings.Current().ConnectionStringUser, WebSettings.Current().ConnectionStringUserPwd, SqlCommonStandardApplicationName.EntityFramework).ToString()
            : WebSettings.Current().ConnectionString;


        /// <summary>
        ///     Biss Email mit Sendgrid Key
        /// </summary>
        public static BissEMail Email = new BissEMail(new SendGridCredentials
                                                      {
                                                          ApiKeyV3 = WebSettings.Current().SendGridApiKey,
                                                      });
    }
}