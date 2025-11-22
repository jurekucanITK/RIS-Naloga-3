using System.Xml.Linq;

namespace Naloga_3
{
    public class OperacijeDobavitelji
    {
        public static bool ShraniDobaviteljeVDatoteko(string potDoXML, string potDoDTD, List<Dobavitelj> seznamDobaviteljev)
        {
            XElement dobaviteljiElement = new XElement("Dobavitelji",
                from dobavitelj in seznamDobaviteljev
                select new XElement("Dobavitelj",
                    new XAttribute("id", dobavitelj.Id),
                    new XElement("NazivDobavitelja", dobavitelj.Naziv),
                    new XElement("Naslov", dobavitelj.Naslov),
                    new XElement("DavcnaSt", dobavitelj.Davcna_st),
                    new XElement("Kontakt", dobavitelj.Kontakt),
                    new XElement("Opis", dobavitelj.Opis)
                )
            );

            XDocument doc = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XDocumentType("Dobavitelji", null, Path.GetFileName(potDoDTD), null),
                dobaviteljiElement
            );

            if (!SplosneOperacije.ValidirajDTD(doc, potDoDTD))
            {
                Console.WriteLine("Generiran XML ni veljaven glede na DTD. Shranjevanje preklicano.");
                return false;
            }

            Console.WriteLine("XML datoteka je veljavna glede na DTD. Dobavitelj je bil uspešno dodan in shranjen!");

            doc.Save(potDoXML);
            return true;
        }

        public static List<Dobavitelj> PreberiDobaviteljeIzDatoteke(string potDoXML, string potDoDTD)
        {
            var seznamDobaviteljev = new List<Dobavitelj>();
            if (!File.Exists(potDoXML))
                return seznamDobaviteljev;

            if (!SplosneOperacije.ValidirajDTD(potDoXML, potDoDTD))
            {
                Console.WriteLine("XML datoteka ni veljavna glede na DTD. Branje se nadaljuje brez validacije.\n");
            }

            XDocument doc = XDocument.Load(potDoXML);

            foreach (var d in doc.Descendants("Dobavitelj"))
            {
                string id = (string)d.Attribute("id");

                Dobavitelj dobavitelj = new Dobavitelj
                {
                    Id = id,
                    Naziv = (string)d.Element("NazivDobavitelja"),
                    Naslov = (string)d.Element("Naslov"),
                    Davcna_st = (string)d.Element("DavcnaSt"),
                    Kontakt = (string)d.Element("Kontakt"),
                    Opis = (string)d.Element("Opis")
                };
                seznamDobaviteljev.Add(dobavitelj);
            }

            return seznamDobaviteljev;
        }

        public static string NastaviIdDobavitelju(List<Dobavitelj> seznamDobaviteljev)
        {
            if (seznamDobaviteljev.Count == 0)
                return "D1";
            else
                return "D" + (seznamDobaviteljev.Count + 1).ToString();
        }

        public static void VnosDobavitelja(string pot, List<Dobavitelj> seznamDobaviteljev, string potDoDTD)
        {
            Dobavitelj novDobavitelj = new Dobavitelj();
            bool konecVnosa = false;

            do
            {
                Console.Write("Vnesi naziv: ");
                string naziv = Console.ReadLine();

                if (string.IsNullOrEmpty(naziv))
                {
                    Console.WriteLine("Polje z nazivom je obvezno!\n");
                }
                else if (naziv[0] == ' ')
                {
                    Console.WriteLine("Nepravilna vrednost naziva!\n");
                }
                else
                {
                    novDobavitelj.Naziv = naziv;
                    konecVnosa = true;
                }

            } while (konecVnosa != true);

            Console.Write("Vnesi naslov: ");
            novDobavitelj.Naslov = Console.ReadLine() ?? "";

            do
            {
                konecVnosa = false;
                Console.Write("Vnesi davčno številko: ");
                string dav = Console.ReadLine();

                if (string.IsNullOrEmpty(dav))
                {
                    Console.WriteLine("Polje z davčno številko je obvezno!\n");
                }
                else
                {
                    novDobavitelj.Davcna_st = dav;
                    konecVnosa = true;
                }

            } while (konecVnosa != true);

            Console.Write("Vnesi kontakt: ");
            novDobavitelj.Kontakt = Console.ReadLine() ?? "";

            Console.Write("Vnesi opis: ");
            novDobavitelj.Opis = Console.ReadLine() ?? "";

            novDobavitelj.Id = NastaviIdDobavitelju(seznamDobaviteljev);

            var tempSeznamDobaviteljev = new List<Dobavitelj>(seznamDobaviteljev) { novDobavitelj };
            if (ShraniDobaviteljeVDatoteko(pot, potDoDTD, tempSeznamDobaviteljev))
            {
                seznamDobaviteljev.Add(novDobavitelj);
            }
        }

        public static void IzpisiDobavitelje(List<Dobavitelj> seznamDobaviteljev)
        {
            if (!seznamDobaviteljev.Any())
            {
                Console.WriteLine("Ni vnešenih dobaviteljev!\n");
                return;
            }

            foreach (Dobavitelj d in seznamDobaviteljev)
            {
                Console.WriteLine(d.Izpisi());
            }
            Console.WriteLine();
        }
    }
}
