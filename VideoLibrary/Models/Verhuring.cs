using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoLibrary.Models;

public class Verhuring
{
    [Key]
    public int VerhuurId { get; set; }
    public int KlantId { get; set; }
    public int FilmId { get; set; }
    public DateTime VerhuurDatum { get; set; }
}
