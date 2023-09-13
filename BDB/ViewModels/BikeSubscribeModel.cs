namespace BDB.ViewModels
{
    public class BikeSubscribeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string BikeName { get; set; }
        public string City { get; set; }
        public string Remarks { get; set; }
    }
    public class BikeSubscribeModelRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string BikeVariantId { get; set; }
        public string CityId { get; set; }
        public int DealerShowRoomId { get; set; }
        public string Remarks { get; set; }
    }
}
