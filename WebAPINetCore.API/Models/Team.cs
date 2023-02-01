using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;

namespace WebAPINetCore.API.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public List<Player>? Players { get; set; }
    }

    public class TeamValidator : AbstractValidator<Team>
    {
        private readonly ApplicationDbContext _dbContext;

        public TeamValidator(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required.")
                .Must((team, name) => !IsDuplicateName(team))
                .WithMessage("Name must be unique.");

            RuleFor(x => x.Location)
                .NotEmpty()
                .WithMessage("Location is required.")
                .Must((team, location) => !IsDuplicateLocation(team))
                .WithMessage("Location must be unique.");
        }

        private bool IsDuplicateName(Team team)
        {
            return _dbContext.Teams
                .Any(x => x.Name == team.Name && x.Id != team.Id);
        }

        private bool IsDuplicateLocation(Team team)
        {
            return _dbContext.Teams
                .Any(x => x.Location == team.Location && x.Id != team.Id);
        }
    }
}