using MovieShop.Database;

namespace MovieShop.ViewModels
{
    public class VMMovieActor
    {
        public int Id { get; set; }
        public int MovieID { get; set; }
        public int ActorID { get; set; }
        public string? CharacterName { get; set; }
        public string? FormattedActorBirthDate => Actor?.BirthDate.ToString("yyyy-MM-dd");
        public string? FormattedActorBirthPlace => Actor?.Country?.CountryName;
        public string? FormattedActorRealName => $"{Actor?.FirstName} {Actor?.LastName}";
        public string? ActorBiography => Actor?.Biography;
        public string? ActorIMDbLink => Actor?.IMDbLink;
        public byte[]? ActorPhoto => Actor?.Photo;
        public virtual Actor Actor { get; set; }
    }
}
