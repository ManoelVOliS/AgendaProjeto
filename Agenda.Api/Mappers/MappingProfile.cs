using AutoMapper;
using Agenda.Core.Dtos;
using Agenda.Core.Entities;

namespace Agenda.Api.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<CreateContactDto, Contact>();
            CreateMap<UpdateContactDto, Contact>();
            CreateMap<Contact, ContactResponseDto>();
        }

    }
}