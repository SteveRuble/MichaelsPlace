using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Internal;
using System.Data.Entity.Migrations;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MichaelsPlace.Models.Persistence;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Owin;

namespace MichaelsPlace
{
    public partial class Startup
    {
        private void ConfigureData(IAppBuilder app)
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }
    }
}
