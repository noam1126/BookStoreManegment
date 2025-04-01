using System.Xml.Serialization;

namespace Book.Store.Models.DM
{
    public class Title
    {
        [XmlAttribute("lang")]
        public string Language { get; set; }

        [XmlText]
        public string Text { get; set; }
    }
}
