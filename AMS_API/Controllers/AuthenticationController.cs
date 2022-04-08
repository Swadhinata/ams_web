using AMS_API.DataAccess;
using AMS_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost]
        [Route("signin")]
        public JsonResult GetUser(User user)
        {
            var connectionString = @"server=S-SAMANTARAY\SQLEXPRESS;database=Ams_DB;trusted_connection=true";
            var userDA = new UserDataAccess(connectionString);
            var userResult = userDA.GetUser(user);
            if (userResult != null)
            {
                return new JsonResult(userResult);
            }

            return new JsonResult("User creadential is not correct !!");
        }
    }
}
