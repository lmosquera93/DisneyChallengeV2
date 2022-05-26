using System.Collections.Generic;

namespace DisneyChallengeV2.Entities
{
    public class Personajes
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public int Edad { get; set; }
        public float Peso { get; set; }
        public string Historia { get; set; }
        public ICollection<Peliculas> Peliculas { get; set; } = new List<Peliculas>();
    }
}
