using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coreMongo.Model
{
    public class Vehicle
    {
        public string vehicleId { get; set; }
        public string vehicleType { get; set; }
        public int yearOfManu { get; set; } = 1900;
        public string deviceId { get; set; }
        public string userId { get; set; }
    }
}
