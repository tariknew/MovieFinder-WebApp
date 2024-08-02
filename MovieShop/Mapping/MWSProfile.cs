using AutoMapper;
using MovieShop.Database;
using MovieShop.Models.Requests;
using MovieShop.ViewModels;

namespace MovieShop.Mapping
{
    public class MWSProfile : Profile
    {
        public MWSProfile()
        {
            CreateMap<Movie, VMMovie>();
            CreateMap<Category, VMCategory>();
            CreateMap<Review, VMReview>();
            CreateMap<MovieCategory, VMMovieCategory>();
            CreateMap<MovieActor, VMMovieActor>();
            CreateMap<MovieInsertRequest, Movie>();
            CreateMap<MovieUpdateRequest, Movie>();
            CreateMap<CategoryUpdateRequest, Category>();
            CreateMap<CategoryInsertRequest, Category>();
            CreateMap<ReviewInsertRequest, Review>();
            CreateMap<Actor, VMActor>();
            CreateMap<ActorInsertRequest, Actor>();
            CreateMap<ActorUpdateRequest, Actor>();
            CreateMap<User, VMUser>();
            CreateMap<UserProfilePictureChange, User>();
            CreateMap<Notification, VMNotification>();
            CreateMap<NotificationInsertRequest, Notification>();
            CreateMap<UserNotification, VMUserNotification>();
            CreateMap<UserNotificationInsertRequest, UserNotification>();
            CreateMap<UserNotificationUpdateRequest, UserNotification>();
            CreateMap<AddToFavoritesRequest, FavouriteMovie>();
            CreateMap<FavouriteMovie, VMMovieFavourites>();

        }
    }
}
