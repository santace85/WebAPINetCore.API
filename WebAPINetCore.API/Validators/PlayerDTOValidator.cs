using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAPINetCore.API.Models;
using WebAPINetCore.API.DTOs;

namespace WebAPINetCore.API.Validators
{
    public class PlayerDTOValidator : AbstractValidator<PlayerDTO>
    {
        public PlayerDTOValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("FirstName is required.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("LastName is required.");
        }
    }
}