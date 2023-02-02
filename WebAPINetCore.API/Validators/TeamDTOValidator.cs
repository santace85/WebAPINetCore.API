using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPINetCore.API.DTOs;
using WebAPINetCore.API.Models;

namespace WebAPINetCore.API.Validators
{
    public class TeamDTOValidator : AbstractValidator<TeamDTO>
    {
        private readonly ApplicationDbContext _dbContext;

        public TeamDTOValidator(ApplicationDbContext dbContext)
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

        private bool IsDuplicateName(TeamDTO teamDTO)
        {
            return _dbContext.Teams
                .Any(x => x.Name == teamDTO.Name && x.Id != teamDTO.Id);
        }

        private bool IsDuplicateLocation(TeamDTO teamDTO)
        {
            return _dbContext.Teams
                .Any(x => x.Location == teamDTO.Location && x.Id != teamDTO.Id);
        }
    }
}