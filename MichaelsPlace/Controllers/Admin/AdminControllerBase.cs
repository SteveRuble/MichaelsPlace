using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using BootstrapMvcSample.Controllers;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Utilities;
using Ninject;

namespace MichaelsPlace.Controllers.Admin
{
    [Authorize(Roles = Constants.Roles.Administrator)]
    public abstract class AdminControllerBase : BootstrapBaseController
    {
        private Injected<IMapper> _mapper;
        private Injected<ApplicationDbContext> _dbContext;

        [Inject]
        public IMapper Mapper
        {
            get { return _mapper.Value; }
            set { _mapper.Value = value; }
        }

        [Inject]
        public ApplicationDbContext DbContext
        {
            get { return _dbContext.Value; }
            set { _dbContext.Value = value; }
        }

    }
}
