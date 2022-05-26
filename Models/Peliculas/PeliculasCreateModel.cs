using DisneyChallengeV2.Entities;
using System;

namespace DisneyChallengeV2.Models.Peliculas
{
    public class PeliculasCreateModel
    {
        public string Imagen { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int Calificacion { get; set; }
        public int idGenero { get; set; }
    }
}
