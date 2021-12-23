// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       20.07.2021 11:04
// Developer:     Istvan Galfi
// Project:       AdminWebApp.Client
// 
// Released under MIT

using System.Threading.Tasks;
using BaseApp;
using Biss.Apps.Blazor.Pages;

namespace AdminWebApp.Client.BissApps
{
    /// <summary>
    ///     Basis für RazorPages
    /// </summary>
    public class ProjectRazorPage : BissRazorPage
    {
        /// <summary>
        ///     Method invoked when the component is ready to start, having received its
        ///     initial parameters from its parent in the render tree.
        ///     Override this method if you will perform an asynchronous operation and
        ///     want the component to refresh when that operation is completed.
        /// </summary>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            //DcInfoStorageBrowser.SetJsRuntime(JsRuntime);
            await base.OnInitializedAsync().ConfigureAwait(true);
        }

        /// <summary>
        ///     Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">
        ///     Set to <c>true</c> if this is the first time
        ///     <see cref="M:Microsoft.AspNetCore.Components.ComponentBase.OnAfterRender(System.Boolean)" /> has been invoked
        ///     on this component instance; otherwise <c>false</c>.
        /// </param>
        /// <remarks>
        ///     The <see cref="M:Microsoft.AspNetCore.Components.ComponentBase.OnAfterRender(System.Boolean)" /> and
        ///     <see cref="M:Microsoft.AspNetCore.Components.ComponentBase.OnAfterRenderAsync(System.Boolean)" /> lifecycle methods
        ///     are useful for performing interop, or interacting with values received from <c>@ref</c>.
        ///     Use the <paramref name="firstRender" /> parameter to ensure that initialization work is only performed
        ///     once.
        /// </remarks>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
        }
    }

    /// <summary>
    ///     Basis für RazorPages
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProjectRazorPage<T> : BissRazorPage<T>
        where T : VmProjectBase
    {
        /// <summary>
        ///     Method invoked when the component is ready to start, having received its
        ///     initial parameters from its parent in the render tree.
        ///     Override this method if you will perform an asynchronous operation and
        ///     want the component to refresh when that operation is completed.
        /// </summary>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            //DcInfoStorageBrowser.SetJsRuntime(JsRuntime);
            //await VmProjectBase.InitializeApp(EnumAppType.Admin).ConfigureAwait(true);
            await base.OnInitializedAsync().ConfigureAwait(true);
        }
    }
}