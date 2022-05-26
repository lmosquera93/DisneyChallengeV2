using System;
using System.Collections.Generic;

namespace DisneyChallengeV2.Entities
{
    public class Peliculas
    {
        public int Id { get; set; }
        public string Imagen { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int Calificacion { get; set; }
        public Generos Generos { get; set; }
        public ICollection<Personajes> Personajes { get; set; } = new List<Personajes>();
    }
}
