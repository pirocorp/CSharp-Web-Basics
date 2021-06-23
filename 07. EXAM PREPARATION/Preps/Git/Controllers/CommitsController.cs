namespace Git.Controllers
{
    using System.Linq;
    using Data;
    using Data.Models;
    using Models.Commits;
    using MyWebServer.Controllers;
    using MyWebServer.Http;

    using static Data.DataConstants;

    public class CommitsController : Controller
    {
        private readonly GitDbContext dbContext;

        public CommitsController(GitDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Authorize]
        public HttpResponse All()
        {
            var commits = this.dbContext.Commits
                .Where(c => c.CreatorId == this.User.Id)
                .OrderByDescending(c => c.CreateOn)
                .Select(c => new CommitListingModel()
                {
                    Id = c.Id,
                    Description = c.Description,
                    CreatedOn = c.CreateOn.ToLocalTime().ToString("F"),
                    Repository = c.Repository.Name,

                })
                .ToList();

            return this.View(commits);
        }

        [Authorize]
        public HttpResponse Create(string id)
        {
            var repo = this.dbContext.Repositories
                .Where(r => r.Id == id)
                .Select(r => new CommitToRepositoryViewModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                })
                .FirstOrDefault();

            if (repo is null)
            {
                return this.BadRequest();
            }

            return this.View(repo);
        }


        [Authorize]
        [HttpPost]
        public HttpResponse Create(CreateCommitFormModel model)
        {
            if (!this.dbContext.Repositories.Any(r => r.Id == model.Id))
            {
                return this.NotFound();
            }

            if (model.Description.Length < CommitMinDescription )
            {
                return this.Error($"Commit description must be at least {CommitMinDescription} characters long.");
            }

            var commit = new Commit()
            {
                RepositoryId = model.Id,
                Description = model.Description,
                CreatorId = this.User.Id,
            };

            this.dbContext.Add(commit);
            this.dbContext.SaveChanges();

            return this.Redirect("/Repositories/All");
        }

        [Authorize]
        public HttpResponse Delete(string id)
        {
            var commit = this.dbContext.Commits.FirstOrDefault(c => c.Id == id);

            if (commit is null || commit.CreatorId != this.User.Id)
            {
                return this.BadRequest();
            }

            this.dbContext.Commits.Remove(commit);
            this.dbContext.SaveChanges();

            return this.Redirect("/Commits/All");
        }
    }
}
