using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace size.Operacao.Data.Mappings
{
    public class OperacaoMapping : IEntityTypeConfiguration<Business.AggregateRoots.Operacao>
    {
        public void Configure(EntityTypeBuilder<Business.AggregateRoots.Operacao> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.TomadorId)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(o => o.Codigo)
                .IsRequired()
                .HasColumnType("varchar(200)")
                 .HasDefaultValueSql("CAST(NEXT VALUE FOR dbo.CodigoSequence AS VARCHAR(20))");

            builder.Property(o => o.ValorBruto)
                .IsRequired()
                .HasColumnType("decimal(12,2)");

            builder.Property(o => o.ValorLiquido)
                .IsRequired()
                .HasColumnType("decimal(12,2)");

            builder.Property(o => o.TaxaAntecipacao)
                .IsRequired()
                .HasColumnType("decimal(12,2)");

            builder.Property(o => o.Prazo)
                .IsRequired();

            builder.Property(o => o.DataCriacao)
                .IsRequired();

            builder.Property(o => o.DataProcessamento)
                .IsRequired(false);

            builder.HasMany(o => o.Duplicatas)
                .WithOne(d => d.Operacao)
                .HasForeignKey(d => d.OperacaoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Operacoes");
        }
    }
}
