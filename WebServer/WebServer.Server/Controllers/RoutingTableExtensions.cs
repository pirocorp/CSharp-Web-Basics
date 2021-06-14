namespace WebServer.Server.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Http;
    using Routing;

    public static class RoutingTableExtensions
    {
        private static Type httpResponseType = typeof(HttpResponse);

        public static IRoutingTable MapGet<TController>(
            this IRoutingTable routingTable,
            string path,
            Func<TController, HttpResponse> controllerFunction)
            where TController : Controller
                => routingTable
                    .MapGet(path, request => controllerFunction(CreateController<TController>(request)));

        public static IRoutingTable MapPost<TController>(
            this IRoutingTable routingTable, 
            string path,
            Func<TController, HttpResponse> controllerFunction)
            where TController : Controller
                => routingTable
                    .MapPost(path, request => controllerFunction(CreateController<TController>(request)));

        public static IRoutingTable MapControllers(this IRoutingTable routingTable)
        {
            var controllerActions = GetControllerActions();

            foreach (var controllerAction in controllerActions)
            {
                var controllerType = controllerAction.DeclaringType;
                var controllerName = controllerType.GetControllerName();
                var actionName = controllerAction.Name;

                var path = $"/{controllerName}/{actionName}";

                var responseFunction = GetResponseFunction(controllerType, controllerAction, path);

                routingTable.MapGet(path, responseFunction);

                MapDefaultRoutes(routingTable, controllerName, actionName, responseFunction);
            }

            return routingTable;
        }

        private static IEnumerable<MethodInfo> GetControllerActions()
            => Assembly.GetEntryAssembly()
                .GetExportedTypes()
                .Where(t => !t.IsAbstract 
                    && t.IsAssignableTo(typeof(Controller)) 
                    && t.Name.EndsWith(nameof(Controller)))
                .SelectMany(t => t
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(m => m.ReturnType.IsAssignableTo(httpResponseType)))
                .ToList();

        // Higher order function
        private static Func<HttpRequest, HttpResponse> GetResponseFunction(
            Type controllerType,
            MethodInfo controllerAction,
            string path)
        {
            HttpResponse ResponseFunction(HttpRequest request)
            {
                var controllerInstance = CreateController(controllerType, request);

                if (controllerAction.ReturnType != httpResponseType)
                {
                    throw new InvalidOperationException($"Controller action '{path}' does not return a HttpResponse object");
                }

                return (HttpResponse)controllerAction.Invoke(controllerInstance, Array.Empty<object>());
            }

            return ResponseFunction;
        }

        private static object CreateController(Type controller, HttpRequest request)
            => Activator.CreateInstance(controller, request);

        private static TController CreateController<TController>(HttpRequest request)
            => (TController)CreateController(typeof(TController), request);

        private static void MapDefaultRoutes(
            IRoutingTable routingTable,
            string controllerName,
            string actionName,
            Func<HttpRequest, HttpResponse> responseFunction)
        {
            const string defaultActionName = "Index";
            const string defaultControllerName = "Home";

            if (actionName == defaultActionName)
            {
                routingTable.MapGet($"{controllerName}", responseFunction);

                if (controllerName == defaultControllerName)
                {
                    routingTable.MapGet("/", responseFunction);
                }
            }
        }
    }
}
