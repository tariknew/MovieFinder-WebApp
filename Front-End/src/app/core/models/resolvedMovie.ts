import { Movie } from '../models/view-models/movie';

export interface ResolvedMovie {
  movie: Movie;
  backUrl: string;
}
