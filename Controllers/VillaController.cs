using MagicVilla_API.Datos;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {

        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDBContext _db;
        
        public VillaController(ILogger<VillaController> logger, ApplicationDBContext db)
        {
            _logger = logger;    
            _db= db;
        }


        // GET: api/<VillaController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.LogInformation("Obtener las villas");
            return Ok(_db.Villas.ToList());
        }



        [HttpGet("id:int", Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Error al traer la villa " + id);
                return BadRequest();
            }

            //var villa=VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(villa);
        }


        [HttpPost]
        [ProducesResponseType (StatusCodes.Status201Created) ]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
       public ActionResult <VillaDto> CrearVilla([FromBody] VillaDto villaDto) 
        { 
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(_db.Villas.FirstOrDefault(v => v.Nombre.ToLower()==villaDto.Nombre.ToLower() ) != null) 
            {
                ModelState.AddModelError("NombreExiste", "La villa con ese nombre ya existe");
                return BadRequest(ModelState);
            
            }


            if(villaDto == null)
            {
                return BadRequest(villaDto);
            }

            if(villaDto.Id>0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }


            Villa modelo = new()
            {
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImagenUrl = villaDto.ImagenUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = villaDto.Tarifa,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                Amenidad = villaDto.Amenidad
            };

            _db.Villas.Add(modelo);
            _db.SaveChanges();

            return CreatedAtRoute("GetVilla", new {id=villaDto.Id}, villaDto);
        
        
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult DeleteVilla(int id)
        {

            if(id==0)
            {
                return BadRequest();
            }

            var villa=_db.Villas.FirstOrDefault(v=>v.Id==id);
            if(villa==null)
            {
                return NotFound();
            }

            _db.Villas.Remove(villa);
            _db.SaveChanges(true);

            return NoContent();

        }

        // PUT api/<VillaController>/5
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villaDto)
        {
            if(villaDto==null || id != villaDto.Id)
            {
                return BadRequest();
            }

            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            //villa.Nombre= villaDto.Nombre;
            //villa.Ocupantes= villaDto.Ocupantes;
            //villa.MetrosCuadrados=villaDto.MetrosCuadrados;

            Villa modelo = new()
            {
                Id= villaDto.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImagenUrl = villaDto.ImagenUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = villaDto.Tarifa,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                Amenidad = villaDto.Amenidad
            };

            _db.Villas.Update(modelo);
            _db.SaveChanges();


            return NoContent();

        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            var villa = _db.Villas.AsNoTracking().FirstOrDefault(v => v.Id == id);
            
            VillaDto villaDto = new()
            {
                Id = villa.Id,
                Nombre = villa.Nombre,
                Detalle = villa.Detalle,
                ImagenUrl = villa.ImagenUrl,
                Ocupantes = villa.Ocupantes,
                Tarifa = villa.Tarifa,
                MetrosCuadrados = villa.MetrosCuadrados,
                Amenidad = villa.Amenidad
            };

            if(villa==null) return BadRequest();

            patchDto.ApplyTo(villaDto, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa modelo = new()
            {
                Id = villaDto.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImagenUrl = villaDto.ImagenUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = villaDto.Tarifa,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                Amenidad = villaDto.Amenidad
            };

            _db.Villas.Update(modelo);
            _db.SaveChanges();

            return NoContent();

        }



        //// GET api/<VillaController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<VillaController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}


        //// DELETE api/<VillaController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
