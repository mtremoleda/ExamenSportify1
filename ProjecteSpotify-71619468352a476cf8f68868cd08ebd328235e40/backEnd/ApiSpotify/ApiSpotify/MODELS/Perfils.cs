namespace ApiSpotify.MODELS
{
    public class Perfil
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nom { get; set; }
        public string Descripcio { get; set; }
        public string Estat { get; set; }
        

    }
}
