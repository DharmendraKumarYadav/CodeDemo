using System;

namespace BDB.ViewModels
{
    public class BikeUserRatingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Review { get; set; }
        public string Rating { get; set; }
        public string BikeName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsPublished { get; set; }
        public int BikeId { get; set; }
    }
}
