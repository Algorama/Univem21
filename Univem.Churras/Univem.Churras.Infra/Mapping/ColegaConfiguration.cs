using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Univem.Churras.Domain.Model.Entities;

namespace Univem.Churras.Infra.Mapping
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