using System.ComponentModel.DataAnnotations;

namespace PokemonReview.Dto
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required]
        public int PokemonId { get; set; }
        [Required]
        public int ReviewerId { get; set; }

    }
}
