namespace Sample.Functional.Migrations
{
    using FluentMigrator;

    [Migration(201712261133)]
    public class M201712261133_SchemaInicial : Migration
    {
        public override void Up()
        {
            Create.Table("Usuario").WithDescription("Tabela de usuários")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey("PK_Usuario")
                .WithColumn("Nome").AsString(256).NotNullable()
                .WithColumn("Idade").AsInt16().NotNullable();

            Create.Table("TipoDeTelefone").WithDescription("Tabela de tipos de telefones")
                .WithColumn("Id").AsInt32().PrimaryKey("PK_TipoDeTelefone")
                .WithColumn("Tipo").AsString().NotNullable();

            Create.Table("Telefone").WithDescription("Tabela de telefones")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey("PK_Telefone")
                .WithColumn("IdUsuario").AsInt32()
                    .ForeignKey("FK_Telefone_Usuario", "Usuario", "Id")
                .WithColumn("IdTipoDeTelefone").AsInt32().NotNullable()
                    .ForeignKey("FK_Telefone_TipoDeTelefone", "TipoDeTelefone", "Id")
                .WithColumn("DDD").AsInt16().NotNullable()
                .WithColumn("Numero").AsInt32().NotNullable();

            Insert.IntoTable("TipoDeTelefone")
                .Row(new { Id = 1, Tipo = "Residencial" })
                .Row(new { Id = 2, Tipo = "Celular" })
                .Row(new { Id = 3, Tipo = "Comercial" });
        }

        public override void Down()
        {
            Delete.FromTable("TipoDeTelefone");
            Delete.ForeignKey("FK_Telefone_TipoDeTelefone");
            Delete.ForeignKey("FK_Telefone_Usuario");
            Delete.PrimaryKey("PK_Telefone");
            Delete.PrimaryKey("PK_TipoDeTelefone");
            Delete.PrimaryKey("PK_Usuario");
        }
    }
}
