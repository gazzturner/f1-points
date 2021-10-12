using System.Collections.Generic;

namespace F1Points.Models
{
    public class DriverPointsModel
    {
        public DriverModel Driver { get; set; }
        public IEnumerable<ResultModel> TopResults { get; set; }
    }
}