using Kernel.Domain.Model.Dtos;
using Kernel.Domain.Model.Providers;
using System;
using System.Threading.Tasks;

namespace Kernel.Infra.Mock
{
    public class MockUserProvider : IUserProvider
    {
        public async Task<Token> GetToken()
        {
            var token = new Token
            {
                Key = 1, // Representa o Colega Logado
                Email = "user@test.com.br",
                Name = "Test User"
            };

            return await Task.FromResult(token);
        }

        public Task RemoveToken()
        {
            throw new NotImplementedException();
        }

        public Task SaveTokenString(string tokenString)
        {
            throw new NotImplementedException();
        }
    }
}
