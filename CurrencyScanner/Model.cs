using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CurrencyScanner
{
    [XmlRoot("ValCurs", Namespace = "")]
    public class CurrencyList
    {
        [XmlAttribute("Date")]
        public string Date;
        [XmlAttribute("name")]
        public string name;
        [XmlElement("Valute")]
        public List<Currency> Currencies;
    }

    [XmlRoot("Valute", Namespace = "")]
    public class Currency
    {
        [XmlAttribute("ID")]
        public string ID;
        [XmlElement("NumCode")]
        public int NumCode;
        [XmlElement("CharCode")]
        public string Ticker;
        [XmlElement("Nominal")]
        public int Nominal;
        [XmlElement("Name")]
        public string Fullname;
        [XmlElement("Value")]
        public string Price;
    }
}
