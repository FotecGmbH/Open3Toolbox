// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       20.07.2021 11:04
// Developer:     Istvan Galfi
// Project:       WebServer
// 
// Released under MIT

using Microsoft.AspNetCore.Mvc;

namespace WebServer.Controllers
{
    /// <summary>
    ///     <para>Controller API Zugriffe</para>
    ///     Klasse ApiController. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    [ApiController]
    public class ApiController : ControllerBase
    {
        ///// <summary>
        /////     Datenbank neu erstellen
        ///// </summary>
        ///// <returns></returns>
        //[AllowAnonymous]
        //[HttpGet]
        //[Route("api/ResetDataBase")]
        //public bool ResetDatabase()
        //{
        //    return DatabaseContext.CreateAndFillUp(true);
        //}
    }
}