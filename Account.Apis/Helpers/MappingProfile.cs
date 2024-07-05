using Account.Core.Dtos;
using Account.Core.Models.Entites;
using AutoMapper;
using System.Globalization;

namespace Account.Apis.Helpers
{
    // AutoMapper configuration
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Item, ItemDto>();
            CreateMap<ItemDto, Item>();

            CreateMap<Persone, PersoneDto>();
            CreateMap<PersoneDto, Persone>();
           
            //CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Complains, ComplainsDto>().ReverseMap();



            CreateMap<Comment, commentpersonDto>().ReverseMap();
            CreateMap<Comment, commentItemDto>().ReverseMap();






        }

    }
}
