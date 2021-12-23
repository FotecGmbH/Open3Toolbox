// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       23.11.2021 10:48
// Developer:     Matthias Mandl
// Project:       ExchangeLibrary
// 
// Released under MIT

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExchangeLibrary.ConfigInterfaces;
using ExchangeLibrary.ExchangeData;
using ExchangeLibrary.SensorData.GeneratedModels.Generated;
using ExchangeLibrary.SensorData.Visitors;
using Newtonsoft.Json.Linq;
using OpCodesDllsLibrary;

namespace ExchangeLibrary.Sensordata.Visitors
{
    /// <summary>
    ///     Visitor for converting readed data of the chips to human redable values
    /// </summary>
    public class InterfaceValuesVisitor : IInterfaceVisitor
    {
        /// <summary>
        ///     The locker
        /// </summary>
        private readonly object _locker;

        /// <summary>
        ///     The opcode chips
        /// </summary>
        private readonly List<IopCodesChip> _opCodeChips;

        /// <summary>
        ///     The data which should be converted
        /// </summary>
        private byte[] _data;

        /// <summary>
        ///     Initializes a new instance
        /// </summary>
        /// <param name="data">The data which should be converted</param>
        /// <param name="locker">The locker</param>
        public InterfaceValuesVisitor(byte[] data, object locker)
        {
            _data = data;
            _locker = locker;

            var currPath = Directory.GetCurrentDirectory();

            _opCodeChips = ChipDllsHandler.GetOpCodeChips(currPath + "\\OpCodeDlls");
        }

        #region Properties

        /// <summary>
        ///     The list of result values from converting
        /// </summary>
        public List<MeasurementValue> Values { get; } = new List<MeasurementValue>();

        #endregion

        #region Interface Implementations

        /// <summary>
        ///     Visiting the i2c interface
        /// </summary>
        /// <param name="visitable">A i2c interface</param>
        public void Visit(I2CInterface visitable)
        {
            if (visitable == null)
            {
                throw new ArgumentNullException(nameof(visitable));
            }

            visitable.I2cChips.ToList().ForEach(chip =>
            {
                var jobj = JObject.Parse(chip.ChipType);
                var jprop = jobj.Property("ChipType", StringComparison.CurrentCulture) ?? throw new ArgumentException("opcodeschip was invalid");
                var currChipType = (string) jprop.Value!;

                IopCodesChip currOpCodeChip = _opCodeChips.First(opCodeChip => opCodeChip.ChipType == currChipType);

                chip.Measurements.ForEach(
                    mea =>
                    {
                        if (_data.Length < 8)
                        {
                            return;
                        }

                        var currData = _data.Take(currOpCodeChip.BytesToRead).ToArray();

                        var val = currOpCodeChip.ParsingReadedValueToValue(currData, mea.Port);

                        lock (_locker)
                        {
                            Values.Add(new MeasurementValue(mea.Id, val, DateTime.Now, -1, -1, -1)); //TODO
                        }

                        _data = _data.Skip(8).ToArray();
                    });
            });
        }

        /// <summary>
        ///     Visiting the gpio interface
        /// </summary>
        /// <param name="visitable">A gpio interface</param>
        public void Visit(GpioInterface visitable)
        {
            if (visitable == null)
            {
                throw new ArgumentNullException(nameof(visitable));
            }

            visitable.GpioChips.ToList().ForEach(chip =>
            {
                var jobj = JObject.Parse(chip.ChipType);
                var jprop = jobj.Property("ChipType", StringComparison.CurrentCulture) ?? throw new ArgumentException("opcodeschip was invalid");
                var currChipType = (string) jprop.Value!;

                IopCodesChip currOpCodeChip = _opCodeChips.First(opCodeChip => opCodeChip.ChipType == currChipType);

                chip.Measurements.ForEach(
                    mea =>
                    {
                        if (_data.Length == 0)
                        {
                            return;
                        }

                        byte[] currData = {_data[0]}; // bool

                        var val = currOpCodeChip.ParsingReadedValueToValue(currData, mea.Port);

                        lock (_locker)
                        {
                            Values.Add(new MeasurementValue(mea.Id, val, DateTime.Now, -1, -1, -1)); //TODO
                        }

                        _data = _data.Skip(1).ToArray();
                    });
            });
        }

        #endregion
    }
}