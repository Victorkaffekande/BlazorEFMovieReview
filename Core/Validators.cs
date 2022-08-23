using FluentValidation;

namespace Entities;

public class MovieValidator : AbstractValidator<Movie>
{
    public MovieValidator()
    {
        RuleFor(m => m.ReleaseYear)
            .Must(ValidateReleaseYear);
    }

    private static bool ValidateReleaseYear(int year)
    {
        return year > 0;
    }
    
}