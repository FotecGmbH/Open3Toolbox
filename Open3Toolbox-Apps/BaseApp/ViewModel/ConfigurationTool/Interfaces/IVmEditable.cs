// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       30.08.2021 15:55
// Developer:     Istvan Galfi
// Project:       BaseApp
// 
// Released under MIT

namespace BaseApp.ViewModel.ConfigurationTool.Interfaces
{
    /// <summary>
    ///     Diese Klasse (Momentan) ist nur da, sodass man nur einmal auf Type statt x-Mal (Momentan 3) casten muss beim
    ///     <see cref="VmConfigurationTool.SelectedValue" />.
    /// </summary>
    public interface IVmEditable
    {
        /// <summary>
        ///     Edit Mode Beginnen.
        /// </summary>
        public void BeginEdit();

        /// <summary>
        ///     Änderungen Speichern.
        /// </summary>
        public void SaveChanges();

        /// <summary>
        ///     Änderungen wegwerfen.
        /// </summary>
        public void UndoChanges();
    }
}