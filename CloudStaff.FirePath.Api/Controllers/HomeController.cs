using System.Web.Http;

namespace CloudStaff.FirePath.Api.Controllers
{
    public class HomeController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Index()
        {
            return Ok("Cloudstaff Firepath API v0.1");
        }
    }
}
