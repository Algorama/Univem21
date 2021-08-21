using Kernel.Domain.Model.Dtos;
using Kernel.Domain.Model.Helpers;
using Kernel.Domain.Model.Providers;
using Kernel.Domain.Model.Settings;
using Kernel.Domain.Model.Validation;
using Kernel.Domain.Repositories;
using Kernel.Domain.Services;
using Kernel.Domain.Validation;
using System;
using System.Threading.Tasks;
using Univem.Churras.Domain.Model.Entities;

namespace Univem.Churras.Domain.Services
{
    public class ColegaService : CrudService<Colega>
    {
        private readonly AppSettings _appSettings;

        public ColegaService(
            ISessionFactory sessionFactory,
            IUserProvider userProvider,
            Validator<Colega> validator, 
            AppSettings appSettings) : base(sessionFactory, userProvider, validator)
        {
            _appSettings = appSettings;
        }

        public async override Task Insert(Colega entity)
        {
            entity.Senha = CryptoHelper.ComputeHashMd5(entity.Senha);
            await base.Insert(entity);
        }

        public async override Task Update(Colega entity)
        {
            var old = await Get(entity.Key);
            entity.Senha = old.Senha;
            await base.Update(entity);
        }

        public async Task<Token> Login(LoginRequest loginRequest)
        {
            var validatorResult = new ValidatorResult();
            validatorResult.ValidateAnnotations(loginRequest);
            if (validatorResult.HasErrors)
                throw new ValidatorException(validatorResult.Errors);

            var senhaHash = CryptoHelper.ComputeHashMd5(loginRequest.Senha);
            var colega = await Get(x => x.Email == loginRequest.Email && x.Senha == senhaHash);
            if(colega == null)
                throw new ValidatorException("Login Inválido!");

            var token = new Token
            {
                Key = colega.Key,
                Name = colega.Nome,
                Email = colega.Email,
                ExpiresIn = DateTime.Now.AddHours(_appSettings.TokenSettings.TokenExpirationInHours)
            };

            return token;
        }

        public async Task TrocaSenha(TrocaSenhaRequest trocaSenhaRequest)
        {
            var validatorResult = new ValidatorResult();
            validatorResult.ValidateAnnotations(trocaSenhaRequest);
            if (validatorResult.HasErrors)
                throw new ValidatorException(validatorResult.Errors);

            if (trocaSenhaRequest.SenhaNova != trocaSenhaRequest.SenhaNovaConfirma)
                throw new ValidatorException("Senhas Novas não batem!");

            var senhaHash = CryptoHelper.ComputeHashMd5(trocaSenhaRequest.SenhaAntiga);
            var colega = await Get(x => x.Email == trocaSenhaRequest.Email && x.Senha == senhaHash);
            if (colega == null)
                throw new ValidatorException("Login Inválido!");

            using(var session = SessionFactory.OpenSession())
            {
                var repo = session.GetRepository<Colega>();
                colega.Senha = CryptoHelper.ComputeHashMd5(trocaSenhaRequest.SenhaNova);
                repo.Update(colega);
                session.SaveChanges();
            }
        }
    }
}
