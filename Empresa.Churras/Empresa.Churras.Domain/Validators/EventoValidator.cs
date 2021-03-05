using Empresa.Churras.Domain.Model.Entities;
using Empresa.Churras.Domain.Services;
using Kernel.Domain.Validation;
using System.Threading.Tasks;

namespace Empresa.Churras.Domain.Validators
{
    public class EventoValidator : Validator<Evento>
    {
        public EventoService Service { get; set; }

        protected async override Task DefaultValidations(ValidatorResult result, Evento entity, string userName)
        {
            await base.DefaultValidations(result, entity, userName);

            if (await Service.TemOutroEventoNoMesmoDia(entity))
                result.AddError("Já existe um Evento nessa Dia!");
        }
    }
}
