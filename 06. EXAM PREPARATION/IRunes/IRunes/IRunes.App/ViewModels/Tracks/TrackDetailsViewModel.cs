namespace IRunes.App.ViewModels.Tracks
{
    using System.Text.RegularExpressions;

    public class TrackDetailsViewModel
    {
        private static readonly Regex YoutubeVideoRegex = 
            new Regex(@"youtu(?:\.be|be\.com)/(?:(.*)v(/|=)|(.*/)?)(?<id>[a-zA-Z0-9-_]+)", RegexOptions.IgnoreCase);

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Link { get; set; }

        public string AlbumId { get; set; }

        public string IFrameSource
        {
            get
            {
                if (this.Link.Contains("youtube"))
                {
                    var videoId = YoutubeVideoRegex.Match(this.Link).Groups["id"].Value;

                    return $"https://www.youtube.com/embed/{videoId}";
                }
                else
                {
                    return this.Link;
                }
            }
        }
    }
}
