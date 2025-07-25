﻿// <auto-generated />
using System;
using InvestmentControlApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace InvestmentControlApi.Migrations
{
    [DbContext(typeof(InvestmentDbContext))]
    partial class InvestmentDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("InvestmentControlApi.Domain.Entities.Ativo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Codigo")
                        .HasColumnType("varchar(10)")
                        .HasColumnName("codigo");

                    b.Property<string>("Nome")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("nome");

                    b.HasKey("Id");

                    b.ToTable("ativos");
                });

            modelBuilder.Entity("InvestmentControlApi.Domain.Entities.Cotacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AtivoId")
                        .HasColumnType("int")
                        .HasColumnName("ativo_id");

                    b.Property<DateTime>("DataHora")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("data_hora");

                    b.Property<decimal?>("EPS")
                        .HasColumnType("decimal(18,4)")
                        .HasColumnName("eps");

                    b.Property<decimal?>("PE")
                        .HasColumnType("decimal(18,4)")
                        .HasColumnName("pe");

                    b.Property<decimal>("PrecoAbertura")
                        .HasColumnType("decimal(18,4)")
                        .HasColumnName("preco_abertura");

                    b.Property<decimal>("PrecoMaximoDia")
                        .HasColumnType("decimal(18,4)")
                        .HasColumnName("preco_maximo_dia");

                    b.Property<decimal>("PrecoMinimoDia")
                        .HasColumnType("decimal(18,4)")
                        .HasColumnName("preco_minimo_dia");

                    b.Property<decimal>("PrecoUnitario")
                        .HasColumnType("decimal(18,4)")
                        .HasColumnName("preco_unitario");

                    b.Property<long>("Volume")
                        .HasColumnType("bigint")
                        .HasColumnName("volume");

                    b.HasKey("Id");

                    b.HasIndex("AtivoId");

                    b.ToTable("cotacoes");
                });

            modelBuilder.Entity("InvestmentControlApi.Domain.Entities.Operacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AtivoId")
                        .HasColumnType("int")
                        .HasColumnName("ativo_id");

                    b.Property<decimal>("Corretagem")
                        .HasColumnType("decimal(18,4)")
                        .HasColumnName("corretagem");

                    b.Property<DateTime>("DataHora")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("data_hora");

                    b.Property<decimal>("PrecoUnitario")
                        .HasColumnType("decimal(18,4)")
                        .HasColumnName("preco_unitario");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int")
                        .HasColumnName("quantidade");

                    b.Property<string>("TipoOperacao")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasColumnName("tipo_operacao");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int")
                        .HasColumnName("usuario_id");

                    b.HasKey("Id");

                    b.HasIndex("AtivoId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("operacoes");
                });

            modelBuilder.Entity("InvestmentControlApi.Domain.Entities.Posicao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AtivoId")
                        .HasColumnType("int")
                        .HasColumnName("ativo_id");

                    b.Property<decimal>("PL")
                        .HasColumnType("decimal(18,4)")
                        .HasColumnName("p_l");

                    b.Property<decimal>("PrecoAtual")
                        .HasColumnType("decimal(18,4)")
                        .HasColumnName("preco_atual");

                    b.Property<decimal>("PrecoMedio")
                        .HasColumnType("decimal(18,4)")
                        .HasColumnName("preco_medio");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int")
                        .HasColumnName("quantidade");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int")
                        .HasColumnName("usuario_id");

                    b.HasKey("Id");

                    b.HasIndex("AtivoId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("posicoes");
                });

            modelBuilder.Entity("InvestmentControlApi.Domain.Entities.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("email");

                    b.Property<string>("Nome")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("nome");

                    b.Property<decimal>("PercentualCorretagem")
                        .HasColumnType("decimal(5,2)")
                        .HasColumnName("corretagem");

                    b.HasKey("Id");

                    b.ToTable("usuarios");
                });

            modelBuilder.Entity("InvestmentControlApi.Domain.Entities.Cotacao", b =>
                {
                    b.HasOne("InvestmentControlApi.Domain.Entities.Ativo", "Ativo")
                        .WithMany("Cotacoes")
                        .HasForeignKey("AtivoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ativo");
                });

            modelBuilder.Entity("InvestmentControlApi.Domain.Entities.Operacao", b =>
                {
                    b.HasOne("InvestmentControlApi.Domain.Entities.Ativo", "Ativo")
                        .WithMany("Operacoes")
                        .HasForeignKey("AtivoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InvestmentControlApi.Domain.Entities.Usuario", "Usuario")
                        .WithMany("Operacoes")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ativo");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("InvestmentControlApi.Domain.Entities.Posicao", b =>
                {
                    b.HasOne("InvestmentControlApi.Domain.Entities.Ativo", "Ativo")
                        .WithMany("Posicoes")
                        .HasForeignKey("AtivoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InvestmentControlApi.Domain.Entities.Usuario", "Usuario")
                        .WithMany("Posicoes")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ativo");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("InvestmentControlApi.Domain.Entities.Ativo", b =>
                {
                    b.Navigation("Cotacoes");

                    b.Navigation("Operacoes");

                    b.Navigation("Posicoes");
                });

            modelBuilder.Entity("InvestmentControlApi.Domain.Entities.Usuario", b =>
                {
                    b.Navigation("Operacoes");

                    b.Navigation("Posicoes");
                });
#pragma warning restore 612, 618
        }
    }
}
