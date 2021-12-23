// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       29.11.2021 11:01
// Developer:     Istvan Galfi
// Project:       Exchange
// 
// Released under MIT

using System;
using System.Collections.Generic;
using System.Globalization;
using Biss.Apps.Interfaces;
using Biss.Apps.Model;

namespace Exchange.Resources
{
    /// <summary>
    ///     <para>Culture der Resource Files via Code setzen</para>
    ///     Klasse Language. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class Language : IAppLanguage
    {
        /// <summary>
        ///     Unterstütze Sprachen (erste Sprache ist die Default Sprache welche verwendet wird als Fallback)
        /// </summary>
        public static readonly IReadOnlyCollection<string> SupportedLanguages = new List<string> {"de", "en"};

        /// <summary>
        ///     Aktuelle Kultur
        /// </summary>
        public static CultureInfo CurrentCulture = CultureInfo.CurrentCulture;

        /// <summary>
        ///     Aktuelle Kultur des Gerätes
        /// </summary>
        public static CultureInfo CurrentDeviceCulture = null!;

        /// <summary>
        ///     Derzeitiger Text
        /// </summary>
        private static ExLanguageContent? _currentText;

        #region Properties

        /// <summary>
        ///     Texte welche im Apps.Base verwendet werden
        /// </summary>
        public static ExLanguageContent GetText
        {
            get
            {
                if (_currentText == null)
                {
                    _currentText = new ExLanguageContent(
                        ResView.Command_Back,
                        ResView.Command_Continue,
                        ResView.Command_Login,
                        ResView.Command_ResendPassword,
                        ResView.MsgBox_CancelText,
                        ResView.MsgBox_DataNotSaved,
                        ResView.MsgBox_HeaderNoInternet,
                        ResView.MsgBox_NewPasswordSent,
                        ResView.MsgBox_NoInternet,
                        ResView.MsgBox_NoText,
                        ResView.MsgBox_OkText,
                        ResView.MsgBox_SaveError,
                        ResView.MsgBox_SaveSuccess,
                        ResView.MsgBox_ServerFail,
                        ResView.MsgBox_ServerTimeout,
                        ResView.MsgBox_ServerTokenFail,
                        ResView.MsgBox_ServerMultiConnectionFail,
                        ResView.MsgBox_YesText,
                        ResView.MsgBoxHeader_DataNotSaved,
                        ResView.MsgBoxHeader_SaveError,
                        ResView.MsgBoxHeader_SaveSuccess,
                        ResView.MsgBoxHeader_ServerRestError,
                        ResView.MsgBoxNewUpdate,
                        ResView.MsgBoxNewUpdateError,
                        ResView.MsgBoxNewUpdateMandatory,
                        ResView.MsgBoxTitleUpdateAvailable,
                        ResView.MsgBoxCameraAccess,
                        ResView.MsgBoxCameraAccessTitle,
                        ResView.MsgBoxCameraAccessInfo,
                        ResView.MsgBoxCameraAccessInfoTitle,
                        ResView.MsgBoxFileAccess,
                        ResView.MsgBoxFileAccessTitle,
                        ResView.MsgBoxFileAccessInfo,
                        ResView.MsgBoxFileAccessInfoTitle,
                        ResView.MsgBoxLocationAccess,
                        ResView.MsgBoxLocationAccessTitle,
                        ResView.MsgBoxLocationAccessInfo,
                        ResView.MsgBoxLocationAccessInfoTitle
                    );
                }

                return _currentText;
            }
        }

        #endregion

        /// <summary>
        ///     Resource Files auf bestimmte Kultur setzen
        /// </summary>
        /// <param name="culture">Die Kultur</param>
        public static void SetLanguageStatic(CultureInfo culture)
        {
            CurrentCulture = culture;
            ResView.Culture = culture;
            ResViewMain.Culture = culture;
            ResViewMenu.Culture = culture;
            ResWebCommon.Culture = culture;

            _currentText = null;
        }

        #region Interface Implementations

        /// <summary>
        ///     Erhalten des derzeitigen Textes
        /// </summary>
        /// <returns>Derzeitiger Text</returns>
        public ExLanguageContent GetLanguageContent()
        {
            return GetText;
        }

        /// <summary>
        ///     Resource Files auf bestimmte Kultur setzen
        /// </summary>
        /// <param name="culture">Die Kultur</param>
        public void SetLanguage(CultureInfo culture)
        {
            CurrentDeviceCulture = culture;
            SetLanguageStatic(culture);
        }

        #endregion
    }
}