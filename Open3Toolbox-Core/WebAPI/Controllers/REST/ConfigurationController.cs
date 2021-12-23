using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebServer;

namespace WebAPI.Controllers.REST
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        [HttpGet]
        [Route("/api/config")]
        public async Task<IActionResult> Test()
        {
            ServerRemoteCalls cs = new ServerRemoteCalls();
            var res = await cs.GetDcExPublishedProjects(1,-1,12,20,-1,null);

            return Ok(res);
        }
    }
}
