using Kernel.Domain.Validation;
using System.Threading.Tasks;
using Univem.Churras.Domain.Model.Entities;
using Univem.Churras.Domain.Services;

namespace Univem.Churras.Domain.Validators
{
    public class EventoValidator : Validator<Evento>
    {
        public EventoService Service { get; set; }

        protected async override Task DefaultValidations(ValidatorResult result, Evento entity, string userName)
        {
            await base.DefaultValidations(result, entity, userName);

            if (await Service.TemOutroEventoNoMesmoDia(entity))
                result.AddError("Já existe um Evento nesse Dia!");
        }
    }
}
