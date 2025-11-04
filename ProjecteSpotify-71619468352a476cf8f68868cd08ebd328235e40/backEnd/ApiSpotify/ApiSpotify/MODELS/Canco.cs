namespace ApiSpotify.MODELS
{
    public class Canco
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Titol { get; set; }
        public string Artista { get; set; }
        public string Album { get; set; }
        public decimal? Durada { get; set; }

    }
}
