using DBAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace GadgetsAPI.Web.Models
{
    public class Gadget
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Model { get; set; } = null!;
        public Category? Category { get; set; }
        public Producer? Producer { get; set; }
        public decimal Price { get; set; }

        public Gadget()
        {

        }

        public Gadget(GadgetModel model)
        {
            Id = model.Id;
            Model = model.Model;
            Price = model.Price;

            if (model.Category != null)
            {
                Category = new Category()
                {
                    Id = model.Category.Id,
                    CategoryName = model.Category.CategoryName
                };
            }

            if (model.Producer != null)
            {
                Producer = new Producer()
                {
                    Id = model.Producer.Id,
                    ProducerName = model.Producer.ProducerName
                };
            }
        }
    }
}
