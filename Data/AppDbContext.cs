using Microsoft.EntityFrameworkCore;
using LanceCertoSQL.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Imovel> Imoveis { get; set; }
    public DbSet<Pregao> Pregoes { get; set; }
    public DbSet<Lance> Lances { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Pregao>()
            .HasOne(p => p.Usuario)
            .WithMany()
            .HasForeignKey(p => p.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Imovel>()
            .HasOne(i => i.Usuario)
            .WithMany(u => u.Imoveis)
            .HasForeignKey(i => i.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        //Para o Lance
        modelBuilder.Entity<Lance>()
            .HasOne(l => l.Usuario)
            .WithMany()
            .HasForeignKey(l => l.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Lance>()
            .HasOne(l => l.Pregao)
            .WithMany(p => p.Lances)
            .HasForeignKey(l => l.PregaoId)
            .OnDelete(DeleteBehavior.Restrict);

        //Casas decimais e problemas com vírgulas
        modelBuilder.Entity<Pregao>()
            .Property(p => p.ValorMinimo)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Imovel>()
            .Property(i => i.ValorEstimado)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Lance>()
            .Property(l => l.Valor)
            .HasColumnType("decimal(18,2)");

    }

}

