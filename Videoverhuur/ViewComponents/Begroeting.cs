using Microsoft.AspNetCore.Mvc;
using VideoLibrary.Models;
using Videoverhuur.Controllers;
using VideoLibrary.Repositories;
using Newtonsoft.Json;

namespace Videoverhuur.ViewComponents;

public class Begroeting : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var sessionklant = HttpContext.Session.GetString("Aangemeld");
        Klant? klant;
        if (!string.IsNullOrEmpty(sessionklant))
        {
            klant = JsonConvert.DeserializeObject<Klant>(sessionklant);
        }
        else
            klant = null;
        
        var begroeting = klant == null ? "Welkom! Meld je aan om te kunnen huren!" : $"Welkom, {klant.Voornaam} {klant.Naam}!";
        return View((object)begroeting);
    }
}
