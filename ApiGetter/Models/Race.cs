using System.Collections.Generic;

namespace ApiGetter.Models
{
    public class Race
    {
        public string RaceName { get; set; }
        public List<Result> Results { get; set; }

        public Race()
        {
            Results = new List<Result>();
        }
    }
}