using System.Collections.Generic;

namespace BDB
{
    public class Filters
    {
        public string? brand { get; set; }
        public string? budget { get; set; }
        public string? bodyStyle { get; set; }
        public string? displacement { get; set; }
        public string? category { get; set; }
    }

    public class SearchFilterModel
    {
        public int page { get; set; }
        public int limit { get; set; }
        public string? sort { get; set; }
        public int type { get; set; }
        public Filters? filters { get; set; }
    }
    //public class SearchFilterModel
    //{
    //    public int BrandId { get; set; }
    //    public int BudgetId { get; set; }
    //    public int DisplacementId { get; set; }
    //    public int BodyStyleId { get; set; }
    //    public int CategotyId { get; set; }
    //}
    public class BikeFilterList {
        public BikeFilterList()
        {
            Items=new List<FilterListItem>();
  
        }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public List<FilterListItem> Items { get; set; }
        public string Value { get; set; }
    }
    public class FilterListItem
    {
        public string Slug { get; set; }
        public string Name { get; set; }
        public string Count { get; set; }
    }
}
