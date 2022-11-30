using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using VideoLibrary.Models;
using VideoLibrary.Repositories;
using Videoverhuur.Models;

namespace Videoverhuur.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly VideoRepository repository;
        private readonly VideoDbContext context;

        public HomeController(ILogger<HomeController> logger, VideoRepository repository, VideoDbContext context)
        {
            _logger = logger;
            this.repository = repository;
            this.context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var klant = new Klant();
            return View(klant);
        }
        [HttpPost]
        public IActionResult Index(Klant klant)
        {
            Klant? gekozenKlant = new Klant();
            gekozenKlant = (from k in context.Klanten
                                where k.Naam == klant.Naam.ToUpper() && k.PostCode == klant.PostCode
                                select k).FirstOrDefault();
            if (gekozenKlant == null)
            {
                return View(nameof(Index));
            }
            else
            {
                var geserializeerdeKlant = JsonConvert.SerializeObject(gekozenKlant);
                HttpContext.Session.SetString("Aangemeld", geserializeerdeKlant);
                return RedirectToAction(nameof(GenreKiezen));
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       
        public IActionResult GenreKiezen()
        {
            var genres = repository.GetAllGenres();
            return View(genres);
        }
        public IActionResult Filmkeuze(int genreId)
        {
            var genre = repository.GetGenreById(genreId);
            var films = repository.GetFilmsByGenreId(genreId);
            ViewBag.GenreNaam = genre.GenreNaam;
            return View(films);
        }
        public IActionResult Winkelmandje(int filmId)
        {
            var film = repository.GetFilmById(filmId);
            var sessionWinkelMandje = HttpContext.Session.GetString("Winkelmandje");
            List<Film>? winkelmandje;
            if (string.IsNullOrEmpty(sessionWinkelMandje))
                winkelmandje = new List<Film>();
            else
                winkelmandje = JsonConvert.DeserializeObject<List<Film>>(sessionWinkelMandje);
            if (film != null)
            {
                winkelmandje?.Add(film);
            }
            var geserializeerdWinkelmandje = JsonConvert.SerializeObject(winkelmandje);
            HttpContext.Session.SetString("Winkelmandje", geserializeerdWinkelmandje);
            ViewBag.Winkelmandje = winkelmandje;
            return View();
        }
        
        public IActionResult Verwijderen(int filmId)
        {
            var film = repository.GetFilmById(filmId);
            return View(film);
        }
        public IActionResult Verwijderd(int filmId)
        {
            Film teVerwijderenFilm = new Film();
            var sessionWinkelMandje = HttpContext.Session.GetString("Winkelmandje");
            List<Film>? winkelmandje = JsonConvert.DeserializeObject<List<Film>>(sessionWinkelMandje);
            foreach (var film in winkelmandje)
            {
                if (film.FilmId == filmId)
                    teVerwijderenFilm = film;
            }
            winkelmandje.Remove(teVerwijderenFilm);
            var geserializeerdWinkelmandje = JsonConvert.SerializeObject(winkelmandje);
            HttpContext.Session.SetString("Winkelmandje", geserializeerdWinkelmandje);
            ViewBag.Winkelmandje = winkelmandje;
            return RedirectToAction(nameof(Winkelmandje));
        }

        public IActionResult Afrekenen()
        {
            var sessionWinkelMandje = HttpContext.Session.GetString("Winkelmandje");
            var sessionKlant = HttpContext.Session.GetString("Aangemeld");
            Klant? klant = JsonConvert.DeserializeObject<Klant>(sessionKlant);
            List<Film>? winkelmandje = JsonConvert.DeserializeObject<List<Film>>(sessionWinkelMandje);
            foreach(var film in winkelmandje)
            {
                context.Verhuringen.Add(new Verhuring
                {
                    KlantId = klant.KlantId,
                    FilmId = film.FilmId,
                    VerhuurDatum = DateTime.Today
                });
                var item = repository.GetFilmById(film.FilmId);
                item.InVoorraad--;
                item.UitVoorraad++;
                item.TotaalVerhuurd++;
                context.SaveChanges();
            }
            ViewBag.Klant = klant;
            return View(winkelmandje);
        }
    }
}