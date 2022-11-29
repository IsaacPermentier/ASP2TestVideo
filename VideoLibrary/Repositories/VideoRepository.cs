using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoLibrary.Models;

namespace VideoLibrary.Repositories;

public class VideoRepository
{
    private readonly VideoDbContext context;
	public VideoRepository(VideoDbContext context)
	{
		this.context = context;
	}
	public Klant? GetKlantById(int id)
		=> context.Klanten.Find(id);

	public IEnumerable<Klant> GetAllKlanten()
		=> context.Klanten.AsNoTracking();
	public Klant? FindKlantByName(string naam)
	{
		var klant = context.Klanten.Where(k => k.Naam == naam)
			.FirstOrDefault();
		return klant;
	}
	public IEnumerable<Genre> GetAllGenres()
		=> context.Genres.AsNoTracking();

	public Genre? GetGenreById(int id)
		=> context.Genres.Find(id);

	public IEnumerable<Film> GetAllFilms()
		=> context.Films.AsNoTracking();

	public IEnumerable<Film> GetFilmsByGenreId(int genreId)
	{
		var films = context.Films.Include(f => f.genre)
			.Where(f => f.GenreId == genreId)
			.OrderBy(f => f.genre.GenreNaam)
			.ToList();
		return films;
	}
	public Film? GetFilmById(int id)
		=> context.Films.Find(id);
}
