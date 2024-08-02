using System.ComponentModel.DataAnnotations;

namespace MovieShop.Models.Requests
{
    public class AddToFavoritesRequest
    {
        [Required(ErrorMessage = "UserId is a Required field")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "MovieId is a Required field")]
        public int MovieId { get; set; }
    }
}

