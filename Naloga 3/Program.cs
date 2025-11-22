using Naloga_3;

const string potDoDTD = @"..\..\..\trgovina.dtd";
const string potDoArtiklovXML = @"..\..\..\artikli.xml";
const string potDoDobaviteljevXML = @"..\..\..\dobavitelji.xml";

List<Dobavitelj> seznamDobaviteljev = new List<Dobavitelj>();

if (File.Exists(potDoDobaviteljevXML))
    seznamDobaviteljev = OperacijeDobavitelji.PreberiDobaviteljeIzDatoteke(potDoDobaviteljevXML, potDoDTD);


List<Artikel> seznamArtiklov = new List<Artikel>();

if (File.Exists(potDoArtiklovXML))
    seznamArtiklov = OperacijeArtikli.PreberiArtikleIzDatoteke(potDoArtiklovXML, potDoDTD);

int izbira;

do
{
    SplosneOperacije.GlavnaNavodila();

    if (!int.TryParse(Console.ReadLine(), out izbira))
    {
        Console.WriteLine("Izbira mora biti ena od navedenih številk v navodilu! \n");
    }
    else
    {
        switch (izbira)
        {
            case 1:
                OperacijeArtikli.VnosArtikla(potDoArtiklovXML, potDoDTD, seznamArtiklov, seznamDobaviteljev);
                break;

            case 2:
                OperacijeArtikli.IzpisiArtikle(seznamArtiklov, seznamDobaviteljev, potDoDTD);
                break;

            case 3:
                OperacijeArtikli.UrediArtikle(seznamArtiklov, seznamDobaviteljev, potDoDTD);
                break;

            case 4:
                OperacijeDobavitelji.VnosDobavitelja(potDoDobaviteljevXML, seznamDobaviteljev, potDoDTD);
                break;

            case 5:
                OperacijeDobavitelji.IzpisiDobavitelje(seznamDobaviteljev);
                break;

            default:
                Console.WriteLine("Izbira mora biti ena od navedenih številk v navodilu! \n");
                break;
        }
    }

} while (izbira != 6);