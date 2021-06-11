﻿namespace WebServer.App.Controllers
{
    using Server.Controllers;
    using Server.Http;

    public class HomeController : Controller
    {
        public HomeController(HttpRequest request) 
            : base(request)
        {
        }

        public HttpResponse Index()
            => this.Html("<h1>Hello World!</h1>");

        public HttpResponse ToLocal() => this.Redirect("http://127.0.0.1:5000/cats");

        public HttpResponse ToSoftUni() => this.Redirect("https://softuni.bg");
    }
}