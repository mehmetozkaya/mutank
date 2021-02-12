using AutoMapper;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using Ordering.Domain.Entities;

namespace Ordering.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrdersVm>().ReverseMap();
            CreateMap<Order, CheckoutOrderCommand>().ReverseMap();

            //CreateMap<Event, EventListVm>().ReverseMap();
            //CreateMap<Event, CreateEventCommand>().ReverseMap();
            //CreateMap<Event, UpdateEventCommand>().ReverseMap();
            //CreateMap<Event, EventDetailVm>().ReverseMap();
            //CreateMap<Event, CategoryEventDto>().ReverseMap();
            //CreateMap<Event, EventExportDto>().ReverseMap();

            //CreateMap<Category, CategoryDto>();
            //CreateMap<Category, CategoryListVm>();
            //CreateMap<Category, CategoryEventListVm>();
            //CreateMap<Category, CreateCategoryCommand>();
            //CreateMap<Category, CreateCategoryDto>();

            //CreateMap<Order, OrdersForMonthDto>();
        }
    }
}
