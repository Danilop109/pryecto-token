using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");

            builder.Property(n => n.UserName)
            .HasColumnName("UserName")// sirve para cambiar el nombre en la base de datos
            .HasColumnType("varchar")
            .IsRequired()
            .HasMaxLength(100);

            builder.Property(e => e.Email)
            .HasColumnName("Email")
            .HasColumnType("varchar")
            .IsRequired()
            .HasMaxLength(100);

            builder.Property(e => e.Password)
            .HasColumnName("Password")
            .HasColumnType("varchar")
            .IsRequired()
            .HasMaxLength(100);

            builder
            .HasMany(r => r.Rols)
            .WithMany(u => u.Users)
            .UsingEntity<UserRol>(

                j => j
                .HasOne(p => p.Rol)
                .WithMany(u => u.UsersRols)
                .HasForeignKey(u => u.RolIdFk),

                j => j
                .HasOne(p => p.User)
                .WithMany(u => u.UsersRols)
                .HasForeignKey(u => u.UserIdFk),

                j => 
                {
                    j.ToTable("UserRol");
                    j.HasKey(t => new { t.UserIdFk, t.RolIdFk});
                }
            );
        }
    }
}