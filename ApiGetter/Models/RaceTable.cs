using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;

namespace ApiGetter.Models
{
    public class RaceTable
    {
        public int Season { get; set; }
        public List<Race> Races { get; set; }

        public RaceTable()
        {
            Races = new List<Race>();
        }
    }
}