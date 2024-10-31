using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisionaryAI.API.Models;

namespace VisionaryAI.API.Mappings
{
    public class EmpresaMapping : IEntityTypeConfiguration<Empresa>
    {
        public void Configure(EntityTypeBuilder<Empresa> builder)
        {
            builder.ToTable("tb_empresas");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Cnpj)
                   .HasColumnName("cnpj")
                   .HasMaxLength(14)
                   .IsRequired();

            builder.Property(x => x.Nome)
                   .HasColumnName("nome")
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.Email)
                   .HasColumnName("e_mail")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(x => x.Descricao)
                   .HasColumnName("descricao")
                   .HasMaxLength(50)
                   .IsRequired();

            
  
        }
    }
}
