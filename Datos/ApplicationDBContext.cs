using MagicVilla_API.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Datos
{
    public class ApplicationDBContext :DbContext
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) :base(options)
        {
            
        }
        public  DbSet<Villa> Villas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Nombre = "Villa Real",
                    Detalle = "Detalle de la villa",
                    ImagenUrl = "",
                    Ocupantes = 5,
                    MetrosCuadrados = 50,
                    Tarifa = 200,
                    Amenidad = "",
                    FechaActualizacion = DateTime.Now,
                    FechaCreacion = DateTime.Now
                },
                new Villa()
                {
                    Id = 2,
                    Nombre = "Villa Premiun",
                    Detalle = "Detalle de la villa premium",
                    ImagenUrl = "",
                    Ocupantes = 4,
                    MetrosCuadrados = 150,
                    Tarifa = 1200,
                    Amenidad = "",
                    FechaActualizacion = DateTime.Now,
                    FechaCreacion = DateTime.Now
                }
                );
        }

    }

}
