using AutoMapper;
using CW2.DAL.Entities;
using CW2.Models;

namespace CW2.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile() { 
        CreateMap<Artwork, ArtworkViewModel>().ReverseMap();
        }
    }
}
