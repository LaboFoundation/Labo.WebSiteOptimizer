using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.SessionState;
using Labo.WebSiteOptimizer.ResourceManagement;

namespace Labo.WebSiteOptimizer.Web.UI.Controllers
{
    [SessionState(SessionStateBehavior.Disabled)]
    public class ResourceController : AsyncController
    {
        public void JsAsync(string name)
        {
            AsyncManager.OutstandingOperations.Increment();

            Task.Factory.StartNew(() => ProcessJs(name));
        }

        private void ProcessJs(string name)
        {
            ResourceManagerRuntime.ResourceHandler.HandleResource(ControllerContext.HttpContext, ResourceType.Js, name);
            AsyncManager.OutstandingOperations.Decrement();
        }

        public void JsCompleted()
        {
        }

        public void CssAsync(string name)
        {
            AsyncManager.OutstandingOperations.Increment();

            Task.Factory.StartNew(() => ProcessCss(name));
        }

        private void ProcessCss(string name)
        {
            ResourceManagerRuntime.ResourceHandler.HandleResource(ControllerContext.HttpContext, ResourceType.Css, name);
            AsyncManager.OutstandingOperations.Decrement();
        }

        public void CssCompleted()
        {
        }
    }
}
