// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       02.07.2021 10:19
// Developer:     Matthias Mandl
// Project:       WebAPI
// 
// Released under MIT

using System.Linq;

namespace WebAPI.Extensions
{
    using System;

    public static class ReflectionExtensions
    {
        public static bool ImplementsInterface(this Type implementing, Type implemented)
        {
            return implementing.GetInterfaces().Contains(implemented);
        }
    }
}