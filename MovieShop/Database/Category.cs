using System.ComponentModel.DataAnnotations;

namespace MovieShop.Database
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string CategoryName { get; set; }
    }
}
