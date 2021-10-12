namespace ApiGetter.Models
{
    public class Result
    {
        public int Position { get; set; }
        public string PositionText { get; set; }
        public bool Finished => int.TryParse(PositionText, out var position);
        public Driver Driver { get; set; }
        public Constructor Constructor { get; set; }
    }
}