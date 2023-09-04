using AutoMapper;

namespace TodoApiV7
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TodoItem, TodoItemDto>();
            CreateMap<TodoItemDto, TodoItem>();
        }
    }
}
