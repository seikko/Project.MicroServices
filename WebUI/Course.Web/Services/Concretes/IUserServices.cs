using Course.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Controllers
{
    public interface IUserServices
    {
        Task<UserViewModel> GetUser();
    }
}
