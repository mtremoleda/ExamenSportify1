namespace UI.Spotify.MODELS
{
    public class QualitatFitxer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid IdCanco { get; set; }
        public Canco Canco { get; set; }
        public string Format { get; set; }
        public int? Bitrate { get; set; }
        public decimal? Mida { get; set; }
    }
}
