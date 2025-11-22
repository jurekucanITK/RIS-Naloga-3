using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.IO;

namespace Naloga_3
{
    public static class SplosneOperacije
    {
        public static void GlavnaNavodila()
        {
            Console.WriteLine("Izberite želeno operacijo: ");
            Console.WriteLine("");
            Console.WriteLine("1 - Vnos artikla\n2 - Izpis artiklov\n3 - Urejanje artiklov\n4 - Vnos dobavitelja\n5 - Izpis dobaviteljev\n6 - Izhod\n");
        }

        public static void NavodilaZaIzpis()
        {
            Console.WriteLine("Izberite želeno operacijo: ");
            Console.WriteLine("");
            Console.WriteLine("1 - Izpiši artikle izbranega dobavitelja\n2 - Izpiši vse artikle\n3 - Izhod\n");
        }

        public static bool ValidirajDTD(string potDoXML, string potDoDTD)
        {
            bool jeVeljaven = true;
            
            try
            {
                XmlReaderSettings nastavitve = new XmlReaderSettings();
                nastavitve.DtdProcessing = DtdProcessing.Parse;
                nastavitve.ValidationType = ValidationType.DTD;
                nastavitve.XmlResolver = new XmlUrlResolver();
                nastavitve.ValidationEventHandler += (sender, e) =>
                {
                    Console.WriteLine("Validation Error: {0}", e.Message);
                    jeVeljaven = false;
                };

                using (XmlReader reader = XmlReader.Create(potDoXML, nastavitve))
                {
                    while (reader.Read()) { }
                }
                return jeVeljaven;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Datoteka '{potDoXML}' ni veljavna: {ex.Message}");
                return false;
            }
        }

        public static bool ValidirajDTD(XDocument doc, string potDoDTD)
        {
            bool jeVeljaven = true;

            XmlReaderSettings nastavitve = new XmlReaderSettings();
            nastavitve.DtdProcessing = DtdProcessing.Parse;
            nastavitve.ValidationType = ValidationType.DTD;
            nastavitve.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            nastavitve.XmlResolver = new XmlUrlResolver();

            nastavitve.ValidationEventHandler += (sender, e) =>
            {
                Console.WriteLine("Validation Error: {0}", e.Message);
                jeVeljaven = false;
            };

            try
            {
                string baseUri = new Uri(Path.GetFullPath(Path.GetDirectoryName(potDoDTD) ?? ".") + Path.DirectorySeparatorChar).AbsoluteUri;

                using (var sr = new StringReader(doc.ToString()))
                using (XmlReader reader = XmlReader.Create(sr, nastavitve, baseUri))
                {
                    while (reader.Read()) { }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Validacija ni uspela: {ex.Message}");
                jeVeljaven = false;
            }

            return jeVeljaven;
        }
    }
}