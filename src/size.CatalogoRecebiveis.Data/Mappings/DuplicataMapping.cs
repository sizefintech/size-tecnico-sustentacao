using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using size.CatalogoRecebiveis.Business.AggregateRoots;

namespace size.CatalogoRecebiveis.Data.Mappings
{
    public class DuplicataMapping : IEntityTypeConfiguration<Duplicata>
    {
        public void Configure(EntityTypeBuilder<Duplicata> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.TomadorId)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(d => d.Numero)
                .IsRequired()
                .HasColumnType("varchar(50)");

            builder.Property(d => d.Valor)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(d => d.ValorLiquido)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(d => d.DataVencimento)
                .IsRequired();

            builder.Property(d => d.Status)
                .IsRequired();

            builder.Property(d => d.NoCarrinho)
                .IsRequired();

            builder.ToTable("Duplicatas");
        }
    }
}
