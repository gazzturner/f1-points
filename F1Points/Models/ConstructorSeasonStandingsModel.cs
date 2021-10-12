using System.Diagnostics.Contracts;

namespace F1Points.Models
{
    public class ConstructorSeasonStandingsModel
    {
        public int Position { get; set; }
        public string Constructor { get; set; }
        public  int Points { get; set; }
    }
}