using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPINetCore.API.Models;

namespace WebAPINetCore.API.Validators
{
    public class PlayerValidator : AbstractValidator<Player>
    {
        private readonly ApplicationDbContext _context;

        public PlayerValidator(ApplicationDbContext dbContext)
        {
            _context = dbContext;

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("FirstName is required.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("LastName is required.");

            RuleFor(p => p.TeamId)
                .Must(IsValidTeamId)
                .WithMessage("Invalid TeamId");

            RuleFor(p => p.TeamId)
                .Must(TeamHasSpace)
                .WithMessage("Team already has 8 players");
        }

        private bool IsValidTeamId(int? teamId)
        {
            return _context.Teams.Any(t => t.Id == teamId);
        }

        private bool TeamHasSpace(int? teamId)
        {
            var team = _context.Teams.Include(t => t.Players).FirstOrDefault(t => t.Id == teamId);
            return team.Players is null || team?.Players.Count < 9;
        }
    }
}