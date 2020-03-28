using FluentValidation;

namespace ModernPhysics.Web.Areas.Admin.Pages.Manage.Quizzes
{
    public class InputPostValidator : AbstractValidator<InputQuizModel>
    {
        public InputPostValidator()
        {
            RuleFor(p => p.Title)
                .NotEmpty()
                .WithMessage("Pole 'Tytuł' nie może być puste")
                .MaximumLength(255)
                .WithMessage("'Tytuł' nie może być dłuższy niż 255 znaków")
                .Matches("^[a-zA-Z0-9ąęóśłżźćńĄĘÓŚŁŻŹĆŃ !?\"#$%&'()*+,./:;<=>@[\\]^`{|}~_-]*$")
                .WithMessage("Dozwolone są tylko duże i małe litery, cyfry, spacje, oraz znaki specjalne.");
            
            RuleFor(p => p.FriendlyUrl)
                .MaximumLength(255)
                .WithMessage("'Przyjazny Url' nie może być dłuższy niż 255 znaków")
                .Matches("^[a-zA-Z0-9_-]*$")
                .WithMessage("Dozwolone są tylko duże i małe litery, cyfry, _ oraz -");
        }
    }
}