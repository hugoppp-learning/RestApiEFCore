using AutoMapper;
using DataStore.EF;
using DataStore.EF.Models;

namespace RestApiTutorial.DTOs;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<CreateTicketDto, Ticket>();
        CreateMap<TicketDto, Ticket>().ReverseMap();

        CreateMap<CreateProjectDto, Project>();
        CreateMap<ProjectDto, Project>().ReverseMap();
    }
}
