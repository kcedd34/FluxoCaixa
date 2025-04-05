﻿// <auto-generated />
using System;
using FluxoCaixa.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FluxoCaixa.Infrastructure.Migrations
{
    [DbContext(typeof(FluxoCaixaDbContext))]
    partial class FluxoCaixaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FluxoCaixa.Domain.Entities.ConsolidadoDiario", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Data")
                        .HasColumnType("date");

                    b.Property<decimal>("SaldoAcumulado")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalCreditos")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalDebitos")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("UltimaAtualizacao")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Data")
                        .IsUnique();

                    b.ToTable("Consolidados");
                });

            modelBuilder.Entity("FluxoCaixa.Domain.Entities.Lancamento", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("AtualizadoEm")
                        .HasColumnType("datetime2");

                    b.Property<string>("Categoria")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Data")
                        .HasColumnType("date");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Lancamentos");
                });
#pragma warning restore 612, 618
        }
    }
}
