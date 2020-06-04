namespace MyCoolWebServer.ByTheCakeApplication.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        public Product()
        {
            this.Orders = new HashSet<OrderProduct>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public decimal Price { get; set; }

        [Required]
        [MaxLength(2000)] //By the StackOverflow :)
        public string ImageUrl { get; set; }

        public ICollection<OrderProduct> Orders { get; set; }
    }
}
