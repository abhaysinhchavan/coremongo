using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace coreMongo.Model
{
    public class VehicleRequest
    {
        // Search Types equal/ range / lt/gt etc. Values to search searchValue / minValue / maxValue 
        [Required(AllowEmptyStrings = false, ErrorMessage = "Search Type cannot be empty")]
        public string searchType { get; set; } = "equal";
        public string searchOn { get; set; } = "vehicleNum";
        public string searchValue { get; set; } = "";
        public int minValue { get; set; } = 0;
        public int maxValue { get; set; } = 0;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Page Size cannot be empty")]
        public int pageSize { get; set; } = 0;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Page Number cannot be empty")]
        public int pageNum { get; set; } = 1; 

    }
}
