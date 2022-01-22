using AutoMapper;
using Course.Services.Domain.OrderAggregate;
using Course.Services.Order.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Services.Order.Application.Mapping
{
   public  class CustomeMapping:Profile
    {
        public CustomeMapping()
        {
            CreateMap<Services.Domain.OrderAggregate.Order, OrderDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
