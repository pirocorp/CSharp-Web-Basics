namespace WebServer.Common
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
        
        public string View()
        {
            return $"<h1>{this._exception.Message}</h1><h2>{this._exception.StackTrace}</h2>";
        }
    }
}
