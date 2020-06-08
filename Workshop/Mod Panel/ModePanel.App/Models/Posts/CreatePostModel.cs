namespace ModePanel.App.Models.Posts
{
    using Infrastructure;
    using Infrastructure.Validation.Posts;

    public class CreatePostModel
    {
        [Title]
        [Required]
        public string Title { get; set; }

        [Content]
        [Required]
        public string Content { get; set; }
    }
}
