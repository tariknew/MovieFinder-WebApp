using System.ComponentModel.DataAnnotations;

namespace MovieShop.Models.Requests
{
    public class CategoryUpdateRequest
    {
        [Required(ErrorMessage = "CategoryName is a Required field")]
        public string CategoryName { get; set; }
    }
}
