using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;
using coreMongo.Model;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace coreMongo.Controllers
{
    
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IConfiguration configuration;
        public UsersController(IConfiguration configs)
        {
            configuration = configs;
        }

        // GET: api/<UsersController>
        [HttpGet]
        [Route("api/index")]
        public string Get()
        {
            return "Please provide correct Command and params.";
        }

        // POST api/<UsersController>
        [HttpPost]
        [Route("api/user/add")]
        public IActionResult Add([FromForm] UserRequest userRequest, IFormFile formFile)
        {
            Response res = new Response();
            string strDatabase = "Vehicle";
            string strCollection = configuration.GetSection(strDatabase).GetSection("userColl").Value;
            bool blnReturn = false;
            string strImagesLocation = @"D:/tasks/coremongodemo/userimages/";
            string strImgExt = ".jpeg";
            try
            {
                #region Validates Input request 

                if (!ModelState.IsValid)
                {
                    var errorDesciption = (from item in ModelState
                                           where item.Value.Errors.Any()
                                           select item.Value.Errors[0].ErrorMessage).ToList();

                    res.Error = new ErrorResponse("ERR.PARAMETERS.INVALID", errorDesciption.First());
                    res.Data = new object();

                    return StatusCode(Convert.ToInt32(HttpStatusCode.BadRequest), res);
                }

                #endregion
                
                //string userId =Convert.ToString(ObjectId.GenerateNewId());
                clsMongoDAL objMongo = new clsMongoDAL(strDatabase);

                if (objMongo.blnCheckDocument(strCollection, "_id", userRequest.userId))
                {
                    // Return ERROR response as user id already exists. Try other user name. 
                    res.Error = new ErrorResponse("ERR.IN.ADD", "User Id already exists. Try other user name.");
                    res.Data = new object();
                    return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), res);
                }
                else
                {
                    var request = HttpContext.Request;
                    var postedFile = request.Form.Files[0];
                    var filePath = Path.Combine($"{strImagesLocation.Trim()}{userRequest.userId.Trim()}{strImgExt.Trim()}");
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                    }
                    BsonDocument bsnInsert = new BsonDocument {
                    { "_id", userRequest.userId },
                    { "name",userRequest.name },
                    { "mobileNumber", userRequest.mobileNumber },
                    { "photoPath",filePath}};

                    blnReturn = objMongo.blnInsertDocument(strCollection, bsnInsert);
                }
                if (blnReturn)
                {
                    res.Data = new object();
                    return StatusCode(Convert.ToInt32(HttpStatusCode.OK), res);
                }
                else
                {
                    res.Error = new ErrorResponse("ERR.IN.ADD", "Error in adding new user details.");
                    res.Data = new object();
                    return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), res);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, res);
            }
        }


        // POST api/<UsersController>
        [HttpPost]
        [Route("api/user/edit")]
        public IActionResult Edit([FromForm] UserRequest userRequest, IFormFile formFile)
        {
            Response res = new Response();
            string strDatabase = "Vehicle";
            string strCollection = configuration.GetSection(strDatabase).GetSection("userColl").Value;
            bool blnReturn = false;
            string strImagesLocation = configuration.GetSection(strDatabase).GetSection("imageLocation").Value;
            string strImgExt = ".jpeg";
            string userId = userRequest.userId;

            clsMongoDAL objMongo = new clsMongoDAL(strDatabase);
            try
            {
                var request = HttpContext.Request;
                var postedFile = request.Form.Files[0];
                var filePath = Path.Combine($"{strImagesLocation.Trim()}{userId.Trim()}{strImgExt.Trim()}");
                if (System.IO.File.Exists(filePath)) {
                    System.IO.File.Delete(filePath);
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                if (objMongo.blnCheckDocument(strCollection, "_id", userId))
                {
                    var varFilter = Builders<BsonDocument>.Filter.Eq("_id", userId);

                    UpdateDefinition<BsonDocument> updateDefinition = Builders<BsonDocument>.Update.Set("name", userRequest.name).Set("mobileNumber", userRequest.mobileNumber);

                    blnReturn = objMongo.blnUpdateDocument(strCollection, varFilter, updateDefinition);
                }
                if (blnReturn)
                {
                    res.Data = new object();
                    return StatusCode(Convert.ToInt32(HttpStatusCode.OK), res);
                }
                else
                {
                    res.Error = new ErrorResponse("ERR.IN.EDIT", "Error in updating user details.");
                    res.Data = new object();
                    return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), res);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, res);
            }
        }

      
      
    }
}
