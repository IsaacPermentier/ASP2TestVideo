using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace VideoLibrary.Models;

public class VideoDbContext : DbContext
{
    public DbSet<Film> Films { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Klant> Klanten { get; set; }
    public DbSet<Verhuring> Verhuringen { get; set; }
    public VideoDbContext() {}
    public VideoDbContext(DbContextOptions options) : base(options) {}
}
