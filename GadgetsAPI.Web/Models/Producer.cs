using System.ComponentModel.DataAnnotations;

namespace GadgetsAPI.Web.Models
{
    public class Producer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ProducerName { get; set; } = null!;
    }
}