using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace F1Points.Models
{
    public class RaceModel
    {
        public string RaceName { get; set; }
        public IEnumerable<ResultModel> Results { get; set; }
    }
}