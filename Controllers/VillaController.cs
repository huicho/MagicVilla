﻿using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
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
        private readonly IMapper _mapper;
        
        public VillaController(ILogger<VillaController> logger, ApplicationDBContext db, IMapper mapper)
        {
            _logger = logger;    
            _db= db;
            _mapper = mapper;
        }


        // GET: api/<VillaController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            _logger.LogInformation("Obtener las villas");
            IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();

            return Ok(_mapper.Map<IEnumerable<VillaDto>>(villaList));
        }



        [HttpGet("id:int", Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task< ActionResult<VillaDto>> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Error al traer la villa " + id);
                return BadRequest();
            }

            //var villa=VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<VillaDto>(villa));
        }


        [HttpPost]
        [ProducesResponseType (StatusCodes.Status201Created) ]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
       public async Task< ActionResult <VillaCreateDto>> CrearVilla([FromBody] VillaCreateDto createDto) 
        { 
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(await _db.Villas.FirstOrDefaultAsync(v => v.Nombre.ToLower()== createDto.Nombre.ToLower() ) != null) 
            {
                ModelState.AddModelError("NombreExiste", "La villa con ese nombre ya existe");
                return BadRequest(ModelState);
            
            }


            if(createDto == null)
            {
                return BadRequest(createDto);
            }

            /* esto se remplaza con una sola linea con el _mapper
              
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

             con la siguiente */

            Villa modelo=_mapper.Map<Villa>(createDto);


           await _db.Villas.AddAsync(modelo);
           await _db.SaveChangesAsync();

            return CreatedAtRoute("GetVilla", new {id=modelo.Id}, modelo);
        
        
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task< IActionResult> DeleteVilla(int id)
        {

            if(id==0)
            {
                return BadRequest();
            }

            var villa=await _db.Villas.FirstOrDefaultAsync(v=>v.Id==id);
            if(villa==null)
            {
                return NotFound();
            }

            _db.Villas.Remove(villa);
          await  _db.SaveChangesAsync(true);

            return NoContent();

        }

        // PUT api/<VillaController>/5
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task< IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            if(updateDto == null || id != updateDto.Id)
            {
                return BadRequest();
            }

            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            //villa.Nombre= villaDto.Nombre;
            //villa.Ocupantes= villaDto.Ocupantes;
            //villa.MetrosCuadrados=villaDto.MetrosCuadrados;

            //lo mismo que en create, se remplaza por la siguiente linea
            ////Villa modelo = new()
            ////{
            ////    Id= villaDto.Id,
            ////    Nombre = villaDto.Nombre,
            ////    Detalle = villaDto.Detalle,
            ////    ImagenUrl = villaDto.ImagenUrl,
            ////    Ocupantes = villaDto.Ocupantes,
            ////    Tarifa = villaDto.Tarifa,
            ////    MetrosCuadrados = villaDto.MetrosCuadrados,
            ////    Amenidad = villaDto.Amenidad
            ////};

            Villa modelo= _mapper.Map<Villa>(updateDto);
            
            _db.Villas.Update(modelo);
           await _db.SaveChangesAsync();


            return NoContent();

        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task< IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
            VillaUpdateDto villaDto=_mapper.Map<VillaUpdateDto>(villa);            
            
            ////VillaUpdateDto villaDto = new()
            ////{
            ////    Id = villa.Id,
            ////    Nombre = villa.Nombre,
            ////    Detalle = villa.Detalle,
            ////    ImagenUrl = villa.ImagenUrl,
            ////    Ocupantes = villa.Ocupantes,
            ////    Tarifa = villa.Tarifa,
            ////    MetrosCuadrados = villa.MetrosCuadrados,
            ////    Amenidad = villa.Amenidad
            ////};

            if(villa==null) return BadRequest();

            patchDto.ApplyTo(villaDto, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            Villa modelo = _mapper.Map<Villa>(villaDto);

            //Villa modelo = new()
            //{
            //    Id = villaDto.Id,
            //    Nombre = villaDto.Nombre,
            //    Detalle = villaDto.Detalle,
            //    ImagenUrl = villaDto.ImagenUrl,
            //    Ocupantes = villaDto.Ocupantes,
            //    Tarifa = villaDto.Tarifa,
            //    MetrosCuadrados = villaDto.MetrosCuadrados,
            //    Amenidad = villaDto.Amenidad
            //};

            _db.Villas.Update(modelo);
            await _db.SaveChangesAsync();

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
        //public void Delete(int id)     //{
        //}
    }
}
