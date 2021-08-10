using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaCore.Entities;
using SocialMediaCore.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaInfrastructure.Data.Configurations
{
    public class SecurityConfiguration : IEntityTypeConfiguration<Security>
    {
        public void Configure(EntityTypeBuilder<Security> builder)
        {
            builder.ToTable("Seguridad");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
           .HasColumnName("IdSeguridad");

           

            builder.Property(e => e.User)
            .HasColumnName("Usuario")//este es el campo de la tabla en la BD  con la que quiero mapear
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.UserName)
            .HasColumnName("NombreUsuario")//este es el campo de la tabla en la BD  con la que quiero mapear
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.Password)
            .HasColumnName("Contrasena")//este es el campo de la tabla en la BD  con la que quiero mapear
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.Role)
                .HasColumnName("Rol")//este es el campo de la tabla en la BD  con la que quiero mapear
                .IsRequired()
                .HasMaxLength(15)
                .HasConversion(x => x.ToString(),
                x => (RoleType)Enum.Parse(typeof(RoleType), x)
                );

        }
    }
}
