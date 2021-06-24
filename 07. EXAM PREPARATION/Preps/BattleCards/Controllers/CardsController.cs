namespace BattleCards.Controllers
{
    using System.Linq;
    using Models.Cards;
    using MyWebServer.Controllers;
    using MyWebServer.Http;
    using Services;

    public class CardsController : Controller
    {
        private readonly IValidator validator;
        private readonly ICardsService cardsService;

        public CardsController(
            IValidator validator,
            ICardsService cardsService)
        {
            this.validator = validator;
            this.cardsService = cardsService;
        }

        [Authorize]
        public HttpResponse Add() => this.View();

        [HttpPost]
        [Authorize]
        public HttpResponse Add(CardCreateFormModel model)
        {
            var modelErrors = this.validator.ValidateCardCreation(model);

            if (modelErrors.Any())
            {
                return this.Error(modelErrors);
            }

            this.cardsService.CreateCard(
                model.Name,
                model.Image,
                model.Keyword,
                model.Attack,
                model.Health,
                model.Description);

            return this.Redirect("/Cards/All");
        }

        [Authorize]
        public HttpResponse All()
        {
            var cards = this.cardsService.All(this.User.Id);

            return this.View(cards);
        }

        [Authorize]
        public HttpResponse Collection()
        {
            var cards = this.cardsService.GetCollection(this.User.Id);

            return this.View(cards);
        }

        [Authorize]
        public HttpResponse AddToCollection(int cardId)
        {
            this.cardsService.AddCardToCollection(cardId, this.User.Id);

            return this.Redirect("/Cards/All");
        }

        [Authorize]
        public HttpResponse RemoveFromCollection(int cardId)
        {
            this.cardsService.RemoveCardFromCollection(cardId, this.User.Id);

            return this.Redirect("/Cards/Collection");
        }
    }
}
