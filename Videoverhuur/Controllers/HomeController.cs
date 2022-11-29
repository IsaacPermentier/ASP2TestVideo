using Microsoft.AspNetCore.Mvc;
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

        public HomeController(ILogger<HomeController> logger, VideoRepository repository)
        {
            _logger = logger;
            this.repository = repository;
        }

        public IActionResult Index()
        {
            var sessionKlant = HttpContext.Session.GetString("AangemeldeKlant");
            Klant? aangemeldeKlant = null;
            if (!string.IsNullOrEmpty(sessionKlant))
            {
                aangemeldeKlant = JsonConvert.DeserializeObject<Klant>(sessionKlant);
                return RedirectToAction(nameof(GenreKiezen));
            }
            else
                return View(aangemeldeKlant);
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
        public IActionResult FindKlantByName(string naam)
        {
            var klant = repository.FindKlantByName(naam);
            return ToonJuistePagina(klant);
        }
        public IActionResult ToonJuistePagina(Klant klant)
        {
            if (klant != null)
                return View(nameof(GenreKiezen));
            else
                return View(nameof(Index));
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
            List<Film>? winkelmandje = JsonConvert.DeserializeObject<List<Film>>(sessionWinkelMandje);
            return View(winkelmandje);
        }
    }
}