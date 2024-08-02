﻿using MovieShop.Database;

namespace MovieShop.ViewModels
{
    public class VMMovieFavourites
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public DateTime DateAdded { get; set; }
        public User User { get; set; }
        public Movie Movie { get; set; }
    }
}
