using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoLibrary.Models;

public class Klant
{
    public int KlantId { get; set; }
    public string Naam { get; set; }
    public string Voornaam { get; set; }
    public string Straat_Nr { get; set;}
    public float PostCode { get; set; }
    public string Gemeente { get; set; }
    public int KlantStat { get; set; }
    public int HuurAantal { get; set; }
    public DateTime DatumLid { get; set; }
    public bool Lidgeld { get; set; }
}
