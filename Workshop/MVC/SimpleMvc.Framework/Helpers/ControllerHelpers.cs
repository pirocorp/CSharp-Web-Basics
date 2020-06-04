namespace SimpleMvc.Framework.Helpers
{
    public static class ControllerHelpers
    {
        public static string GetViewFullQualifiedName(string controllerName, string action)
        {
            //SimpleMvc.App.Views.Home.Index, ...
            var viewFullQualifiedName = string.Format(
                "{0}.{1}.{2}.{3}, {0}",
                MvcContext.Get.AssemblyName,
                MvcContext.Get.ViewsFolder,
                controllerName,
                action);

            return viewFullQualifiedName;
        }

        public static string GetControllerName(object obj)
            => obj.GetType()
                .Name
                .Replace(MvcContext.Get.ControllersSuffix, string.Empty);
    }
}
