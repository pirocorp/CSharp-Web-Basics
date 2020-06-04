namespace MyCoolWebServer.Server.Common
{
    using System;

    using Contracts;

    public class InternalServerErrorView : IView
    {
        private readonly Exception _exception;

        public InternalServerErrorView(Exception exception)
        {
            this._exception = exception;
        }

        /// <summary>
        /// Never do this in production
        /// </summary>
        public string View()
        {
            return $"<h1>{this._exception.Message}<h1>" 
                   + "<br />"
                   + $"<h2>{this._exception.StackTrace}</h2>";
        }
    }
}
