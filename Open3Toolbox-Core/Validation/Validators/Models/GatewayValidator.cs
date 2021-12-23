﻿// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       02.07.2021 10:19
// Developer:     Istvan Galfi
// Project:       Validation
// 
// Released under MIT

using ExchangeLibrary.DataBaseData.Entities;
using FluentValidation;

namespace Validation.Validators.Models
{
    /// <summary>
    ///     The validator for <see cref="TablePublishedGateway" />.
    /// </summary>
    /// <seealso cref="FluentValidation.AbstractValidator{Exchange.Entities.GatewayDB}" />
    public class GatewayValidator : AbstractValidator<TablePublishedGateway>
    {
    }
}