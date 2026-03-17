using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using size.Carrinho.Business.Entities;

namespace size.Carrinho.Data.Mappings
{
    public class DuplicataMapping : IEntityTypeConfiguration<Duplicata>
    {
        public void Configure(EntityTypeBuilder<Duplicata> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Id)
                .HasColumnType("varchar(200)");

            builder.Property(d => d.CarrinhoId)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.HasOne(d => d.Carrinho)
                .WithMany(c => c.Duplicatas)
                .HasForeignKey(d => d.CarrinhoId);

            builder.ToTable("Duplicatas");
        }
    }
}
