namespace Git.Models.Commits
{
    public class CommitListingModel
    {
        public string Id { get; init; }

        public string Repository { get; init; }

        public string Description { get; init; }

        public string CreatedOn { get; init; }
    }
}
