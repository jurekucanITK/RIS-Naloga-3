using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naloga_3
{
    public class Dobavitelj
    {
        public string Id { get; set; }
        public string Naziv { get; set; }
        public string Naslov { get; set; }
        public string Davcna_st { get; set; }
        public string Kontakt { get; set; }
        public string Opis { get; set; }

        public Dobavitelj() { }

        public Dobavitelj(string Id, string Naziv, string Naslov, string Davcna_st, string Kontakt, string Opis)
        {
            this.Id = Id;
            this.Naziv = Naziv;
            this.Naslov = Naslov;
            this.Davcna_st = Davcna_st;
            this.Kontakt = Kontakt;
            this.Opis = Opis;
        }

        public string Izpisi()
        {
            return $"Id: {Id} | Naziv: {Naziv} | Naslov: {Naslov} | Davčna številka: {Davcna_st} | Kontakt: {Kontakt} | Opis: {Opis}";
        }
    }
}
