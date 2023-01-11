namespace DBAccess.Models
{
    public class GadgetModel
    {
        public int Id { get; set; }
        public string Model { get; set; } = null!;
        public int? CategoryId { get; set; }
        public CategoryModel? Category { get; set; }
        public int? ProducerId { get; set; }
        public ProducerModel? Producer { get; set; }
        public decimal Price { get; set; }
    }
}
