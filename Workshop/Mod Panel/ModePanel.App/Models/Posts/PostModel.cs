namespace ModePanel.App.Models.Posts
{
    using Data.Models;
    using Infrastructure;
    using Infrastructure.Mapping;
    using Infrastructure.Validation.Posts;

    public class PostModel : IMapFrom<Post>
    {
        [Title]
        [Required]
        public string Title { get; set; }

        [Content]
        [Required]
        public string Content { get; set; }
    }
}
