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

        public void DebugJsAsync(string name, string fileName)
        {
            AsyncManager.OutstandingOperations.Increment();

            Task.Factory.StartNew(() => ProcessDebugJs(name, fileName));
        }

        private void ProcessDebugJs(string name, string fileName)
        {
            ProcessDebug(ResourceType.Js, name, fileName);
            AsyncManager.OutstandingOperations.Decrement();
        }

        public void DebugCssCompleted()
        {
        }

        public void DebugCssAsync(string name, string fileName)
        {
            AsyncManager.OutstandingOperations.Increment();

            Task.Factory.StartNew(() => ProcessDebugCss(name, fileName));
        }

        private void ProcessDebugCss(string name, string fileName)
        {
            ProcessDebug(ResourceType.Css, name, fileName);
            AsyncManager.OutstandingOperations.Decrement();
        }

        public void DebugJsCompleted()
        {
        }

        private void ProcessDebug(ResourceType resourceType, string name, string fileName)
        {
            ResourceManagerRuntime.ResourceHandler.HandleResource(ControllerContext.HttpContext, resourceType, name, fileName, false, false);
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
