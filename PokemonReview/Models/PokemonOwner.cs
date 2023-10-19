namespace PokemonReview.Models
{
    public class PokemonOwner
    {
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }
        public int PokemonId { get; set; }
        public Pokemon Pokemon { get; set; }
    }
}
