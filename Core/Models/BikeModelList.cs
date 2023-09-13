using Core;

namespace BDB
{
    public class BikesCurrentMonthList
    {
        public List<BikeModelList> ScotterBikes { get; set; }
        public List<BikeModelList> MilageBikes { get; set; }
        public List<BikeModelList> SportsBike { get; set; }
        public List<BikeModelList> CruisersBike { get; set; }
        public List<BikeModelList> ElectricBike { get; set; }
    }
    public class BikeFeaturedList
    {
        public List<BikeModelList> PopularBikes { get; set; }
        public List<BikeModelList> NewLaunches { get; set; }
        public List<BikeModelList> Upcoming { get; set; }
        public List<BikeModelList> ScotterBikes { get; set; }
        public List<BikeModelList> SportsBike { get; set; }
        public List<BikeModelList> CruisersBike { get; set; }
        public List<BikeModelList> ElectricBike { get; set; }
    }
    public class BikeModelList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShowRoomName { get; set; }
        public string[] Images { get; set; }
        public string BasicSpecification { get; set; }
        public List<BikeColourModelList> Colours { get; set; }
        public string BodyStyle { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Price { get; set; }

        public bool IsElectricBike { get; set; }
        public string Displacement { get; set; }
        public int TypeId { get; set; }
    }
    public class BikeModelPagedResult
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public List<BikeModelList> Result { get; set; }
        public List<BikeFilterList> Filters { get; set; }
  
    }
}
