using Microsoft.EntityFrameworkCore;
using VehicleManagement.Domain.Entities;

namespace VehicleManagement.Infra.Context;

public class VehicleManagementDbContext : DbContext
{
    public VehicleManagementDbContext(DbContextOptions<VehicleManagementDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Veiculo> Veiculos => Set<Veiculo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(usuario => usuario.Id);
            entity.Property(usuario => usuario.Nome).IsRequired();
            entity.Property(usuario => usuario.Login).IsRequired();
            entity.Property(usuario => usuario.SenhaHash).IsRequired();
        });

        modelBuilder.Entity<Veiculo>(entity =>
        {
            entity.HasKey(veiculo => veiculo.Id);
            entity.Property(veiculo => veiculo.Descricao).IsRequired();
            entity.Property(veiculo => veiculo.Marca).IsRequired();
            entity.Property(veiculo => veiculo.Modelo).IsRequired();
            entity.Property(veiculo => veiculo.Opcionais).IsRequired(false);
            entity.Property(veiculo => veiculo.Valor).IsRequired(false);
        });
    }
}
