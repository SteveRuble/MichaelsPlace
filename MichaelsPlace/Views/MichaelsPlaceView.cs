using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentBootstrap;
using FluentBootstrap.Mvc;

namespace MichaelsPlace.Views
{
    public abstract class MichaelsPlaceView<TModel> : WebViewPage<TModel>
    {
        public MvcBootstrapHelper<TModel> Bootstrap => Html.Bootstrap();
    }

    public abstract class MichaelsPlaceView : WebViewPage
    {
        public MvcBootstrapHelper<object> Bootstrap => Html.Bootstrap();
    }
}
