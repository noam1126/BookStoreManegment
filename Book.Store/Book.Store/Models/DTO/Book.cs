using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Book.Store.Models.DTO
{
    public class Book
    {
        [Required]
        [MaxLength(100)]
        public string Category { get; set; }

        [Required]
        [MaxLength(50)]
        public string Isbn { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public List<string>? Authors { get; set; }

        public int Year { get; set; }

        public decimal Price { get; set; }

        public DM.Book ToDataModel()
        {
            return new DM.Book
            {
                Authors = Authors,
                Title = Title,
                Year = Year,
                Price = Price,
                Category = Category,
                Isbn = Isbn,
            };
        }

    }
}
