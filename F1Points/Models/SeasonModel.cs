using System.Collections.Generic;
using System.ComponentModel;

namespace F1Points.Models
{
    public class SeasonModel
    {
        public int Season { get; set; }
        public IEnumerable<RaceModel> Races { get; set; }
        public List<DriverSeasonStandingsModel> DriverStandings { get; set; }
        public List<ConstructorSeasonStandingsModel> ConstructorStandings { get; set; }

        public SeasonModel()
        {
            DriverStandings = new List<DriverSeasonStandingsModel>();
            ConstructorStandings = new List<ConstructorSeasonStandingsModel>();
        }
    }
}