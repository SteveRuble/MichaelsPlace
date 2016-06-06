using System.Web.Mvc;
using FluentBootstrap;
using FluentBootstrap.Mvc;
using JetBrains.Annotations;

namespace MichaelsPlace.Views
{
    /// <summary>
    /// Base class for views with a strongly typed model.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class MichaelsPlaceView<TModel> : WebViewPage<TModel>
    {
        /// <summary>
        /// Bootstrap HTML helper.
        /// </summary>
        [UsedImplicitly]
        public MvcBootstrapHelper<TModel> Bootstrap => Html.Bootstrap();
    }

    /// <summary>
    /// Base class for views with a dynamic typed model.
    /// </summary>
    public abstract class MichaelsPlaceView : WebViewPage
    {
        /// <summary>
        /// Bootstrap HTML helper.
        /// </summary>
        [UsedImplicitly]
        public MvcBootstrapHelper<object> Bootstrap => Html.Bootstrap();
    }
}
