using Microsoft.AspNetCore.Mvc;

namespace BlogWebApi.Controllers
{
    [ApiController]
    [Route("Health-Check")]
    public class HomeController : ControllerBase
    {
        [HttpGet("")] 
        public ActionResult Get()
        {
            return Ok
                (
                    new
                    {
                        response = "It looks like everything is fine"
                    }
                );
        }
    }
}
