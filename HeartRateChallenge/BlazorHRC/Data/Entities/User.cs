using System.ComponentModel.DataAnnotations;

namespace BlazorHRC.Data.Entities
{
    public class User
    {
        [Key]
        public string Username { get; set; }

        public string Password { get; set; }
        public string Name { get; set; }
        public int Zone1LowerBound { get; set; }
        public int Zone1UpperBound { get; set; }
        public int Zone2LowerBound { get; set; }
        public int Zone2UpperBound { get; set; }
        public int Zone3LowerBound { get; set; }
        public int Zone3UpperBound { get; set; }
        public int Zone4LowerBound { get; set; }
        public int Zone4UpperBound { get; set; }
        public int Zone5LowerBound { get; set; }
        public int Zone5UpperBound { get; set; }
    }
}
