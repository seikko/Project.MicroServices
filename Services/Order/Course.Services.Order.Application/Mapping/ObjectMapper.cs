using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Services.Order.Application.Mapping
{
    public static class ObjectMapper
    {
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CustomeMapping>();
            });
            return config.CreateMapper();
        });

        public static IMapper Mapper => lazy.Value; // burayı cagırdıgım zaman lazy ozaman yuklenicek

    }
}
