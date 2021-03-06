using Kernel.Domain.Model.Dtos;
using Kernel.Domain.Model.Settings;
using Kernel.Domain.Services;
using System;

namespace Kernel.Infra.Mock
{
    public class MockEmailService : IEmailService
    {
        public void Send(EmailMessage message, SmtpSettings settings = null)
        {
            Console.WriteLine($"Email Enviado: {message.Subject} - Para:");
            foreach(var to in message.To)
                Console.WriteLine(to);
        }
    }
}