using Kernel.Domain.Model.Enums;
using Kernel.Domain.Model.Providers;
using Kernel.Domain.Repositories;
using Kernel.Domain.Services;
using System;
using System.Threading.Tasks;
using Univem.Churras.Domain.Model.Entities;
using Univem.Churras.Domain.Validators;

namespace Univem.Churras.Domain.Services
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
                var repoColega = session.GetRepository<Colega>();

                entity.Key = await GetNextSequence(session);

                entity.DonoDaCasa = await repoColega.Get(token.Key);

                await repo.Insert(entity);
                session.SaveChanges();
            }
        }

        internal async Task<bool> TemOutroEventoNoMesmoDia(Evento entity)
        {
            var outroEventoNoDia = await Get(x => x.Key != entity.Key && x.Dia == entity.Dia);
            return outroEventoNoDia != null;
        }

        public async Task ConfirmarPresenca(long eventoKey, string vaiLevar)
        {
            var token = await GetToken();

            using(var session = SessionFactory.OpenSession())
            {
                var repoEvento = session.GetRepository<Evento>();
                var repoColega = session.GetQueryRepository<Colega>();

                var evento = await repoEvento.Get(eventoKey);
                var colegaConfirmando = await repoColega.Get(token.Key);

                evento.ConfirmarPresenca(colegaConfirmando, vaiLevar);

                repoEvento.Update(evento);
                session.SaveChanges();
            }
        }

        public async Task CancelarPresenca(long eventoKey)
        {
            var token = await GetToken();

            using (var session = SessionFactory.OpenSession())
            {
                var repoEvento = session.GetRepository<Evento>();
                var repoColega = session.GetQueryRepository<Colega>();

                var evento = await repoEvento.Get(eventoKey);
                var colegaCancelando = await repoColega.Get(token.Key);

                evento.CancelarPresenca(colegaCancelando);

                repoEvento.Update(evento);
                session.SaveChanges();
            }
        }

    }
}
