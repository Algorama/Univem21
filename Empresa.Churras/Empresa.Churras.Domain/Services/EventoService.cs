using Empresa.Churras.Domain.Model.Entities;
using Kernel.Domain.Model.Enums;
using Kernel.Domain.Model.Providers;
using Kernel.Domain.Repositories;
using Kernel.Domain.Services;
using Kernel.Domain.Validation;
using System.Threading.Tasks;

namespace Empresa.Churras.Domain.Services
{
    public class EventoService : CrudService<Evento>
    {
        public EventoService(
            ISessionFactory sessionFactory, 
            IUserProvider userProvider, 
            Validator<Evento> validator) : base(sessionFactory, userProvider, validator)
        {
        }

        public override async Task Insert(Evento entity)
        {
            var token = await GetToken();
            await Validate(entity, ValidationType.Insert, token.Email);

            using (var session = SessionFactory.OpenSession())
            {
                var repo = session.GetRepository<Evento>();
                var repoColega = session.GetQueryRepository<Colega>();

                entity.Key = await GetNextSequence(session);
                entity.DonoDaCasa = await repoColega.Get(token.Key);

                await repo.Insert(entity);
                session.SaveChanges();
            }
        }
    }
}
