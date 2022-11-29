using Microsoft.AspNetCore.Mvc;
using VideoLibrary.Models;
using VideoLibrary.Repositories;

namespace Videoverhuur.ViewComponents;

public class Begroeting : ViewComponent
{
    private readonly VideoRepository repository;
    public Begroeting(VideoRepository repository)
    {
        this.repository = repository;
    }
    public IViewComponentResult Invoke(string klantnaam)
    {
        var klant = repository.FindKlantByName(klantnaam);
        var begroeting = klant == null ? "Welkom! Meld je aan om te kunnen huren!" : $"Welkom, {klant.Voornaam} {klant.Naam}";
        return View((object)begroeting);
    }
}
