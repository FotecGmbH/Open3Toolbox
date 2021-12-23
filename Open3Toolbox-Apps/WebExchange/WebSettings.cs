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

using Biss.Apps.Service.Push;

namespace WebExchange
{
    public class WebSettings :
        IAppServiceSettingPush
    {
        private static WebSettings _current;

        public WebSettings()
        {
            ConnectionString = "";
            ConnectionStringDb = "TODO";
            ConnectionStringDbServer = "TODO";
            ConnectionStringUser = "TODO";
            ConnectionStringUserPwd = "TODO";
            SendEMailAs = "TODO";
            SendEMailAsDisplayName = "TODO";
            SendGridApiKey = "TODO";
            ServerKey = "TODO";
        }

        #region Properties

        #region IAppServiceSettingPush

        public string ServerKey { get; set; }

        #endregion

        #endregion

        public static WebSettings Current()
        {
            if (_current == null)
            {
                _current = new WebSettings();
            }

            return _current;
        }

        #region IAppSettingsDataBaseOrNoSql

        public string ConnectionString { get; set; }
        public string ConnectionStringDb { get; set; }
        public string ConnectionStringDbServer { get; set; }
        public string ConnectionStringUser { get; set; }
        public string ConnectionStringUserPwd { get; set; }

        #endregion

        #region IAppSettingsEMail

        public string SendEMailAs { get; set; }
        public string SendEMailAsDisplayName { get; set; }
        public string SendGridApiKey { get; set; }

        #endregion
    }
}