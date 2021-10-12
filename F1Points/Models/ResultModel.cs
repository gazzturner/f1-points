namespace F1Points.Models
{
    public class ResultModel
    {
        public int Position { get; set; }
        public DriverModel Driver { get; set; }
        public ConstructorModel Constructor { get; set; }
        public bool Finished { get; set; }
        public int Points { get; set; }
        public bool FastestLap { get; set; } 
    }
}