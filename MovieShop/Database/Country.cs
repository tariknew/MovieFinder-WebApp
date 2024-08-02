﻿using System.ComponentModel.DataAnnotations;

namespace MovieShop.Database
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        public string CountryName { get; set; }
    }
}
