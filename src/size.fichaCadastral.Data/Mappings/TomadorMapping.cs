using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using size.FichaCadastral.Business.AggregateRoots;

namespace size.fichaCadastral.Data.Mappings
{
    public class TomadorMapping : IEntityTypeConfiguration<Tomador>
    {
        public void Configure(EntityTypeBuilder<Tomador> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.RazaoSocial)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.OwnsOne(t => t.Documento, doc =>
            {
                doc.Property(d => d.Numero)
                    .IsRequired()
                    .HasColumnName("Numero")
                    .HasColumnType("varchar(20)");

                doc.Property(d => d.Tipo)
                    .IsRequired()
                    .HasColumnName("Tipo");
            });

            builder.ToTable("Tomadores");
        }
    }
}
