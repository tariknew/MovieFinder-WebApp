using System.ComponentModel.DataAnnotations;

namespace MovieShop.Models.Requests
{
    public class CategoryInsertRequest
    {
        [Required(ErrorMessage = "CategoryName is a Required field")]
        public string CategoryName { get; set; }
    }
}
