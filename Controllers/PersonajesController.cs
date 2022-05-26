using DisneyChallengeV2.Data;
using DisneyChallengeV2.Entities;
using DisneyChallengeV2.Models.Personajes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DisneyChallengeV2.Controllers
{
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("characters/")]
    [ApiController]
    public class PersonajesController : ControllerBase
    {
        private readonly DisneyDbContext _context;

        public PersonajesController(DisneyDbContext ctx)
        {
            _context = ctx;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            List<Personajes> list =  _context.Personajes.ToList();

            return Ok(list.Select(x => new { Nombre = x.Nombre, Imagen = x.Imagen }).ToList());
        }

        [HttpGet]
        [Route("detalle/{id}")]
        public IActionResult GetDetalleById(int id)
        {
            IQueryable<Personajes> findcharacter = _context.Personajes.Include(x => x.Peliculas).Where(y => y.Id == id);

            if (findcharacter == null)
             {

                return BadRequest("Personaje no encontrado.");

             }

           // return Ok(findcharacter);
            return Ok(findcharacter.Select(x => new { Id = x.Id ,Nombre = x.Nombre, Imagen = x.Imagen, Edad = x.Edad  }).ToList());
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Save(PersonajeCreateModel personajes)
        {
            var nuevopersonaje = new Personajes()
            {
                Nombre = personajes.Nombre,
                Imagen = personajes.Imagen,
                Edad = personajes.Edad,
                Peso = personajes.Peso,
                Historia = personajes.Historia

            };

            _context.Personajes.Add(nuevopersonaje);

            _context.SaveChanges();

            return Ok("Personaje Creado");
        }


        [HttpGet]
        [Route("/name={Nombre}")]
        public IActionResult GetPersonajeByName(string Nombre)
        {
            IQueryable<Personajes> list = _context.Personajes.Include(x => x.Peliculas);

            if (Nombre != null) list = list.Where(x => x.Nombre == Nombre);

            return Ok(list.Select(x => new { Nombre = x.Nombre, Imagen = x.Imagen }).ToList());
        }

        [HttpGet]
        [Route("/age={Edad}")]
        public IActionResult GetPersonajeByAge(int Edad)
        {
            IQueryable<Personajes> list = _context.Personajes.Include(x => x.Peliculas);

            list = list.Where(x => x.Edad == Edad);

            return Ok(list.Select(x => new { Nombre = x.Nombre, Imagen = x.Imagen }).ToList());
        }

        [HttpGet]
        [Route("/movies={idMovies}")]
        public IActionResult GerPersonajesByMovie(int idMovies)
        {
            IQueryable<Personajes> list = _context.Personajes.Include(x => x.Peliculas);

            return Ok(list.Select(x => new { Nombre = x.Nombre, Imagen = x.Imagen }).ToList());
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var personaje = _context.Personajes.FirstOrDefault(x => x.Id == id);

            if (personaje == null)
            {
                return NotFound("El personaje que desea eliminar no existe.");
            }
            else
            {
                _context.Personajes.Remove(personaje);

                _context.SaveChanges();

                return Ok("El personaje fue eliminado.");
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public IActionResult Update(PersonajeUpdateModel personaje, int id)
        {
            var findcharacter = _context.Personajes.FirstOrDefault(x => x.Id == id);

            if (findcharacter == null)
            {
                return NotFound("No existe el personaje.");
            }
            else
            {
                findcharacter.Nombre = personaje.Nombre;
                findcharacter.Imagen = personaje.Imagen;
                findcharacter.Edad = personaje.Edad;
                findcharacter.Peso = personaje.Peso;
                findcharacter.Historia = personaje.Historia;

                _context.Personajes.Update(findcharacter);

                _context.SaveChanges();

                return Ok("Personaje editado con exito.");
            }
        }

        [HttpPost]
        [Route("addMovie")]
        public IActionResult Post(int idPersonaje, int idPelicula)
        {
            var personaje = _context.Personajes.FirstOrDefault(x => x.Id == idPersonaje);

            var pelicula = _context.Peliculas.FirstOrDefault(x => x.Id == idPelicula);

            if (personaje == null || pelicula == null)
            {
                return NotFound("El Personaje o Pelicula ingregasada no existen");
            }
            else
            {
                personaje.Peliculas.Add(pelicula);

                _context.Personajes.Update(personaje);

                _context.SaveChanges();

                return Ok("Personaje agregado a la pelicula");
            }




        }
    }
}
