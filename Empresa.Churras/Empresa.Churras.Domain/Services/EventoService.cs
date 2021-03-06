using Empresa.Churras.Domain.Model.Entities;
using Empresa.Churras.Domain.Validators;
using Kernel.Domain.Model.Enums;
using Kernel.Domain.Model.Providers;
using Kernel.Domain.Model.Validation;
using Kernel.Domain.Repositories;
using Kernel.Domain.Services;
using System;
using System.Threading.Tasks;

namespace Empresa.Churras.Domain.Services
{
    public class EventoService : CrudService<Evento>
    {
        public EventoService(
            ISessionFactory sessionFactory, 
            IUserProvider userProvider,
            EventoValidator validator) : base(sessionFactory, userProvider, validator)
        {
            validator.Service = this;
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

        public async override Task<Evento> Get(object key)
        {
            using (var session = SessionFactory.OpenSession())
            {
                var repo = session.GetRepository<Evento>();
                var repoColega = session.GetQueryRepository<Colega>();

                var entity = await repo.Get(key);
                if(entity != null)
                    entity.DonoDaCasa = await repoColega.Get(entity.DonoDaCasaKey);

                return entity;
            }
        }

        public async Task<bool> TemOutroEventoNoMesmoDia(Evento entity)
        {
            var outroEventoNoDia = await Get(x => x.Key != entity.Key && x.Dia == entity.Dia);
            return outroEventoNoDia != null;
        }

        public async Task ConfirmarPresenca(long eventoKey, string vaiLevar)
        {
            var token = await GetToken();

            using (var session = SessionFactory.OpenSession())
            {
                var repo = session.GetRepository<Evento>();
                var repoColega = session.GetQueryRepository<Colega>();

                var evento = await repo.Get(eventoKey);
                var colegaConfirmando = await repoColega.Get(token.Key);

                evento.ConfirmarPresenca(colegaConfirmando, vaiLevar);

                repo.Update(evento);
                session.SaveChanges();
            }
        }

        public async Task CancelarPresenca(long eventoKey)
        {
            var token = await GetToken();

            using (var session = SessionFactory.OpenSession())
            {
                var repo = session.GetRepository<Evento>();
                var repoColega = session.GetQueryRepository<Colega>();

                var evento = await repo.Get(eventoKey);
                var colegaConfirmado = await repoColega.Get(token.Key);

                evento.CancelarPresenca(colegaConfirmado);

                repo.Update(evento);
                session.SaveChanges();
            }
        }
    }
}
