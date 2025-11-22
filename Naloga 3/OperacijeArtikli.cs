using System.Globalization;
using System.Xml.Linq;

namespace Naloga_3
{
    public class OperacijeArtikli
    {
        public static bool ShraniArtikleVDatoteko(string potDoXML, string potDoDTD, List<Artikel> seznamArtiklov)
        {
            XElement artikliElement = new XElement("Artikli",
                from artikel in seznamArtiklov
                select new XElement("Artikel",
                    new XAttribute("id", artikel.Id),
                    new XAttribute("DobaviteljId", artikel.DobaviteljId),
                    new XElement("NazivArtikla", artikel.Naziv),
                    new XElement("Cena",
                        new XAttribute("valuta", "EUR"),
                        artikel.Cena.ToString(CultureInfo.InvariantCulture)),
                    new XElement("Zaloga", artikel.Zaloga),
                    new XElement("DatumZadnjeNabave", artikel.DatumZadnjeNabave.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
                )
            );

            XDocument doc = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XDocumentType("Artikli", null, Path.GetFileName(potDoDTD), null),
                artikliElement
            );

            if (!SplosneOperacije.ValidirajDTD(doc, potDoDTD))
            {
                Console.WriteLine("Generiran XML ni veljaven glede na DTD. Shranjevanje preklicano.");
                return false;
            }

            Console.WriteLine("XML datoteka je veljavna glede na DTD. Artikel je bil uspešno dodan in shranjen!");

            doc.Save(potDoXML);
            return true;
        }

        public static List<Artikel> PreberiArtikleIzDatoteke(string potDoXML, string potDoDTD)
        {
            var seznamArtiklov = new List<Artikel>();

            if (!File.Exists(potDoXML))
                return seznamArtiklov;

            if (!SplosneOperacije.ValidirajDTD(potDoXML, potDoDTD))
            {
                Console.WriteLine("XML datoteka ni veljavna glede na DTD. Branje se nadaljuje brez validacije.\n");
            }

            XDocument doc = XDocument.Load(potDoXML);

            foreach (var a in doc.Descendants("Artikel"))
            {
                string id = (string)a.Attribute("id");
                string dobaviteljId = (string)a.Attribute("dobaviteljId");

                Artikel artikel = new Artikel
                {
                    Id = id,
                    Naziv = (string)a.Element("NazivArtikla"),
                    Cena = double.Parse((string)a.Element("Cena"), CultureInfo.InvariantCulture),
                    Zaloga = (int)a.Element("Zaloga"),
                    DobaviteljId = dobaviteljId,
                    DatumZadnjeNabave = DateTime.ParseExact((string)a.Element("DatumZadnjeNabave"), "yyyy-MM-dd", CultureInfo.InvariantCulture)
                };
                seznamArtiklov.Add(artikel);
            }

            return seznamArtiklov;
        }

        public static string NastaviIdArtiklu(List<Artikel> seznamArtiklov)
        {
            if (seznamArtiklov.Count == 0)
                return "A1";
            else
                return "A" + (seznamArtiklov.Count + 1).ToString();
        }

        public static void VnosArtikla(string potDoXML, string potDoDTD, List<Artikel> seznamArtiklov, List<Dobavitelj> seznamDobaviteljev)
        {
            Artikel novArtikel = new Artikel();
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
                    novArtikel.Naziv = naziv;
                    konecVnosa = true;
                }

            } while (konecVnosa != true);

            do
            {
                konecVnosa = false;

                Console.Write("Vnesi ceno: ");
                string cena = Console.ReadLine();

                if (string.IsNullOrEmpty(cena))
                {
                    Console.WriteLine("Polje s ceno je obvezno!\n");
                }
                else if (!double.TryParse(cena, out double _))
                {
                    Console.WriteLine("Cena ni število!\n");
                }
                else
                {
                    novArtikel.Cena = double.Parse(cena);
                    konecVnosa = true;
                }

            } while (konecVnosa != true);

            do
            {
                konecVnosa = false;

                Console.Write("Vnesi zalogo: ");
                string zaloga = Console.ReadLine();

                if (string.IsNullOrEmpty(zaloga))
                {
                    Console.WriteLine("Polje zalogo je obvezno!\n");
                }
                else if (!int.TryParse(zaloga, out int _))
                {
                    Console.WriteLine("Zaloga ni število!\n");
                }
                else
                {
                    novArtikel.Zaloga = int.Parse(zaloga);
                    konecVnosa = true;
                }

            } while (konecVnosa != true);

            do
            {
                konecVnosa = false;

                Console.Write("Izberi dobavitelja: ");

                for (int i = 0; i < seznamDobaviteljev.Count; i++)
                {
                    Console.WriteLine(i + " - " + seznamDobaviteljev[i].Naziv);
                }

                string dobavitelj = Console.ReadLine();

                if (string.IsNullOrEmpty(dobavitelj))
                {
                    Console.WriteLine("Polje z dobaviteljem je obvezno!\n");
                }
                else if (!int.TryParse(dobavitelj, out int _))
                {
                    Console.WriteLine("Dobavitelj mora biti številka!\n");
                }
                else if (int.Parse(dobavitelj) < 0 || int.Parse(dobavitelj) >= seznamDobaviteljev.Count)
                {
                    Console.WriteLine("Izbrati morate enega od ponujenih dobaviteljev!\n");
                }
                else
                {
                    novArtikel.DobaviteljId = seznamDobaviteljev[int.Parse(dobavitelj)].Id;
                    konecVnosa = true;
                }

            } while (konecVnosa != true);

            novArtikel.DatumZadnjeNabave = DateTime.Today;

            novArtikel.Id = NastaviIdArtiklu(seznamArtiklov);

            var tmpSeznamArtiklov = new List<Artikel>(seznamArtiklov) { novArtikel };

            if (ShraniArtikleVDatoteko(potDoXML, potDoDTD, tmpSeznamArtiklov))
            {
                seznamArtiklov.Add(novArtikel);
            }
        }

        public static void IzpisiArtikle(List<Artikel> seznamArtiklov, List<Dobavitelj> seznamDobaviteljev, string potDoDTD)
        {
            int izbira;
            do
            {
                SplosneOperacije.NavodilaZaIzpis();
                if (!int.TryParse(Console.ReadLine(), out izbira))
                {
                    Console.WriteLine("Izbira mora biti ena od navedenih številk v navodilu! \n");
                    continue;
                }
                else
                {
                    switch (izbira)
                    {
                        case 1:
                            Console.Write("Izberi dobavitelja: ");

                            for (int i = 0; i < seznamDobaviteljev.Count; i++)
                            {
                                Console.WriteLine(i + " - " + seznamDobaviteljev[i].Naziv);
                            }

                            string izbiraDobavitelja = Console.ReadLine();

                            if (string.IsNullOrEmpty(izbiraDobavitelja))
                            {
                                Console.WriteLine("Polje z dobaviteljem je obvezno!\n");
                                break;
                            }
                            else if (!int.TryParse(izbiraDobavitelja, out int _))
                            {
                                Console.WriteLine("Dobavitelj mora biti številka!\n");
                                break;
                            }
                            else if (int.Parse(izbiraDobavitelja) < 0 || int.Parse(izbiraDobavitelja) >= seznamDobaviteljev.Count)
                            {
                                Console.WriteLine("Izbrati morate enega od ponujenih dobaviteljev!\n");
                                break;
                            }

                            string dobaviteljId = seznamDobaviteljev[int.Parse(izbiraDobavitelja)].Id;

                            Console.Write("Vnesi število, za katero želite izpisati artikle, ki imajo manjšo zalogo, kot to število: ");
                            string zaloga = Console.ReadLine();

                            if (!int.TryParse(zaloga, out int _))
                            {
                                Console.WriteLine("Zaloga mora biti število!");
                            }
                            else
                            {
                                var artikliDobavitelja = seznamArtiklov.Where(a => a.DobaviteljId == dobaviteljId && a.Zaloga < int.Parse(zaloga)).ToList();

                                if (!artikliDobavitelja.Any())
                                {
                                    Console.WriteLine("Ni artiklov tega dobavitelja oz. ni artiklov, ki bi imele manjšo zalogo, kot izbrano število!\n");
                                }
                                else
                                {
                                    foreach (Artikel artikel in artikliDobavitelja)
                                    {
                                        Console.WriteLine(artikel.Izpisi(seznamDobaviteljev));
                                    }

                                    Console.WriteLine();

                                    string imeDatoteke = $"Artikli_{dobaviteljId}_zaloga_manj_kot_{zaloga}.xml";
                                    ShraniArtikleVDatoteko(@"..\..\..\" + imeDatoteke, potDoDTD, artikliDobavitelja);
                                }
                            }
                            break;
                        case 2:
                            if (!seznamArtiklov.Any())
                            {
                                Console.WriteLine("Ni vnešenih artiklov!\n");
                            }
                            else
                            {
                                foreach (Artikel artikel in seznamArtiklov)
                                {
                                    Console.WriteLine(artikel.Izpisi(seznamDobaviteljev));
                                }
                                Console.WriteLine();
                            }
                            break;
                        case 3:
                            break;
                        default:
                            Console.WriteLine("Izbira mora biti ena od navedenih številk v navodilu! \n");
                            break;
                    }
                }
            } while (izbira != 3);
        }

        public static void UrediArtikle(List<Artikel> seznamArtiklov, List<Dobavitelj> seznamDobaviteljev, string potDoDTD)
        {
            Console.Write("Vnesi vrednost popusta v procentih: ");
            string vrednost = Console.ReadLine();

            if (!int.TryParse(vrednost, out int _))
            {
                Console.WriteLine("Vrednost mora biti število!");
            }
            else if (int.Parse(vrednost) < 0 || int.Parse(vrednost) > 100)
            {
                Console.WriteLine("Vrednost popusta mora biti med 0 in 100!");
            }
            else
            {
                List<Artikel> seznamArtiklovVAkciji = seznamArtiklov.Select(a => new Artikel(a.Id, a.Naziv, a.Cena, a.Zaloga, a.DobaviteljId, a.DatumZadnjeNabave)).ToList();
                foreach (Artikel artikel in seznamArtiklovVAkciji)
                {
                    artikel.Cena = artikel.Cena * (1 - int.Parse(vrednost) / 100.0);
                }

                string imeDatoteke = $"Artikli_v_akciji_{Guid.NewGuid().ToString("N")[..8]}.xml";

                ShraniArtikleVDatoteko(@"..\..\..\" + imeDatoteke, potDoDTD, seznamArtiklovVAkciji);
            }
        }
    }
}
