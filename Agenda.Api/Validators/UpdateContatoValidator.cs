using Agenda.Core.Dtos;
using FluentValidation;

namespace Agenda.Api.Validators
{
    public class UpdateContatoValidator : AbstractValidator<UpdateContatoDto>
    {
        public UpdateContatoValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MinimumLength(3).WithMessage("O nome dev ter no mínimo 3 caracteres.")
                .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O email é obrigatório.")
                .EmailAddress().WithMessage("O formato do email é inválido")
                .MaximumLength(100).WithMessage("O email deve ter no máximo 100 caracteres.");
            RuleFor(x => x.Telefone)
                .NotEmpty().WithMessage("O telefone é obrigatório.")
                .MinimumLength(9).WithMessage("O telefone deve ter no mínimo 8 caracteres.")
                .MaximumLength(15).WithMessage("O telefone deve ter no máximo 15 caracteres.");
        }
    }
}