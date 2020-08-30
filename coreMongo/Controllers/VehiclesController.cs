using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coreMongo.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace coreMongo.Controllers
{
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private IConfiguration configuration;
        public VehiclesController(IConfiguration configs)
        {
            configuration = configs;
        }

        [HttpPost]
        [Route("api/vehicle/getlist")]
        public IEnumerable<dynamic> GetList([FromBody] VehicleRequest vehicleRequest)
        {
            string strDatabase = "Vehicle";
            string strCollection = configuration.GetSection("Vehicle").GetSection("vehicleColl").Value;
            clsMongoDAL objMongo = new clsMongoDAL(strDatabase);
            Vehicle vehicle = new Vehicle();
            List<Vehicle> lstVehicles = new List<Vehicle>();
            string value = vehicleRequest.searchOn;

            try
            {
                //Validate request section... 

                // Count can be returned when page num = 1 so client side can pass page number.
                // e.g. total count 39 total pages whole number (total/size) 39/10 = 3 + 1, assuming 10 entries per page. 
                long total = objMongo.GetCount(strCollection, vehicleRequest.searchOn, vehicleRequest.searchValue);

                var dbResult = objMongo.GetWithPagination(strCollection, vehicleRequest.searchOn, vehicleRequest.searchValue, vehicleRequest.pageSize, vehicleRequest.pageNum);

                foreach (var strData in dbResult)
                {
                    vehicle = new Vehicle
                    {
                        vehicleId = Convert.ToString(strData.GetValue("_id")),
                        vehicleType = Convert.ToString(strData.GetValue("vehicleType")),
                        yearOfManu = Convert.ToInt32(strData.GetValue("yearOfManu")),
                        deviceId = Convert.ToString(strData.GetValue("deviceId")),
                        userId = Convert.ToString(strData.GetValue("userId"))
                    };
                    lstVehicles.Add(vehicle);
                }
            }
            catch (Exception ex)
            { 
            
            }
            return lstVehicles;
        }
      
    }
}
