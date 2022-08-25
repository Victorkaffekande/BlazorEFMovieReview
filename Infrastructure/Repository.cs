using System.Text;
using Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure;

public class Repository :  IRepository
{


    private DbContextOptions<RepositoryDbContext> _opts;

    public Repository()
    {
        _opts = new DbContextOptionsBuilder<RepositoryDbContext>()
            .UseSqlite("Data source=..//GUI/db.db").Options;
    }

    public List<Review> GetReviews()
    {
        using (var Context = new RepositoryDbContext(_opts, ServiceLifetime.Scoped))
        {
            return Context.ReviewTable.Include(r => r.Movie).ToList();
        }
    }

    public List<Movie> GetMovies()
    {
        using (var context = new RepositoryDbContext(_opts,ServiceLifetime.Scoped))
        {
            return context.MovieTable.ToList();
        }
    }

    public Movie DeleteMovie(int movieId)
    {
        using (var context = new RepositoryDbContext(_opts,ServiceLifetime.Scoped))
        {
            var movie = context.MovieTable.Find(movieId);
            context.MovieTable.Remove(movie ?? throw new InvalidOperationException());
            context.SaveChanges();
            return movie;
        }
    }

    public Review DeleteReview(int reviewId)
    {
        using (var context = new RepositoryDbContext(_opts, ServiceLifetime.Scoped))
        {
            var review = context.ReviewTable.Find(reviewId);
            context.ReviewTable.Remove(review ?? throw new InvalidOperationException());
            context.SaveChanges();
            return review;
        }
    }

    public Movie AddMovie(Movie movie)
    {
        using (var context = new RepositoryDbContext(_opts,ServiceLifetime.Scoped))
        {
            var validator = new MovieValidator();
            var result = validator.Validate(movie);
            if (result.IsValid)
            {
                context.MovieTable.Add(movie);
                context.SaveChanges();
            }
        }
        return movie;
    }

    public Review AddReview(Review review)
    {
        using (var context = new RepositoryDbContext(_opts,ServiceLifetime.Scoped))
        {
            review.Movie = context.MovieTable.Find(review.MovieId);
            context.ReviewTable.Add(review);
            context.SaveChanges();
        }
        return review;
    }

    public void Migrate()
    {
        using (var context = new RepositoryDbContext(_opts,ServiceLifetime.Scoped))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}