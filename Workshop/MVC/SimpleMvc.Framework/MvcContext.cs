namespace SimpleMvc.Framework
{
    /// <summary>
    /// Lazy loading supported
    /// </summary>
    public class MvcContext
    {
        private static MvcContext _instance;

        private MvcContext()
        {
            this.ControllersFolder = "Controllers";
            this.ControllersSuffix = "Controller";
            this.ViewsFolder = "Views";
            this.ModelsFolder = "Models";
            this.ResourcesFolder = "Resources";
            this.DefaultController = "HomeController";
            this.DefaultAction = "Index";
        }

        /// <summary>
        /// Returns the _instance
        /// If _instance is null it will be instantiated. 
        /// </summary>
        public static MvcContext Get => _instance ??= new MvcContext();

        public string AssemblyName { get; set; }

        public string ControllersFolder { get; set; }

        public string ControllersSuffix { get; set; }

        public string ViewsFolder { get; set; }

        public string ModelsFolder { get; set; }

        public string ResourcesFolder { get; set; }

        public string DefaultController { get; set; }

        public string DefaultAction { get; set; }
    }
}
