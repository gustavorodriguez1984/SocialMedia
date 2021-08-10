using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SocialMediaCore.Entities;
using SocialMediaInfrastructure.Data.Configurations;

namespace SocialMediaInfrastructure.Data
{
    public partial class SocialMediaContext : DbContext
    {
        public SocialMediaContext()
        {
        }

        public SocialMediaContext(DbContextOptions<SocialMediaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Security> Securities { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Primera forma para agregar la configuracion de cada una de las entidades 
            //modelBuilder.ApplyConfiguration(new CommentConfiguration());

            //modelBuilder.ApplyConfiguration(new PostConfiguration());


            //modelBuilder.ApplyConfiguration(new UserConfiguration());
            //modelBuilder.ApplyConfiguration(new SecurityConfiguration());

            //Segunda forma  de agregar al context la configuracion de las entidades mas
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());




        }

        
    }
}
