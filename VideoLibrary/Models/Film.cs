using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoLibrary.Models;

public class Film
{
    public int FilmId { get; set; }
    public string Titel { get; set; }
    public int GenreId { get; set; }
    public int InVoorraad { get; set; }
    public int UitVoorraad { get; set; }
    [DisplayFormat(DataFormatString = "{0:€ #,##0.00}")]
    public decimal Prijs { get; set; }
    public int TotaalVerhuurd { get; set; }
    public Genre genre { get; set; }
}
