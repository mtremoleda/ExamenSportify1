namespace ApiSpotify.MODELS
{
    public class LlistaReproduccioCanco
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid IdCanco { get; set; }
        public Guid IdLlista { get; set; }
    }
}
