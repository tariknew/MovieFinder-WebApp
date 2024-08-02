import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MovieParameterService } from '../../../services/movie-parameter.service';
import { GetAllCategoriesEndPoint } from '@core/endpoints/category/getallcategories-endpoint/getallcategories.endpoint';
import { GetLastPriceEndPoint } from '@core/endpoints/movie/getlastprice-endpoint/getlastprice.endpoint';
import { GetAllEndPoint } from '@core/endpoints/movie/getall-endpoint/getall.endpoint';
import { Movie } from '@core/models/view-models/movie';
import { ToastrService } from 'ngx-toastr';
import { Category } from '@core/models/view-models/category';
import { Options } from '@angular-slider/ngx-slider';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material';
import { forkJoin } from 'rxjs';
import { SearchObject } from '@core/models/requests/searchObject';
@Component({
  selector: 'app-movie-select',
  templateUrl: './movie-select.component.html',
  styleUrls: ['./movie-select.component.scss'],
  // Need to remove view encapsulation so that the custom tooltip style defined in
  // `tooltip-custom-class-example.css` will not be scoped to this component's view.
  encapsulation: ViewEncapsulation.None
})
export class MovieSelectComponent implements OnInit {

  pageTitle = 'Movie List';
  load = true;

  moviesData: Movie[] = [];
  categoryData: Category[] = [];
  categoryID: number;

  // slider
  minValue = 0;
  maxValue = 0;
  options: Options = {
    floor: 0,
    ceil: 0
  };

  // server side paging
  paginatorForm: FormGroup;
  totalMovies: any;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  // filters
  category: Category[] = [];
  defaultCategory: Category = {
    id: 0,
    categoryName: 'All'
  };
  orderBy: string;
  title: string;
  constructor(
    private toastrService: ToastrService,
    private movieGetAll: GetAllEndPoint,
    private fb: FormBuilder,
    private movieGetLastPrice: GetLastPriceEndPoint,
    private moviesGetAllCategories: GetAllCategoriesEndPoint,
    private movieParameterService: MovieParameterService,
    private route: ActivatedRoute
  ) { }
  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.pageTitle = 'Movie List';
      this.getFilters();
      this.getAllCategories();
      this.GetMovieLastPrice();
      this.defaultPagingValues();
    });
  }
  private getFilters(): void {
    this.orderBy = this.movieParameterService.orderBy;
    this.title = this.movieParameterService.filterByTitle;
  }
  GetMovieLastPrice(): void {
    this.movieGetLastPrice.getMovieLastPrice().subscribe(
      (data: number) => {
        this.options.ceil = data;
        this.updateSliderOptions(this.options.ceil);
      },
    );
  }
  onPageChange(event: any) {
    this.paginatorForm.patchValue({
      pageNumber: event.pageIndex + 1,
      pageSize: event.pageSize
    });
    this.filterAndSortMovies();
  }
  updateSliderOptions(lastPrice: number) {
    return this.options = {
      ceil: lastPrice,
      translate: (value: number): string => {
        return '€ ' + value;
      }
    };
  }
  defaultPagingValues() {
    this.paginatorForm = this.fb.group({
      pageNumber: 1,
      pageSize: 5
    });
    this.setDefaultMovies();
  }
  // Postavi paginator index na 0 i 'pageNumber' na 1
  // Sada ce ovo biti prva stranica
  setPaginatorPage(): void {
    this.paginatorForm.patchValue({
      pageNumber: 1,
    });
    this.paginator.pageIndex = 0;
  }
  setSliderValue(): void {
    // Nakon slider eventa vrati me na pocetnu stranicu
    this.setPaginatorPage();
    this.filterAndSortMovies();
  }
  getAllCategories(): void {
    this.moviesGetAllCategories.getAllCategories().subscribe(
      (categories: Category[]) => {
        this.categoryData = categories;
        // 'unshift' se koristi za dodavanje jednog ili vise elemenata na pocetak niza
        this.categoryData.unshift(this.defaultCategory);
      },
      (error) => this.toastrService.error(error),
    );
  }
  // Metoda koja na prvom ucitavanju stranice prikazuje listu filmova koje smo definisali
  setDefaultMovies(): void {
    const { pageNumber, pageSize } = this.paginatorForm.value;
    const searchObject: SearchObject = {
      movietitle: undefined,
      pricefrom: undefined,
      priceto: undefined,
      categoryid: 0,
      sortedcolumn: 'LastAddedMovies',
      isdescending: true,
      pagenumber: pageNumber,
    };
    this.movieGetAll.getMovies(searchObject)
      .subscribe(
        (movies: Movie[]) => {
          this.moviesData = movies;
          // Postavi defaultno da na prvoj stranici da izbacuje sve filmove u rasponu 'priceFrom -> priceTo'
          this.minValue = 0;
          this.maxValue = this.options.ceil;
          this.filterAndSortMovies();
        },
        (error) =>  this.toastrService.error(error),
        () => this.load = false
      );
  }
  filterAndSortMovies(): void {
    const { pageNumber, pageSize } = this.paginatorForm.value;
    let sortedColumn: string;
    let isDescending: boolean;
    if (this.orderBy === 'Last Added') {
      sortedColumn = 'LastAddedMovies';
      isDescending = true;
    } else if (this.orderBy === 'Most Popular') {
      sortedColumn = 'AverageScore';
      isDescending = true;
    }
    // Kreiranje searchObject objekta i inicijalizacija varijabli
    const searchObject: SearchObject = {
      movietitle: this.title,
      pricefrom: this.minValue,
      priceto: this.maxValue,
      categoryid: this.categoryID,
      sortedcolumn: sortedColumn,
      isdescending: isDescending,
      pagenumber: pageNumber,
    };
    // 'forkJoin' ceka da oba observablea završe svoje operacije
    // Kad se zavrsi 'forkJoin' operacija, izvrsava se blok unutar 'subscribe' metode
    forkJoin([
      this.movieGetAll.getMovies(searchObject),
      this.movieGetAll.getMoviesCount(searchObject)
    ]).subscribe(
      ([filteredMovies, moviesCount]: [Movie[], number]) => {
        this.moviesData = filteredMovies;
        this.totalMovies = moviesCount;
      },
      (error) => this.toastrService.error(error),
      () => this.load = false
    );
  }
  changeCategory(category: number): void {
    this.categoryID = category;
    this.setPaginatorPage();
    this.filterAndSortMovies();
  }
  changeOrderBy(order: string): void {
    this.movieParameterService.orderBy = order;
    this.orderBy = order;
    this.setPaginatorPage();
    this.filterAndSortMovies();
  }
  searchByTitle(title: string): void {
    this.movieParameterService.filterByTitle = title;
    this.title = title;
    this.setPaginatorPage();
    this.filterAndSortMovies();
  }
  get displayView(): string {
    return this.movieParameterService.displayView;
  }
  changeView(view: string): void {
    this.movieParameterService.displayView = view;
  }
}
