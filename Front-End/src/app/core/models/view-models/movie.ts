export interface MovieActor {
  actorPhoto: Uint8Array | null;
  formattedActorRealName: string | null;
  characterName: string | null;
  formattedActorBirthDate: string | null;
  formattedActorBirthPlace: string | null;
  actorIMDbLink: string | null;
  actorBiography: string | null;
  actor: Actor | null;
}
export interface Actor {
  firstName: string;
  lastName: string;
  photo: Uint8Array | null;
  birthDate: string;
  countryID: number;
  imDbLink: string | null;
  biography: string | null;
}
export interface Movie {
  id: number;
  title: string;
  formattedReleaseDate: string;
  formattedDuration: string;
  directorName: string;
  image: Uint8Array | null;
  trailerLink: string | null;
  storyLine: string;
  formattedPrice: number;
  averageScore: number | null;
  categoriesNames: string;
  movieActors: MovieActor[];
}






