using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naloga_3
{
    public class Artikel
    {
        public string Id { get; set; }
        public string Naziv { get; set; }
        public double Cena { get; set; }
        public int Zaloga { get; set; }
        public string DobaviteljId { get; set; }
        public DateTime DatumZadnjeNabave { get; set; }

        public Artikel() { }

        public Artikel(string Id, string Naziv, double Cena, int Zaloga, string DobaviteljId, DateTime DatumZadnjeNabave)
        {
            this.Id = Id;
            this.Naziv = Naziv;
            this.Cena = Cena;
            this.Zaloga = Zaloga;
            this.DobaviteljId = DobaviteljId;
            this.DatumZadnjeNabave = DatumZadnjeNabave;
        }

        public string Izpisi(List<Dobavitelj> seznamDobaviteljev)
        {
            var dob = seznamDobaviteljev.Find(x => x.Id == DobaviteljId);

            string dobNaziv = "";
            
            if(dob != null)
                dobNaziv = dob.Naziv;
            else
                dobNaziv = "Neznan dobavitelj";

            return $"Id: {Id} | Ime: {Naziv} | Cena: {Cena} | Zaloga: {Zaloga} | Dobavitelj: {dobNaziv} | Datum zadnje nabave: {DatumZadnjeNabave:yyyy-MM-dd HH:mm:ss}";
        }
    }
}