namespace SimpleMvc.Framework.Routers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Attributes.Methods;
    using Contracts;
    using Controllers;
    using Helpers;
    using WebServer.Contracts;
    using WebServer.Http.Contracts;
    using WebServer.Http.Response;

    public class ControllerRouter : IHandleable
    {
        private IDictionary<string, string> _getParameters;
        private IDictionary<string, string> _postParameters;
        private string _requestMethod;
        private Controller _controllerInstance;
        private string _controllerName;
        private string _actionName;
        private object[] _methodParameters;

        public IHttpResponse Handle(IHttpRequest request)
        {
            this._controllerInstance = null;
            this._actionName = null;
            this._methodParameters = null;

            this._getParameters = new Dictionary<string, string>(request.UrlParameters);
            this._postParameters = new Dictionary<string, string>(request.FormData);
            this._requestMethod = request.Method.ToString().ToUpper();

            this.PrepareControllerAndActionNames(request);

            var methodInfo = this.GetActionForExecution();

            if (methodInfo == null)
            {
                return new NotFoundResponse();
            }

            this.PrepareMethodParameters(methodInfo);

            try
            {
                if (this._controllerInstance != null)
                {
                    this._controllerInstance.Request = request;
                    this._controllerInstance.InitializeController();
                }

                return this.GetResponse(methodInfo, this._controllerInstance);
            }
            catch (Exception e)
            {
                return new InternalServerErrorResponse(e);
            }
        }

        /// <summary>
        /// Instantiates Controller child with parameterless constructor
        /// </summary>
        /// <param name="controllerType">Type (class) which will be instantiated.
        /// Must inherit Controller class
        /// </param>
        /// <returns>Controller instance</returns>
        protected virtual Controller CreateController(Type controllerType)
            => Activator.CreateInstance(controllerType) as Controller;

        private void PrepareControllerAndActionNames(IHttpRequest request)
        {
            var pathParts = request.Path.Split(new []{'/', '?'}, StringSplitOptions.RemoveEmptyEntries);

            if (pathParts.Length < 1)
            {
                this._controllerName = MvcContext.Get.DefaultController;
                this._actionName = MvcContext.Get.DefaultAction;

                return;
            }
            else if (pathParts.Length < 2)
            {
                this._controllerName = $"{pathParts[0].Capitalize()}{MvcContext.Get.ControllersSuffix}";
                this._actionName = MvcContext.Get.DefaultAction;

                return;
            }

            this._controllerName = $"{pathParts[0].Capitalize()}{MvcContext.Get.ControllersSuffix}";
            this._actionName = pathParts[1].Capitalize();
        }

        private object GetControllerInstance()
        {
            if (this._controllerInstance != null)
            {
                return this._controllerInstance;
            }

            var controllerFullQualifiedName = string.Format(
                "{0}.{1}.{2}, {0}", 
                MvcContext.Get.AssemblyName,
                MvcContext.Get.ControllersFolder,
                this._controllerName);

            var controllerType = Type.GetType(controllerFullQualifiedName);

            if (controllerType == null)
            {
                return null;
            }

            this._controllerInstance = this.CreateController(controllerType);

            return this._controllerInstance;
        }

        private IEnumerable<MethodInfo> GetSuitableMethods()
        {
            var controller = this.GetControllerInstance();

            if (controller == null)
            {
                return Enumerable.Empty<MethodInfo>();
            }

            return controller
                .GetType()
                .GetMethods()
                .Where(m => m.Name == this._actionName);
        }

        private MethodInfo GetActionForExecution()
        {
            foreach (var method in this.GetSuitableMethods())
            {
                var httpMethodAttributes = method
                    .GetCustomAttributes()
                    .Where(a => a is HttpMethodAttribute)
                    .Cast<HttpMethodAttribute>()
                    .ToArray();

                if (!httpMethodAttributes.Any() && this._requestMethod == "GET")
                {
                    return method;
                }

                foreach (var httpMethodAttribute in httpMethodAttributes)
                {
                    if (httpMethodAttribute.IsValid(this._requestMethod))
                    {
                        return method;
                    }
                }
            }

            return null;
        }

        private void ProcessPrimitiveParameter(ParameterInfo parameter, int index)
        {
            var getParameterValue = this._getParameters[parameter.Name];

            var value = Convert.ChangeType(getParameterValue, parameter.ParameterType);

            this._methodParameters[index] = value;
        }

        private void ProcessModelParameter(ParameterInfo parameter, int index)
        {
            var modelType = parameter.ParameterType;
            var modelInstance = Activator.CreateInstance(modelType);

            var modelProperties = modelType
                .GetProperties();

            foreach (var modelProperty in modelProperties)
            {
                var postParameterValue = this._postParameters[modelProperty.Name];

                try
                {
                    var value = Convert.ChangeType(postParameterValue, modelProperty.PropertyType);
                    modelProperty.SetValue(modelInstance, value);
                }
                catch
                {
                    var type = modelProperty.PropertyType;
                    var defaultValue = type.IsValueType ? Activator.CreateInstance(type) : null;
                    modelProperty.SetValue(modelInstance, defaultValue);
                }
            }

            this._methodParameters[index] = Convert.ChangeType(modelInstance, modelType);
        }

        private void PrepareMethodParameters(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();

            this._methodParameters = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                if (parameter.ParameterType.IsPrimitive 
                    || parameter.ParameterType == typeof(string))
                {
                    this.ProcessPrimitiveParameter(parameter, i);
                }
                else
                {
                    this.ProcessModelParameter(parameter, i);
                }
            }
        }

        private IHttpResponse GetResponse(MethodInfo method, object controller)
        {
            var actionResult = method.Invoke(controller, this._methodParameters)
                as IActionResult;

            if (actionResult == null)
            {
                var methodResultAsHttpResponse = actionResult as IHttpResponse;

                if (methodResultAsHttpResponse != null)
                {
                    return methodResultAsHttpResponse;
                }
                else
                {
                    throw new InvalidOperationException("Controller actions should return either IActionResult or IHttpResponse");
                }
            }

            return actionResult.Invoke();
        }
    }
}
