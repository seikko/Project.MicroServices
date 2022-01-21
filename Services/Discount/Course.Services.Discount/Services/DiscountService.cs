using Course.Shared.Dtos;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _connection;

        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<Response<NoContent>> Delete(int id)
        {
            var status = await _connection.ExecuteAsync("Delete from discount where id = @id", new { Id = id });
            return status > 0 ? Response<NoContent>.Success(204) : Response<NoContent>.Fail("response not found", 404);
        }

        public async Task<Response<List<Models.Discount>>> GetAll()
        {
            var discount = await _connection.QueryAsync<Models.Discount>("Select * from discount");
            return Response<List<Models.Discount>>.Success(discount.ToList(), 200);

        }

        public async Task<Response<Models.Discount>> GetByCodeAndUserId(string code, string userId)
        {
            var discounts = await _connection.QueryAsync<Models.Discount>("Select * from discount where userid=@userId and code=@code", new { UserId = userId, Code = code });
            var hasDiscounts = discounts.FirstOrDefault();
            return hasDiscounts != null ? Response<Models.Discount>.Success(hasDiscounts, 200) : Response<Models.Discount>.Fail("Discount not found", 404);
        }

        public async Task<Response<Models.Discount>> GetById(int id)
        {
            var discount = (await _connection.QueryAsync<Models.Discount>("Select * from discount where id = @Id", new { Id = id })).SingleOrDefault();
            return discount != null ? Response<Models.Discount>.Success(discount, 200) : Response<Models.Discount>.Fail("Discount not found", 404);


        }

        public async Task<Response<NoContent>> Save(Models.Discount discount)
        {
          
                var status = await _connection.ExecuteAsync("INSERT  INTO discount(userid,rate,code)VALUES(@UserId,@Rate,@Code)", discount);
                //if (status > 0) return Response<NoContent>.Success(204);
                //return Response<NoContent>.Fail("an error occurred while adding", 500);
                return status > 0 ? Response<NoContent>.Success(204) : Response<NoContent>.Fail("an error occurred while adding", 500);
            
          


        }

        public async Task<Response<NoContent>> Update(Models.Discount discount)
        {
            var status = await _connection.ExecuteAsync("update discount set userid=@Userid,code=@Code,rate=@Rate where id=@id", new Models.Discount
            {
                Id = discount.Id,
                UserId = discount.UserId,
                Code = discount.Code,
                Rate = discount.Rate,
              
            });
            return status > 0 ? Response<NoContent>.Success(204) : Response<NoContent>.Fail("discount not found", 404);

            //if (status > 0) return Response<NoContent>.Success(204);
            //return Response<NoContent>.Fail("discount not found", 404);
        }
    }
}
