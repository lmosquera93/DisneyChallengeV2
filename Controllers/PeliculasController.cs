using DisneyChallengeV2.Data;
using DisneyChallengeV2.Entities;
using DisneyChallengeV2.Models.Peliculas;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DisneyChallengeV2.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("movies/")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly DisneyDbContext _context;

        public PeliculasController(DisneyDbContext ctx)
        {
            _context = ctx;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var peliculas = _context.Peliculas.Include(x => x.Personajes).ToList();

            var peliculasDTO = new List<PeliculasListModel>();

            foreach (var p in peliculas)
            {
                peliculasDTO.Add(
                    new PeliculasListModel
                    {
                        Imagen = p.Imagen,
                        Titulo = p.Titulo,
                        FechaCreacion = p.FechaCreacion

                    });
            }

            return Ok(peliculasDTO);
        }

        [HttpGet]
        [Route("detalle/{id}")]
        public IActionResult GetDetalleById(int id)
        {

            var findmovie = _context.Peliculas.FirstOrDefault(x => x.Id == id);

            if (findmovie == null)
            {

                return BadRequest("Pelicula no encontrada.");

            }

            return Ok(findmovie);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Save(PeliculasCreateModel pelicula)
        {

            var nuevaPelicula = new Peliculas
            {
                Imagen = pelicula.Imagen,
                Titulo = pelicula.Titulo,
                FechaCreacion = pelicula.FechaCreacion,
                Calificacion = pelicula.Calificacion
            };

            if(pelicula.Calificacion > 5 || pelicula.Calificacion < 0)
            {
                return BadRequest("la calificacion debe ser del 1 al 5");
            }
            else
            {
                if (pelicula.idGenero != 0)
                {
                    var genero = _context.Generos.FirstOrDefault(x => x.Id == pelicula.idGenero);

                    if (genero != null)
                    {
                        if (nuevaPelicula.Generos == null) nuevaPelicula.Generos = new Generos();

                        nuevaPelicula.Generos = genero;
                    }
                }

                _context.Add(nuevaPelicula);

                _context.SaveChanges();

                return Ok(new PeliculasCreateModel
                {
                    Imagen = pelicula.Imagen,
                    Titulo = pelicula.Titulo,
                    FechaCreacion = pelicula.FechaCreacion,
                    Calificacion = pelicula.Calificacion,
                    idGenero = pelicula.idGenero

                });
            }



            //var genero = _context.Generos.FirstOrDefault(x => x.Id == pelicula.idGenero);

            //if (genero == null)
            //{

            //    return BadRequest("No se encontro el genero");
            //    //nuevaPelicula.Generos = genero;
            //}
            //else
            //{
            //     _context.Peliculas.Add(
            //        new Peliculas
            //        {
            //            Imagen = nuevaPelicula.Imagen,
            //            Titulo = nuevaPelicula.Titulo,
            //            FechaCreacion = nuevaPelicula.FechaCreacion,
            //            Calificacion = nuevaPelicula.Calificacion,
            //            Generos =
            //            {
            //                Id = genero.Id,
            //                Nombre = genero.Nombre,
            //                Imagen = genero.Imagen

            //            }

            //        });

            // _context.Peliculas.Add(pelis);


        }


        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var peliculas = _context.Peliculas.FirstOrDefault(x => x.Id == id);

            if (peliculas == null)
            {
                return NotFound("La pelicula que desea eliminar no existe.");
            }
            else
            {
                _context.Peliculas.Remove(peliculas);

                _context.SaveChanges();

                return Ok("La pelicula fue eliminada.");
            }
        }

        [HttpPut]
        [Route("update")]
        public IActionResult Update(PeliculasUpdateModel pelicula, int id)
        {
            var findmovie = _context.Peliculas.FirstOrDefault(x => x.Id == id);

            if (findmovie == null)
            {
                return NotFound("No existe la pelicula.");
            }
            else
            {
                findmovie.Imagen = pelicula.Imagen;
                findmovie.Titulo = pelicula.Imagen;
                findmovie.FechaCreacion = pelicula.FechaCreacion;
                findmovie.Calificacion = pelicula.Calificacion;

                if (pelicula.Calificacion > 5 || pelicula.Calificacion < 0)
                {
                    return BadRequest("La calificacion debe ser de 1 al 5");
                }
                _context.Peliculas.Update(findmovie);

                _context.SaveChanges();

                return Ok("Pelicula editado con exito.");
            }
        }

        [HttpGet]
        [Route("name={Nombre}")]
        public IActionResult GetPeliculasByName(string Nombre)
        {
            IQueryable<Peliculas> list = _context.Peliculas.Include(x => x.Personajes);

            if (Nombre != null) list = list.Where(x => x.Titulo == Nombre);

            return Ok(list.Select(x => new
            {
                Imagen = x.Imagen,
                Titulo = x.Titulo,
                FechaCreacion = x.FechaCreacion
            }).ToList());
        }

        [HttpGet]
        [Route("genre={idGenero}")]
        public IActionResult GetPeliculasByGenero(int idGenero)
        {
            IQueryable<Peliculas> list = _context.Peliculas.Include(x => x.Personajes);

            if (idGenero != 0) list = list.Where(x => x.Generos.Id == idGenero);

            return Ok(list.Select(x => new
            {
                Imagen = x.Imagen,
                Titulo = x.Titulo,
                FechaCreacion = x.FechaCreacion
            }).ToList());
        }


        [HttpGet]
        [Route("order={order}")]
        public IActionResult GetPeliculasOrdenadas(string order)
        {
            IQueryable<Peliculas> list = _context.Peliculas.Include(x => x.Personajes);

            if (order == "ASC" || order == "DESC")
            {
                if (order == "ASC") list = list.OrderBy(x => x.FechaCreacion);

                if (order == "DESC") list = list.OrderByDescending(x => x.FechaCreacion);

                return Ok(list.Select(x => new
                {
                    Imagen = x.Imagen,
                    Titulo = x.Titulo,
                    FechaCreacion = x.FechaCreacion

                }).ToList());
            }
            else
            {
                return BadRequest("Colocar ASC o DESC");
            }


        }


    }
}
