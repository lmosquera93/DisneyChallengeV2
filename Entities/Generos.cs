using System.Collections.Generic;

namespace DisneyChallengeV2.Entities
{
    public class Generos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public ICollection<Peliculas> Peliculas { get; set; } = new List<Peliculas>();
    }
}
