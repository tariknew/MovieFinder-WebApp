import {Component, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {Movie} from '@core/models/view-models/movie';
import {noImg} from '@shared/helpers/noImgFound';
import {DomSanitizer, SafeResourceUrl} from '@angular/platform-browser';
import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {StarRatingColor} from '@shared/components/star-rating/star-rating.component';
import {FormBuilder, FormGroup} from '@angular/forms';
import {ToastrService} from 'ngx-toastr';
import {environment} from '@env/environment';
import {catchError, first, tap} from 'rxjs/operators';
import {forkJoin, throwError} from 'rxjs';
import {MatPaginator} from '@angular/material';
import {MatDialog} from '@angular/material/dialog';
import {ActorDialogComponent} from '@shared/components/actor-dialog/actor-dialog.component';
import {JwtService} from '@app/auth/services/jwt.services';
import {FavoriteMoviesService} from '@app/movie/services/favourite-movies.service';

@Component({
  selector: 'app-movie-detail',
  templateUrl: './movie-detail.component.html',
  styleUrls: ['./movie-detail.component.scss']
})
export class MovieDetailComponent implements OnInit {
  // Rating
  rating = 0;
  starCount = 5;
  starColor: StarRatingColor = StarRatingColor.accent;

  // Varijabla gdje cu pohraniti objekt od 'user-a' iz storage
  userData = null;

  // Varijable koje cu koristiti za 'getReviews' API
  movieId: number;
  allReviews: any[] = []; // Polje za pohranu svih recenzija

  // Inicijalizacija forme
  reviewForm: FormGroup;

  // Varijabla koju cu koristiti za searchReviewByUserName metodu
  // U nju pohranjujem 'username' koji smo ukucali za event
  username: string;

  // Od Almera
  movie: Movie;
  backUrl: string;
  trailerUrl: SafeResourceUrl;
  isFavorite = false;
  showTooltip: false;


  // server side paging
  paginatorForm: FormGroup;
  totalReviews: number;
  @ViewChild(MatPaginator, {static: false}) paginator: MatPaginator;

  constructor(
    private router: Router,
    private dialog: MatDialog,
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private sanitizer: DomSanitizer,
    private http: HttpClient,
    private jwtService: JwtService,
    private toastrService: ToastrService,
    private favoriteMoviesService: FavoriteMoviesService
  ) {
  }

  ngOnInit() {
    const dataName = 'movie';
    const data = this.route.snapshot.data[dataName];
    if (data && data.movie.id) {
      this.movie = data.movie;
      this.backUrl = data.backUrl;

      if (this.movie.trailerLink) {
        const videoUrl = `https://www.youtube.com/embed/${this.movie.trailerLink}`;
        this.trailerUrl = this.sanitizer.bypassSecurityTrustResourceUrl(videoUrl);
      }
    }
    this.getMovieIdFromRoute();
    // Dobavi 'user-a' i nakon toga napravi formu
    this.getUser();
    this.checkIfMovieIsFavorite();
    this.defaultPagingValues();
    this.getAllReviews(this.movieId);
    // Postavi defaultnu vrijednost za 'rating' zvijezdice da bude 0 (nista nije obojeno)
    // Ovo vrijedi kada ucitamo stranicu
    this.rating = 0;
  }

  onPageChange(event: any) {
    this.paginatorForm.patchValue({
      pageNumber: event.pageIndex + 1,
      pageSize: event.pageSize
    });
    this.getAllReviews(this.movieId);
  }

  // Otvaranje dialoga (osnovne informacije o glumcu)
  openDialog(actorData: any): void {
    const dialogRef = this.dialog.open(ActorDialogComponent, {
      data: actorData
    });
  }

  defaultPagingValues() {
    this.paginatorForm = this.formBuilder.group({
      pageNumber: 1,
      pageSize: 3
    });
  }

  searchReviewByUserName(username: string): void {
    this.username = username;
    this.getAllReviews(this.movieId);
    this.setPaginatorPage();
  }

  setPaginatorPage(): void {
    this.paginatorForm.patchValue({
      pageNumber: 1,
    });
  }

  // Osvjezi stranicu prebaci me opet na isti URL
  refreshPage() {
    window.location.reload();
  }

  onSubmit() {
    // stop here if form is invalid
    // Ako forma nije popunjena i 'rating zvijezdice' ne dozvoli da se nastavi
    if (this.reviewForm.invalid && this.rating === 0) {
      return;
    }
    if (this.reviewForm.controls.inputComment.value.length < 4) {
      this.toastrService.error('Comment must be at least 4 characters long');
      return;
    }
    this.toastrService.clear();
    this.postReview()
      .pipe(first())
      .subscribe(
        () => {
          this.toastrService.success('Review has been posted succesfully!');
          // Za sad ne treba radi toga sto ima refresh page
          // this.setPaginatorPage();
          // this.getAllReviews(this.movieId);
        },
        error => {
          this.toastrService.error(error);
        }
      );
  }

  private createForm(): void {
    this.reviewForm = this.formBuilder.group({
      inputComment: [''],
    });
  }

  // Provjeri da li 'Input' sadrzi iskljucivo samo 81 karakter
  checkMaxLength(event) {
    const maxLength = 81;
    if (event.target.value.length > maxLength) {
      event.target.value = event.target.value.slice(0, maxLength);
    }
  }

  // Okidac koji mjenja rating (zvijezdice)
  onRatingChanged(rating) {
    this.rating = rating;
  }

  getUser() {
    const storedToken = localStorage.getItem('token');
    const storedUserId = localStorage.getItem('userId');
    if (storedToken) {
      const decodedToken = this.jwtService.getInfoFromToken(storedToken);
      const usernameFromToken = decodedToken.username;
      this.userData = {
        userId: JSON.parse(storedUserId),
        userName: usernameFromToken,
      };
      if (this.userData) {
        this.createForm();
      }
    }
  }


  postReview() {
    const apiUrl = `${environment.apiUrl}/Review/PostReview`;
    const requestBody = {
      userID: this.userData.userId,
      movieID: this.movieId,
      comment: this.reviewForm.get('inputComment').value,
      score: this.rating
    };
    // Naznačimo da šaljemo JSON podatke na server
    const headers = new HttpHeaders().set('Content-Type', 'application/json');
    return this.http.post(apiUrl, requestBody, {headers}).pipe(
      tap(() => {
        // Osvježi stranicu nakon uspješnog dodavanja recenzije
        this.refreshPage();
      }),
      catchError(this.handleError)
    );
  }

  getAllReviews(movieId: number) {
    const {pageNumber, pageSize} = this.paginatorForm.value;
    let apiUrl1 = `${environment.apiUrl}/Review/GetReviewsByMovieId?MovieID=${movieId}&pageNumber=${pageNumber}`;
    let apiUrl2 = `${environment.apiUrl}/Review/GetReviewsCount?MovieID=${movieId}`;
    // Dodajemo pretragu po korisničkom imenu ako je dostupno
    if (this.username != null) {
      apiUrl1 += `&UserName=${this.username}`;
      apiUrl2 += `&UserName=${this.username}`;
    }
    // Dohvat svih komentara sa servera
    forkJoin([
      this.http.get<any[]>(apiUrl1),
      this.http.get<number>(apiUrl2),
    ]).subscribe(
      ([reviews, reviewsCount]: [any[], number]) => {
        // Ovdje pohranjujemo objekt recenzije
        this.allReviews = reviews;
        // Ovo prati koliko ima recenzija (zavisno od filtera)
        // Potrebno za server-side paging
        this.totalReviews = reviewsCount;
      },
      (error) => this.toastrService.error(error),
    );
  }

  onDelete(reviewId: number) {
    const apiUrl = `${environment.apiUrl}/Review/${reviewId}`;
    // Poziv API za brisanje recenzije
    this.http.delete(apiUrl).subscribe(
      () => {
        this.getAllReviews(this.movieId);
        this.toastrService.success('Review has been deleted succesfully!');
        // Osvježi stranicu nakon uspješnog dodavanja recenzije
        this.refreshPage();
      },
      (error) => this.toastrService.error(error),
    );
  }

  getProfileImage(profileImage: string): string {
    if (profileImage) {
      return `data:image/jpeg;base64,${profileImage}`;
    } else {
      // Vratite putanju do slike iz vašeg direktorija
      return './assets/img/register-avatar.png';
    }
  }

  // Dobavi sliku glumca
  getActorProfileImage(profileImage: any): any {
    return `data:image/jpeg;base64,${profileImage}`;
  }

  // Dobavi sliku filma
  getMovieImage(movieImage: any): any {
    if (movieImage) {
      return `data:image/jpeg;base64,${movieImage}`;
    } else {
      return noImg;
    }
  }

  // Uzmi 'putanju', tjt. 'Id' filma iz linka
  private getMovieIdFromRoute(): void {
    const url = this.route.snapshot.url;
    // U varijablu 'movieId' pohranjujemo taj 'Id'
    this.movieId = +url[url.length - 1].path;
  }

  onBack(): void {
    this.router.navigate(['../'], {relativeTo: this.route});
  }
  checkIfMovieIsFavorite() {
    const userId = this.userData.userId;
    if (userId) {
      const movieId = this.movie.id;
      this.favoriteMoviesService.isMovieFavourite(userId, movieId).subscribe(
        (isFavourite: boolean) => {
          this.isFavorite = isFavourite;
        },
        error => {
          this.toastrService.error(error);
          this.isFavorite = false;
        }
      );
    }
  }

  toggleFavorite() {
    const movieId = this.movie.id;
    const userId = this.userData.userId;
    if (this.isFavorite) {
      this.removeFromFavorites(userId, movieId);
    } else {
      this.addToFavorites(userId, movieId);
    }
  }

  addToFavorites(userId: number, movieId: number): void {
    this.favoriteMoviesService.addToFavorites(userId, movieId).subscribe(
      () => {
        this.toastrService.success('The movie has been successfully added to favorites');
        this.isFavorite = true; // Ažuriraj status da je film omiljen
      },
      error => {
        this.toastrService.error(error);
      }
    );
  }

  removeFromFavorites(userId: number, movieId: number): void {
    this.favoriteMoviesService.removeFromFavorites(userId, movieId).subscribe(
      () => {
        this.toastrService.success('The movie has been successfully removed from favorites');
        this.isFavorite = false; // Ažuriraj status da film nije omiljen
      },
      error => {
        this.toastrService.error(error);
      }
    );
  }


  private handleError(err: HttpErrorResponse) {
    let errorMessage: string;
    if (err.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      errorMessage = `${err.error}`;
    }
    console.error(err);
    return throwError(errorMessage);
  }
}

