namespace Panda.Services
{
    using System;
    using System.Linq;
    using Data;
    using Models;

    public class PackagesService : IPackagesService
    {
        private readonly PandaDbContext db;
        private readonly IReceiptsService receiptsService;

        public PackagesService(PandaDbContext db, 
            IReceiptsService receiptsService)
        {
            this.db = db;
            this.receiptsService = receiptsService;
        }

        public void Create(string description, decimal weight, 
            string shippingAddress, string recipientName)
        {
            var userId = this.db
                .Users
                .Where(u => u.Username == recipientName)
                .Select(u => u.Id)
                .FirstOrDefault();

            if (userId == null)
            {
                return;
            }

            var package = new Package()
            {
                Description = description,
                Weight = weight,
                PackageStatus = PackageStatus.Pending,
                ShippingAddress = shippingAddress,
                RecipientId = userId
            };

            this.db.Packages.Add(package);
            this.db.SaveChanges();
        }

        public IQueryable<Package> GetAllByStatus(PackageStatus status)
        {
            var packages = this.db
                .Packages
                .Where(p => p.PackageStatus == status);

            return packages;
        }

        public void Deliver(string id)
        {
            var package = this.db
                .Packages
                .SingleOrDefault(p => p.Id == id);

            if (package == null)
            {
                return;
            }

            package.PackageStatus = PackageStatus.Delivered;
            this.db.SaveChanges();

            this.receiptsService.CreateFromPackage(package.Weight, package.Id, package.RecipientId);
        }
    }
}
