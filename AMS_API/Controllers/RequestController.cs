using AMS_API.DataAccess;
using AMS_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        [HttpPost]
        [Route("getrequest")]
        public ActionResult GetRequest([FromBody]int requestId)
        {
            var connectionString = @"server=S-SAMANTARAY\SQLEXPRESS;database=Ams_DB;trusted_connection=true";
            var requestDA = new RequestDataAccess(connectionString);
            var requestResult = requestDA.GetRequest(requestId);
            if (requestResult != null)
            {
                return Ok(requestResult);
            }

            return BadRequest("No Data Found !!");
        }

        [HttpGet]
        [Route("getrequests")]
        public ActionResult GetRequests()
        {
            var connectionString = @"server=S-SAMANTARAY\SQLEXPRESS;database=Ams_Db;trusted_connection=true";
            var requestDA = new RequestDataAccess(connectionString);
            var requestResult = requestDA.GetRequests();
            if (requestResult != null)
            {
                return Ok(requestResult);
            }

            return BadRequest("No Data Found !!");
        }

        [HttpPost]
        [Route("raiserequests")]
        public ActionResult InsertRequests(Request request)
        {
            var connectionString = @"server=S-SAMANTARAY\SQLEXPRESS;database=Ams_Db;trusted_connection=true";
            var requestDA = new RequestDataAccess(connectionString);
            var requestResult = requestDA.InsertRequest(request);
            if (requestResult !=  null)
            {
                return Ok(requestResult);
            }

            return BadRequest("No Data Found !!");
        }
        [HttpPost]
        [Route("insertassets")]
        public ActionResult InsertAssets(Request request)
        {
            var connectionString = @"server=S-SAMANTARAY\SQLEXPRESS;database=Ams_Db;trusted_connection=true";
            var requestDA = new RequestDataAccess(connectionString);
            var requestResult = requestDA.InsertRequest(request);
            if (requestResult != null)
            {
                return Ok(requestResult);
            }

            return BadRequest("No Data Found !!");
        }

        [HttpPost]
        [Route("modifyrequests")]
        public ActionResult UpdateRequests(Request request)
        {
            var connectionString = @"server=S-SAMANTARAY\SQLEXPRESS;database=Ams_Db;trusted_connection=true";
            var requestDA = new RequestDataAccess(connectionString);
            var requestResult = requestDA.UpdateRequest(request);
            if (requestResult > 0)
            {
                return Ok(requestResult);
            }

            return BadRequest("No Record Updated !!");
        }

        [HttpPost]
        [Route("getasset")]
        public ActionResult GetAsset([FromBody] int assetCategoryId)
        {
            var connectionString = @"server=S-SAMANTARAY\SQLEXPRESS;database=Ams_Db;trusted_connection=true";
            var requestDA = new RequestDataAccess(connectionString);
            var assetResult = requestDA.GetAsset(assetCategoryId);
            if (assetResult != null)
            {
                return Ok(assetResult);
            }

            return BadRequest("No Data Found !!");
        }

        [HttpGet]
        [Route("getcategory")]
        public ActionResult GetCategories()
        {
            var connectionString = @"server=S-SAMANTARAY\SQLEXPRESS;database=Ams_Db;trusted_connection=true";
            var requestDA = new RequestDataAccess(connectionString);
            var categoryResult = requestDA.GetAssetCategory();
            if (categoryResult != null)
            {
                return Ok(categoryResult);
            }

            return BadRequest("No Data Found !!");
        }

        [HttpGet]
        [Route("GetAssetType")]
        public ActionResult GetAssetCategory(int id)
        {
            var connectionString = @"server=S-SAMANTARAY\SQLEXPRESS;database=Ams_Db;trusted_connection=true";
            var requestDA = new RequestDataAccess(connectionString);
            var categoryResult = requestDA.GetAssetTypes(id);
            if (categoryResult != null)
            {
                return Ok(categoryResult);
            }

            return BadRequest("No Data Found !!");
        }
    }
}
