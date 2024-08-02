using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieShop.Database
{
    public class MovieActor
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Movie")]
        public int MovieID { get; set; }
        public Movie Movie { get; set; }

        [ForeignKey("Actor")]
        public int ActorID { get; set; }
        public Actor Actor { get; set; }
        public string CharacterName { get; set; }
    }
}
