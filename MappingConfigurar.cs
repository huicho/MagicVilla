using AutoMapper;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;

namespace MagicVilla_API
{
    public class MappingConfigurar : Profile
    {

        public MappingConfigurar()
        {
            //en dos lineas
            CreateMap<Villa, VillaDto>();
            CreateMap<VillaDto, Villa>();   //viceversa

            //en una sola linea
            CreateMap<Villa, VillaCreateDto>().ReverseMap();
            CreateMap<Villa, VillaUpdateDto>().ReverseMap();

        }


    }


}
