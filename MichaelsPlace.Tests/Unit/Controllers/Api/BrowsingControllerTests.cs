using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MichaelsPlace.Controllers.Api;
using MichaelsPlace.Models.Api;
using MichaelsPlace.Models.Persistence;
using MichaelsPlace.Queries;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Unit.Controllers.Api
{
    [TestFixture]
    public class BrowsingControllerTests : ApiControllerUnitTestBase<BrowsingController>
    {
        [Test]
        public void article_by_id()
        {
            Kernel.GetMock<ByIdQuery<Article>>().Setup(m => m.Execute<BrowsingItemModel>(1)).Returns(new BrowsingItemModel()).Verifiable();
            Target.ArticleById(1);
            Kernel.GetMock<ByIdQuery<Article>>().Verify();
        }
    }
}
