﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using stela_api.src.Infrastructure.Data;

#nullable disable

namespace stela_api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240612024605_portfolio")]
    partial class portfolio
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("stela_api.src.Domain.Models.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ConfirmationCode")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ConfirmationCodeValidBefore")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<bool>("IsEmailVerified")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsPhoneVerified")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastPasswordDateModified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<string>("RestoreCode")
                        .HasColumnType("text");

                    b.Property<DateTime?>("RestoreCodeValidBefore")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.Property<DateTime?>("TokenValidBefore")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("WasPasswordResetRequest")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("stela_api.src.Domain.Models.AdditionalService", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("AdditionalServices");
                });

            modelBuilder.Entity("stela_api.src.Domain.Models.Busket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("Buskets");
                });

            modelBuilder.Entity("stela_api.src.Domain.Models.BusketItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BusketId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("MemorialId")
                        .HasColumnType("uuid");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("BusketId");

                    b.HasIndex("MemorialId");

                    b.ToTable("BusketItems");
                });

            modelBuilder.Entity("stela_api.src.Domain.Models.Memorial", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Images")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Memorials");
                });

            modelBuilder.Entity("stela_api.src.Domain.Models.MemorialMaterial", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Hex")
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("stela_api.src.Domain.Models.MemorialMaterials", b =>
                {
                    b.Property<Guid>("MemorialId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MaterialId")
                        .HasColumnType("uuid");

                    b.HasKey("MemorialId", "MaterialId");

                    b.HasIndex("MaterialId");

                    b.ToTable("MemorialMaterials");
                });

            modelBuilder.Entity("stela_api.src.Domain.Models.PortfolioMemorial", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CemeteryName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("PortfolioMemorials");
                });

            modelBuilder.Entity("stela_api.src.Domain.Models.PortfolioMemorialMaterials", b =>
                {
                    b.Property<Guid>("MemorialMaterialId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PortfolioMemorialId")
                        .HasColumnType("uuid");

                    b.HasKey("MemorialMaterialId", "PortfolioMemorialId");

                    b.HasIndex("PortfolioMemorialId");

                    b.ToTable("PortfolioMemorialMaterials");
                });

            modelBuilder.Entity("stela_api.src.Domain.Models.UnconfirmedAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ConfirmationCode")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ConfirmationCodeValidBefore")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("UnconfirmedAccounts");
                });

            modelBuilder.Entity("stela_api.src.Domain.Models.Busket", b =>
                {
                    b.HasOne("stela_api.src.Domain.Models.Account", "Account")
                        .WithOne("Busket")
                        .HasForeignKey("stela_api.src.Domain.Models.Busket", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("stela_api.src.Domain.Models.BusketItem", b =>
                {
                    b.HasOne("stela_api.src.Domain.Models.Busket", "Busket")
                        .WithMany("BusketItems")
                        .HasForeignKey("BusketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("stela_api.src.Domain.Models.Memorial", "Memorial")
                        .WithMany("BusketItems")
                        .HasForeignKey("MemorialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Busket");

                    b.Navigation("Memorial");
                });

            modelBuilder.Entity("stela_api.src.Domain.Models.MemorialMaterials", b =>
                {
                    b.HasOne("stela_api.src.Domain.Models.MemorialMaterial", "Material")
                        .WithMany("Memorials")
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("stela_api.src.Domain.Models.Memorial", "Memorial")
                        .WithMany("Materials")
                        .HasForeignKey("MemorialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Material");

                    b.Navigation("Memorial");
                });

            modelBuilder.Entity("stela_api.src.Domain.Models.PortfolioMemorialMaterials", b =>
                {
                    b.HasOne("stela_api.src.Domain.Models.MemorialMaterial", "Material")
                        .WithMany("PortfolioMemorialMaterials")
                        .HasForeignKey("MemorialMaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("stela_api.src.Domain.Models.PortfolioMemorial", "PortfolioMemorial")
                        .WithMany("Materials")
                        .HasForeignKey("PortfolioMemorialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Material");

                    b.Navigation("PortfolioMemorial");
                });

            modelBuilder.Entity("stela_api.src.Domain.Models.Account", b =>
                {
                    b.Navigation("Busket")
                        .IsRequired();
                });

            modelBuilder.Entity("stela_api.src.Domain.Models.Busket", b =>
                {
                    b.Navigation("BusketItems");
                });

            modelBuilder.Entity("stela_api.src.Domain.Models.Memorial", b =>
                {
                    b.Navigation("BusketItems");

                    b.Navigation("Materials");
                });

            modelBuilder.Entity("stela_api.src.Domain.Models.MemorialMaterial", b =>
                {
                    b.Navigation("Memorials");

                    b.Navigation("PortfolioMemorialMaterials");
                });

            modelBuilder.Entity("stela_api.src.Domain.Models.PortfolioMemorial", b =>
                {
                    b.Navigation("Materials");
                });
#pragma warning restore 612, 618
        }
    }
}
