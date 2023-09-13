namespace BDB
{
    public class FeaturedBikeTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string BrandName { get; set; }
        public string BodyStyle { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public int BikeId { get; set; }
        public int TypeId { get; set; }
    }

    public class SimilarBikeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public string BodyStyle { get; set; }
        public string Category { get; set; }
        public int BikeId { get; set; }
    }
    public class SimilarBikeRequest
    {
        public int Id { get; set; }
        public int SimilarBikeId { get; set; }
        public int BikeId { get; set; }
    }
}
