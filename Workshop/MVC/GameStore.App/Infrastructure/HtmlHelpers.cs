namespace GameStore.App.Infrastructure
{
    using Models.Games;
    using Models.Home;
    using Models.Orders;

    public static class HtmlHelpers
    {
        public static string ToHtml(this GameListingAdminModel game)
            => $@"<tr>
                    <th scope=""row"">{game.Id}</th>
                    <td>{game.Name}</td>
                    <td>{game.Size:F1} GB</td>
                    <td>{game.Price:F2} &euro;</td>
                    <td>
                        <a href=""/admin/editGame?id={game.Id}"" class=""btn btn-success btn-sm"">Edit</a>
                        <a href=""/admin/deleteGame?id={game.Id}"" class=""btn btn-danger btn-sm"">Delete</a>
                    </td>
                </tr>";

        public static string ToHtml(this GameListingHomeModel game, bool isAdmin)
            => $@"
                <div class=""card col-sm-4 thumbnail"">

                    <img class=""card-image-top img-fluid img-thumbnail""
                         onerror=""this.src='https://i.ytimg.com/vi/{game.VideoId}/maxresdefault.jpg';""
                         src=""{game.ThumbnailUrl}"">

                    <div class=""card-body"">
                        <h4 class=""card-title"">{game.Title}</h4>
                        <p class=""card-text""><strong>Price</strong> - {game.Price:F2}&euro;</p>
                        <p class=""card-text""><strong>Size</strong> - {game.Size:F1} GB</p>
                        <p class=""card-text"">{game.Description.Shortify(300)}</p>
                    </div>

                    <div class=""card-footer"">
                        {(!isAdmin ? string.Empty : $@"
                        <a class=""card-button btn btn-warning"" name=""edit"" href=""/admin/editGame?id={game.Id}"">Edit</a>
                        <a class=""card-button btn btn-danger"" name=""delete"" href=""/admin/deleteGame?id={game.Id}"">Delete</a>")}                        

                        <a class=""card-button btn btn-outline-primary"" name=""info"" href=""/games/details?id={game.Id}"">Info</a>
                        <a class=""card-button btn btn-primary"" name=""buy"" href=""/orders/buy?id={game.Id}"">Buy</a>
                    </div>

                </div>";

        public static string ToHtml(this GameListingOrdersModel game)
            => $@"<div class=""list-group-item"">
                        <div class=""media"">
                            <a class=""btn btn-outline-danger btn-lg align-self-center mr-3"" href=""/orders/remove?id={game.Id}"">X</a>
                            <img class=""d-flex mr-4 align-self-center img-thumbnail"" height=""127"" src=""{game.ThumbnailUrl}"" onerror=""https://i.ytimg.com/vi/{game.VideoId}/maxresdefault.jpg""
                                 width=""227"" alt=""{game.Title}"">
                            <div class=""media-body align-self-center"">
                                <a href=""/games/details?id={game.Id}"">
                                    <h4 class=""mb-1 list-group-item-heading""> {game.Title} </h4>
                                </a>
                                <p>
                                    {game.Description.Shortify(300)}
                                </p>
                            </div>
                            <div class=""col-md-2 text-center align-self-center mr-auto"">
                                <h2> {game.Price:F2}&euro; </h2>
                            </div>
                        </div>
                    </div>";

        public static string ToHtml(this GameDetailsModel game, bool isAdmin)
            => $@"<h1 class=""display-3"">{game.Title}</h1>
                <br />

                <iframe width=""560"" height=""315"" src=""https://www.youtube.com/embed/{game.VideoId}"" frameborder=""0""
                        allowfullscreen></iframe>

                <br />
                <br />

                <p>
                    {game.Description}
                </p>

                <p><strong>Price</strong> - {game.Price:F2}&euro;</p>
                <p><strong>Size</strong> - {game.Size:F1} GB</p>
                <p><strong>Release Date</strong> - {game.ReleaseDate.ToShortDateString()}</p>

                <a class=""btn btn-outline-primary"" href=""/"">Back</a>
                {(!isAdmin ? string.Empty : $@"<a class=""btn btn-warning"" href=""/admin/editGame?id={game.Id}"">Edit</a>
                <a class=""btn btn-danger"" href=""/admin/deleteGame?id={game.Id}"">Delete</a>")}
                <a class=""btn btn-primary"" href=""/orders/buy?id={game.Id}"">Buy</a>";
    }
}
