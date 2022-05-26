using DisneyChallengeV2.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DisneyChallengeV2.Data
{
    public class DisneyDbContext : IdentityDbContext
    {
        public DisneyDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Personajes> Personajes { get; set; }
        public DbSet<Peliculas> Peliculas { get; set; } 
        public DbSet<Generos> Generos { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);



            builder.Entity<Peliculas>().ToTable("Peliculas");
            builder.Entity<Peliculas>().HasKey(x => x.Id);
            builder.Entity<Peliculas>().HasMany<Personajes>(x => x.Personajes).WithMany(x => x.Peliculas);
            builder.Entity<Peliculas>().HasOne<Generos>(x => x.Generos).WithMany(x => x.Peliculas);

            builder.Entity<Personajes>().HasKey(x => x.Id);
            builder.Entity<Personajes>().ToTable("Personajes");
            builder.Entity<Personajes>().HasMany<Peliculas>(x => x.Peliculas).WithMany(x => x.Personajes);

            builder.Entity<Generos>().HasKey(x => x.Id);
            builder.Entity<Generos>().ToTable("Generos");
            builder.Entity<Generos>().HasMany<Peliculas>(x => x.Peliculas).WithOne(x => x.Generos).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Peliculas>(x =>
            {
                x.Property(x => x.Id).IsRequired().UseIdentityColumn();
                x.Property(x => x.Titulo).IsRequired().HasMaxLength(50);
                x.Property(x => x.FechaCreacion).IsRequired();
                x.Property(x => x.Calificacion).IsRequired();
                x.Property(x => x.Imagen).HasMaxLength(200).HasColumnType("varchar");
            });

            builder.Entity<Personajes>(x =>
            {
                x.Property(x => x.Id).IsRequired().UseIdentityColumn().HasColumnType("int");
                x.Property(x => x.Nombre).IsRequired().HasMaxLength(50).HasColumnType("varchar");
                x.Property(x => x.Imagen).HasMaxLength(200).HasColumnType("varchar");
                x.Property(x => x.Edad).IsRequired().HasColumnType("int");
                x.Property(x => x.Peso).HasColumnType("float");
                x.Property(x => x.Historia).HasMaxLength(250).HasColumnType("varchar");
            });

            builder.Entity<Generos>(x =>
            {
                x.Property(x => x.Id).IsRequired().UseIdentityColumn().HasColumnType("int");
                x.Property(x => x.Nombre).IsRequired().IsUnicode().HasMaxLength(30).HasColumnType("varchar");
                x.Property(x => x.Imagen).HasMaxLength(200).HasColumnType("varchar");
            });

            //builder.Entity<Generos>()
            //    .HasData(
            //    new Generos
            //    {
            //        Id = 1,
            //        Nombre = "Aventura",
            //        Imagen = "https://i0.wp.com/imagenesparapeques.com/wp-content/uploads/2016/11/mickey-and-the-roadster-racers-png-mickey-aventura-sobre-ruedas-imagenes-mickey-sobre-ruedas.png?w=454&ssl=1"

            //    },
            //    new Generos
            //    {
            //        Id = 2,
            //        Nombre = "Familiar",
            //        Imagen = "https://i0.wp.com/imagenesparapeques.com/wp-content/uploads/2016/11/mickey-and-the-roadster-racers-png-mickey-aventura-sobre-ruedas-imagenes-mickey-sobre-ruedas.png?w=454&ssl=1"
            //    });

        }
    }
}
