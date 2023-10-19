using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonReview.Models
{
    public class Review
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        [Required]
        public int Rating { get; set; }

        [ForeignKey(nameof(Pokemon))]
        public int PokemonId { get; set; }
        public Pokemon Pokemon { get; set; }

        [ForeignKey(nameof(Reviewer))]
        public int ReviewerId { get; set; }
        public Reviewer Reviewer { get; set; }
    }
}
