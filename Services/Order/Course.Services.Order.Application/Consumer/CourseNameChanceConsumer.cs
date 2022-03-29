using Course.Services.Order.Infrastructure;
using Course.Shared.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Services.Order.Application.Consumer
{
    public class CourseNameChanceConsumer : IConsumer<CourseNameChanceEvent>
    {
        private readonly OrderDbContext _dbContext;

        public CourseNameChanceConsumer(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<CourseNameChanceEvent> context)
        {
            var orderItems = await _dbContext.OrderItems.Where(x => x.ProductId == context.Message.CourseId).ToListAsync();
            orderItems.ForEach(x =>
            {
                x.UpdateOrderItem(context.Message.UpdatedName, x.PictureUrl, x.Price);
            });
            await _dbContext.SaveChangesAsync();
        }
    }
}
