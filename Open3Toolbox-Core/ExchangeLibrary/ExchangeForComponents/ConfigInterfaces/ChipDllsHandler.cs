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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using OpCodesDllsLibrary;

namespace ExchangeLibrary.ConfigInterfaces
{
    using System;

    /// <summary>
    ///     This class handles the configChip-dlls from a given directory
    /// </summary>
    public static class ChipDllsHandler
    {
        /// <summary>
        ///     Gets the Opcode classes from the dlls from the given path
        /// </summary>
        /// <param name="path">The path from the dlls</param>
        /// <returns>The list of the found opcode chips</returns>
        public static List<IopCodesChip> GetOpCodeChips(string path)
        {
            List<IopCodesChip> opCodesChips = new List<IopCodesChip>();
            List<Assembly> assemblies = new List<Assembly>();

            string[] filepaths = Directory.GetFiles(path);

            filepaths.ToList().Where(file => file.EndsWith("dll", StringComparison.CurrentCulture) && !file.EndsWith("ConfigLibrary.dll", StringComparison.CurrentCulture) && !file.EndsWith("ExchangeLibrary.dll", StringComparison.CurrentCulture))
                .ToList().ForEach(filepath => assemblies.Add(Assembly.LoadFile(filepath)));

            assemblies.ForEach(assembly => (assembly.GetTypes()).ToList().ForEach(type =>
            {
                if (type.GetInterfaces().Contains(typeof(IopCodesChip)))
                {
                    opCodesChips.Add((IopCodesChip) Activator.CreateInstance(type));
                }
            }));

            return opCodesChips;
        }
    }
}