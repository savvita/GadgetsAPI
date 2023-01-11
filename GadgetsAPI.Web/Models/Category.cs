using System.ComponentModel.DataAnnotations;

namespace GadgetsAPI.Web.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string CategoryName { get; set; } = null!;
    }
}