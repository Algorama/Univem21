using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Univem.Churras.Domain.Model.Entities;

namespace Univem.Churras.Infra.Mapping
{
    public class EventoConfiguration : IEntityTypeConfiguration<Evento>
    {
        public void Configure(EntityTypeBuilder<Evento> builder)
        {
            builder.ToContainer("Churras");
            builder.HasPartitionKey(x => x.Discriminator);
            builder.HasOne(x => x.DonoDaCasa).WithMany();
            builder.OwnsOne(x => x.Periodo);
            builder.OwnsMany(x => x.ColegasConfirmados);
        }
    }
}