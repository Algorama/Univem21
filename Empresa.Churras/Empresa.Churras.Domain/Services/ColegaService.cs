using Empresa.Churras.Domain.Model.Entities;
using Kernel.Domain.Model.Providers;
using Kernel.Domain.Repositories;
using Kernel.Domain.Services;
using Kernel.Domain.Validation;

namespace Empresa.Churras.Domain.Services
{
    public class ColegaService : CrudService<Colega>
    {
        public ColegaService(
            ISessionFactory sessionFactory, 
            IUserProvider userProvider, 
            Validator<Colega> validator) : base(sessionFactory, userProvider, validator)
        {
        }
    }
}
