using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using BootstrapMvcSample.Controllers;
using MichaelsPlace.Models.Persistence;
using Ninject;

namespace MichaelsPlace.Controllers.Admin
{
    [Authorize(Roles = Constants.Roles.Administrator)]
    public abstract class AdminControllerBase : BootstrapBaseController
    {
        [Inject]
        public IMapper Mapper { get; set; }
        [Inject]
        public ApplicationDbContext DbContext { get; set; }
    }
    
}
