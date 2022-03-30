using AutoMapper;
using DataStore.EF;

namespace RestApiTutorial.DTOs;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<CreateTicketDto, Ticket>();
        CreateMap<Ticket, TicketDto>();
        
        CreateMap<Project, ProjectDto>();
        CreateMap<CreateProjectDto, Project>();
    }

}
