using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Book.Store.Models.DM
{
    public class Book
    {
        [XmlAttribute("category")]
        public string Category { get; set; }

        [XmlElement("isbn")]
        public string Isbn { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("author")]
        public List<string> Authors { get; set; }

        [XmlElement("year")]
        public int Year { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }

        public bool Valid => !string.IsNullOrWhiteSpace(Isbn) &&
                               !string.IsNullOrWhiteSpace(Title) &&
                               Year >= 0 &&
                               Price >= 0;

        public DTO.Book ToDTO()
        {
            return new DTO.Book
            {
                Title = Title,
                Isbn = Isbn,
                Category = Category,
                Authors = Authors,
                Year = Year,
                Price = Price

            };
        }
    }



}
