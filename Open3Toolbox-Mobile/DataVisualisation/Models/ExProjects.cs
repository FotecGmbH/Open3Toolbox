// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Erstversion vom 23.12.2021 12:29
// Entwickler      Manuel Fasching
// Projekt         DataVisualisation
//
// Released under MIT

using System.Collections.Generic;

namespace DataVisualisation
{
    /// <summary>
    /// Logic Views for User
    /// </summary>
    public class ExUserLogicViews
    {
        #region Properties

        public List<ExUserLogicView> LstUserLogicViews { get; set; }

        #endregion
    }

    /// <summary>
    /// Logic View
    /// </summary>
    public class ExUserLogicView
    {
        #region Properties

        public int Id { get; set; }
        public string Name { get; set; }
        public List<ExLogicViewMeasurement> UserLogicViews { get; set; }

        #endregion
    }

    /// <summary>
    /// Logic View Measurement
    /// </summary>
    public class ExLogicViewMeasurement
    {
        #region Properties

        public long Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }

        public List<MeasurementValue> MeasurementValues { get; set; }

        #endregion
    }
}