using Course.Services.Order.Application.Dtos;
using Course.Shared.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Services.Order.Application.Commands
{
    public class CreateOrderCommand : IRequest<Response<CreatedOrderDto>>
    {
        public string BuyyerId { get; set; }
        public List<OrderItemDto> OrderItem { get; set; }
        public AddressDto Address { get; set; }
    }
}
