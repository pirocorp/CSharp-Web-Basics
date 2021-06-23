namespace Git.Controllers
{
    using System.Linq;
    using Data;
    using Data.Models;
    using Models.Repositories;
    using MyWebServer.Controllers;
    using MyWebServer.Http;
    using Services;

    using static Data.DataConstants;

    public class RepositoriesController : Controller
    {
        private readonly GitDbContext dbContext;
        private readonly IValidator validator;

        public RepositoriesController(
            GitDbContext dbContext,
            IValidator validator)
        {
            this.dbContext = dbContext;
            this.validator = validator;
        }

        public HttpResponse All()
        {
            IQueryable<Repository> repositoriesQuery; 

            if (!this.User.IsAuthenticated)
            {
                repositoriesQuery = this.dbContext.Repositories
                    .Where(r => r.IsPublic);
            }
            else
            {
                repositoriesQuery = this.dbContext.Repositories
                    .Where(r => r.IsPublic || r.OwnerId == this.User.Id);
            }

            var data = repositoriesQuery
                .OrderByDescending(r => r.CreateOn)
                .Select(r => new RepositoryListingViewModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Owner = r.Owner.Username,
                    CreatedOn = r.CreateOn.ToLocalTime().ToString("F"),
                    Commits = r.Commits.Count(),
                })
                .ToList();

            return this.View(data);
        }
        
        [Authorize]
        public HttpResponse Create() => this.View();

        [HttpPost]
        [Authorize]
        public HttpResponse Create(RepositoryCreateFormModel model)
        {
            var modelErrors = this.validator.ValidateRepository(model);

            if (modelErrors.Any())
            {
                return this.Error(modelErrors);
            }

            var repository = new Repository()
            {
                Name = model.Name,
                IsPublic = model.RepositoryType == RepositoryPublicType,
                OwnerId = this.User.Id,
            };

            this.dbContext.Add(repository);
            this.dbContext.SaveChanges();

            return this.Redirect("/Repositories/All");
        }
    }
}
