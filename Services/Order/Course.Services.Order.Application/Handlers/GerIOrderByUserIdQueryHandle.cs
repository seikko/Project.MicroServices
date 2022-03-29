using Course.Services.Order.Application.Dtos;
using Course.Services.Order.Application.Mapping;
using Course.Services.Order.Application.Queries;
using Course.Services.Order.Infrastructure;
using Course.Shared.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Course.Services.Order.Application.Handlers
{
    public class GerIOrderByUserIdQueryHandle : IRequestHandler<GetOrderByUserIdQuery, Response<List<OrderDto>>>
    {
        private readonly OrderDbContext _context;

        public GerIOrderByUserIdQueryHandle(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Response<List<OrderDto>>> Handle(GetOrderByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders.Include(x => x.OrderItems).Where(x => x.BuyerId == request.UserId).ToListAsync();
            if (!orders.Any()) return Response<List<OrderDto>>.Success(new List<OrderDto>(), 200);

            var ordersDto = ObjectMapper.Mapper.Map<List<OrderDto>>(orders);
             
            return Response<List<OrderDto>>.Success(ordersDto,200); 

        }
    }
}
