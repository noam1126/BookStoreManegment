using System.Xml.Serialization;

namespace Book.Store.Models.DM
{
    [XmlRoot("bookstore")]
    public class Bookstore
    {
        [XmlElement("book")]
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
