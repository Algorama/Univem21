using Empresa.Churras.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Empresa.Churras.Infra.Mapping
{
    public class ColegaConfiguration : IEntityTypeConfiguration<Colega>
    {
        public void Configure(EntityTypeBuilder<Colega> builder)
        {
            builder.ToContainer("Churras");
            builder.HasPartitionKey(x => x.Discriminator);
            builder.OwnsOne(x => x.Endereco);
        }
    }
}