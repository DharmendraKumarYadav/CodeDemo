namespace BDB.ViewModels.Common
{
    public class AreaViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
    }

    public class AreaModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PinCode { get; set; }
        public int CityId { get; set; }
        public string City { get; set; }
    }
}
