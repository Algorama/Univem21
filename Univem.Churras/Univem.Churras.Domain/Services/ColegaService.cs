using Kernel.Domain.Model.Providers;
using Kernel.Domain.Repositories;
using Kernel.Domain.Services;
using Kernel.Domain.Validation;
using Univem.Churras.Domain.Model.Entities;

namespace Univem.Churras.Domain.Services
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
