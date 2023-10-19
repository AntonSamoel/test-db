﻿using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonReview.Models
{
    public class Owner
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gym { get; set; }
        public ICollection<PokemonOwner> PokemonOwners { get; set; }

        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
