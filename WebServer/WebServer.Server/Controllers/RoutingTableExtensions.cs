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
        private static readonly Type httpResponseType = typeof(HttpResponse);
        private static readonly Type stringType = typeof(string);

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

                var httpMethod = HttpMethod.Get;
                var httpMethodAttribute = controllerAction.GetCustomAttribute<HttpMethodAttribute>();

                if (httpMethodAttribute != null)
                {
                    httpMethod = httpMethodAttribute.HttpMethod;
                }

                routingTable.Map(httpMethod, path, responseFunction);

                MapDefaultRoutes(routingTable, httpMethod, controllerName, actionName, responseFunction);
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
                if (!IsAuthorized(controllerAction, request.Session))
                {
                    return new HttpResponse(HttpStatusCode.Unauthorized);
                }

                var controllerInstance = CreateController(controllerType, request);

                var parametersValues = GetParametersValues(controllerAction, request);

                return (HttpResponse)controllerAction.Invoke(controllerInstance, parametersValues);
            }

            return ResponseFunction;
        }

        private static object CreateController(Type controllerType, HttpRequest request)
        {
            var controller = (Controller)request.Services.CreateInstance(controllerType);

            return controller;
        }

        private static TController CreateController<TController>(HttpRequest request)
            => (TController)CreateController(typeof(TController), request);

        private static void MapDefaultRoutes(
            IRoutingTable routingTable,
            HttpMethod method,
            string controllerName,
            string actionName,
            Func<HttpRequest, HttpResponse> responseFunction)
        {
            const string defaultActionName = "Index";
            const string defaultControllerName = "Home";

            if (actionName == defaultActionName)
            {
                routingTable.Map(method, $"{controllerName}", responseFunction);

                if (controllerName == defaultControllerName)
                {
                    routingTable.Map(method, "/", responseFunction);
                }
            }
        }

        private static bool IsAuthorized(MethodInfo controllerAction, HttpSession session)
        {
            var authorizationAttribute = controllerAction.DeclaringType
                ?.GetCustomAttribute<AuthorizeAttribute>()
                ?? controllerAction.GetCustomAttribute<AuthorizeAttribute>();

            if (authorizationAttribute != null)
            {
                var userIsAuthorized = session.ContainsKey(Controller.UserSessionKey)
                    && session[Controller.UserSessionKey] != null;

                if (!userIsAuthorized)
                {
                    return false;
                }
            }

            return true;
        }

        private static object[] GetParametersValues(MethodInfo controllerAction, HttpRequest request)
        {
            var actionParameters = controllerAction
                .GetParameters()
                .Select(p => new
                {
                    Name = p.Name,
                    Type = p.ParameterType
                })
                .ToArray();

            var parametersValues = new object[actionParameters.Length];

            for (var i = 0; i < actionParameters.Length; i++)
            {
                var parameter = actionParameters[i];
                var parameterName = actionParameters[i].Name;
                var parameterType = parameter.Type;

                if (parameterType.IsPrimitive || parameterType == stringType)
                {
                    var parameterValue = request.GetValue(parameterName);

                    parametersValues[i] = Convert.ChangeType(parameterValue, parameterType);
                }
                else
                {
                    var instance = Activator.CreateInstance(parameterType);

                    var parameterProperties = parameterType.GetProperties();

                    foreach (var property in parameterProperties)
                    {
                        var value = request.GetValue(property.Name);

                        property.SetValue(instance, Convert.ChangeType(value, property.PropertyType));
                    }

                    parametersValues[i] = instance;
                }
            }

            return parametersValues;
        }

        private static string GetValue(this HttpRequest request, string name)
            => request.Query.GetValueOrDefault(name)
               ?? request.Form.GetValueOrDefault(name);
    }
}
