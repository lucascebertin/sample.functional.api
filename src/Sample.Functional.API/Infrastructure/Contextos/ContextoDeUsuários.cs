namespace Sample.Functional.API.Infrastructure.Contextos
{
    using Extensions;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ContextoDeUsuários : DbContext
    {
        public ContextoDeUsuários(
            DbContextOptions options)
            : base(options) { }

        public DbSet<Usuário> Usuários { get; set; }
        public DbSet<Telefone> Telefones { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        (
            modelBuilder.Entity<Telefone>(p =>
            {
                p.HasKey(x => x.Id).HasName("Id");
                p.Property(x => x.Tipo).HasColumnName("IdTipoDeTelefone");
                p.Property(x => x.IdUsuario).HasColumnName("IdUsuario").IsRequired(false);
                p.HasOne(x => x.Usuário);
                p.ToTable("Telefone");
            }),
            modelBuilder.Entity<Usuário>(p =>
            {
                p.HasKey(x => x.Id).HasName("Id");
                p.HasMany(x => x.Telefones).WithOne(x => x.Usuário).HasForeignKey(x => x.IdUsuario);
                p.ToTable("Usuario");
            })
        ).Executar();
    }
}
