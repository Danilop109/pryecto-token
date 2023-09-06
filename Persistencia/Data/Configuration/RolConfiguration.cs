using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration
{
    public class RolConfiguration : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.ToTable("Rol");

            builder.Property(n => n.Nombre)
            .HasColumnName("RolName")// sirve para cambiar el nombre en la base de datos
            .HasColumnType("varchar")
            .IsRequired()
            .HasMaxLength(100);
        }
    }
}