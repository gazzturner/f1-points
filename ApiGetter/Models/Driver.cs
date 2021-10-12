namespace ApiGetter.Models
{
    public class Driver
    {
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Name => GivenName + " " + FamilyName;
        public string Url { get; set; }
    }
}