using FluentValidation;

namespace ModernPhysics.Web.Areas.Admin.Pages.Manage.Posts
{
    public class InputPostValidator : AbstractValidator<InputPostModel>
    {
        public InputPostValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty()
                .WithMessage("Pole 'Tytuł' nie może być puste")
                .MaximumLength(256)
                .WithMessage("'Tytuł' nie może być dłuższy niż 255 znaków");
            
            RuleFor(p => p.FriendlyUrl)
                .MaximumLength(256)
                .WithMessage("'Przyjazny Url' nie może być dłuższy niż 255 znaków")
                .Matches("^[a-zA-Z0-9_-]*$")
                .WithMessage("Dozwolone są tylko duże i małe litery, cyfry, _ oraz -");

            RuleFor(p => p.Shortcut)
                .MaximumLength(500)
                .WithMessage("'Skrót' nie może być dłuższy niż 500 znaków");

            RuleFor(p => p.ContentType)
                .NotNull()
                .WithMessage("'Typ zawartości' jest wymagany");

            RuleFor(p => p.Content)
                .MaximumLength(65535)
                .WithMessage("'Zawartość' nie może być dłuższa niż 16,777,215 znaków");

            RuleFor(p => p.Category)
                .NotEmpty()
                .WithMessage("'Kategoria' jest wymagana");
        }
    }
}