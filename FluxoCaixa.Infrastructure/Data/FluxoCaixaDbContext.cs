using FluxoCaixa.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FluxoCaixa.Infrastructure.Data
{
    public class FluxoCaixaDbContext : DbContext
    {
        public DbSet<Lancamento> Lancamentos { get; set; }
        public DbSet<ConsolidadoDiario> Consolidados { get; set; }

        public FluxoCaixaDbContext(DbContextOptions<FluxoCaixaDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração para Lancamento
            modelBuilder.Entity<Lancamento>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Descricao).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Valor).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Categoria).HasMaxLength(50);
                entity.Property(e => e.Data).HasColumnType("date");
            });

            // Configuração para ConsolidadoDiario
            modelBuilder.Entity<ConsolidadoDiario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TotalCreditos).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalDebitos).HasColumnType("decimal(18,2)");
                entity.Property(e => e.SaldoAcumulado).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Data).HasColumnType("date");
                entity.HasIndex(e => e.Data).IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}