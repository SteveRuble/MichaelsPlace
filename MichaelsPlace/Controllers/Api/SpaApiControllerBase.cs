using System.Web.Http;
using AutoMapper;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Utilities;
using Ninject;

namespace MichaelsPlace.Controllers.Api
{
    public abstract class SpaApiControllerBase : ApiController
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