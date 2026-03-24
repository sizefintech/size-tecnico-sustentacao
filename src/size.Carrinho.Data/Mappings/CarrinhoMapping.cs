using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace size.Carrinho.Data.Mappings
{
    public class CarrinhoMapping : IEntityTypeConfiguration<Business.AggregateRoots.Carrinho>
    {
        public void Configure(EntityTypeBuilder<Business.AggregateRoots.Carrinho> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.AgregateId)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(c => c.InicioProcessamento)
                .IsRequired(false);

            builder.HasMany(c => c.Duplicatas)
                .WithOne(d => d.Carrinho)
                .HasForeignKey(d => d.CarrinhoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Carrinhos");
        }
    }
}
