using System.Collections.Generic;
using InvestmentControlApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvestmentControlApi.Infrastructure.Data
{
    public class InvestmentDbContext : DbContext
    {
        public InvestmentDbContext(DbContextOptions<InvestmentDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Ativo> Ativos { get; set; }
        public DbSet<Operacao> Operacoes { get; set; }
        public DbSet<Cotacao> Cotacoes { get; set; }
        public DbSet<Posicao> Posicoes { get; set; } // <-- adicionado

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Operacoes)
                .WithOne(o => o.Usuario)
                .HasForeignKey(o => o.UsuarioId);

            modelBuilder.Entity<Ativo>()
                .HasMany(a => a.Operacoes)
                .WithOne(o => o.Ativo)
                .HasForeignKey(o => o.AtivoId);

            modelBuilder.Entity<Ativo>()
                .HasMany(a => a.Cotacoes)
                .WithOne(c => c.Ativo)
                .HasForeignKey(c => c.AtivoId);

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Posicoes)
                .WithOne(p => p.Usuario)
                .HasForeignKey(p => p.UsuarioId);

            modelBuilder.Entity<Ativo>()
                .HasMany(a => a.Posicoes)
                .WithOne(p => p.Ativo)
                .HasForeignKey(p => p.AtivoId);
        }
    }
}