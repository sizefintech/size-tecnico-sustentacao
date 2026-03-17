using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using size.Operacao.Business.Entities;

namespace size.Operacao.Data.Mappings
{
    public class DuplicataMapping : IEntityTypeConfiguration<Duplicata>
    {
        public void Configure(EntityTypeBuilder<Duplicata> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Numero)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(d => d.Vencimento)
                .IsRequired();

            builder.Property(d => d.Valor)
                .IsRequired()
                .HasColumnType("decimal(12,2)");

            builder.Property(d => d.OperacaoId)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.HasOne(d => d.Operacao)
                .WithMany(o => o.Duplicatas)
                .HasForeignKey(d => d.OperacaoId);

            builder.ToTable("Duplicatas");
        }
    }
}
