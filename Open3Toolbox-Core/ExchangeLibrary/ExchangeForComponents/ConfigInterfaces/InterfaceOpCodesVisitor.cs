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
using System.Linq;
using ExchangeLibrary.SensorData.GeneratedModels.Generated;
using ExchangeLibrary.SensorData.Visitors;
using Newtonsoft.Json.Linq;
using OpCodesDllsLibrary;

namespace ExchangeLibrary.ConfigInterfaces
{
    /// <summary>
    ///     Visitor for Interfaces (eg gpio i2c)
    ///     for handling getting the opcodes of chips
    /// </summary>
    public class InterfaceOpCodesVisitor : IInterfaceVisitor
    {
        /// <summary>
        ///     The config chips
        /// </summary>
        private readonly List<IopCodesChip> _chips;

        /// <summary>
        ///     Initializes a new instance of the visitor
        /// </summary>
        /// <param name="chips">The config chips</param>
        public InterfaceOpCodesVisitor(List<IopCodesChip> chips) => _chips = chips ?? throw new ArgumentNullException(nameof(chips));

        #region Properties

        /// <summary>
        ///     All collected opcodes
        /// </summary>
        public List<byte> OpCodes { get; } = new List<byte>();

        #endregion

        #region Interface Implementations

        /// <summary>
        ///     Visits the i2cInterface and its measurements and actors
        ///     will get the commands from the equal config chip
        /// </summary>
        /// <param name="visitable">The interface</param>
        public void Visit(I2CInterface visitable)
        {
            if (visitable == null)
            {
                throw new ArgumentNullException(nameof(visitable));
            }

            foreach (var i2CChip in visitable.I2cChips)
            {
                var jobj = JObject.Parse(i2CChip.ChipType);
                var jprop = jobj.Property("ChipType", StringComparison.CurrentCulture) ?? throw new ArgumentException("opcodeschip was invalid");
                var currChipType = (string) jprop.Value!;

                foreach (var opCodesChip in _chips)
                {
                    if (currChipType == opCodesChip.ChipType && opCodesChip.InterfaceType == OpCodesDllsLibrary.InterfaceType.i2CInterface)
                    {
                        foreach (var measurement in i2CChip.Measurements)
                        {
                            measurement.ReadOpCodes = opCodesChip.GetOpCodes((byte) measurement.Port).Item1.ToList();
                            OpCodes.AddRange(measurement.ReadOpCodes);
                        }


                        foreach (var actor in i2CChip.Actors)
                        {
                            actor.WriteOpCodes = opCodesChip.GetOpCodes((byte) actor.Port).Item2.ToList();
                            OpCodes.AddRange(actor.WriteOpCodes);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Visits the gpioInterface and its measurements and actors
        ///     will get the commands from the equal config chip
        /// </summary>
        /// <param name="visitable">The interface</param>
        public void Visit(GpioInterface visitable)
        {
            if (visitable == null)
            {
                throw new ArgumentNullException(nameof(visitable));
            }

            foreach (var gpioChip in visitable.GpioChips)
            {
                foreach (var opCodesChip in _chips)
                {
                    var jobj = JObject.Parse(gpioChip.ChipType);
                    var jprop = jobj.Property("ChipType", StringComparison.CurrentCulture) ?? throw new ArgumentException("opcodeschip was invalid");
                    var currChipType = (string) jprop.Value!;

                    if (currChipType == opCodesChip.ChipType && opCodesChip.InterfaceType == OpCodesDllsLibrary.InterfaceType.gpioInterface)
                    {
                        foreach (var measurement in gpioChip.Measurements)
                        {
                            measurement.ReadOpCodes = opCodesChip.GetOpCodes((byte) measurement.Port).Item1.ToList();
                            OpCodes.AddRange(measurement.ReadOpCodes);
                        }


                        foreach (var actor in gpioChip.Actors)
                        {
                            actor.WriteOpCodes = opCodesChip.GetOpCodes((byte) actor.Port).Item2.ToList();
                            OpCodes.AddRange(actor.WriteOpCodes);
                        }
                    }
                }
            }
        }

        #endregion
    }
}