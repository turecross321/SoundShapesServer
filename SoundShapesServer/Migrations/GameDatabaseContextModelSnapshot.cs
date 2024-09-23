﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SoundShapesServer.Database;

#nullable disable

namespace SoundShapesServer.Migrations
{
    [DbContext(typeof(GameDatabaseContext))]
    partial class GameDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SoundShapesServer.Types.Database.DbCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)");

                    b.Property<int>("CodeType")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Codes");
                });

            modelBuilder.Entity("SoundShapesServer.Types.Database.DbIp", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Authorized")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("AuthorizedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasMaxLength(39)
                        .HasColumnType("character varying(39)");

                    b.Property<bool?>("OneTimeUse")
                        .HasColumnType("boolean");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Ips");
                });

            modelBuilder.Entity("SoundShapesServer.Types.Database.DbRefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("ExpiryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("SoundShapesServer.Types.Database.DbToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("ExpiryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool?>("GenuineNpTicket")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("IpId")
                        .HasColumnType("uuid");

                    b.Property<int?>("Platform")
                        .HasColumnType("integer");

                    b.Property<Guid?>("RefreshTokenId")
                        .HasColumnType("uuid");

                    b.Property<int>("TokenType")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("IpId");

                    b.HasIndex("RefreshTokenId");

                    b.HasIndex("UserId");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("SoundShapesServer.Types.Database.DbUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EmailAddress")
                        .HasMaxLength(320)
                        .HasColumnType("character varying(320)");

                    b.Property<bool>("FinishedRegistration")
                        .HasColumnType("boolean");

                    b.Property<bool>("IpAuthorization")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.Property<string>("PasswordBcrypt")
                        .HasMaxLength(60)
                        .HasColumnType("character varying(60)");

                    b.Property<bool>("PsnAuthorization")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("RegistrationExpiryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<bool>("RpcnAuthorization")
                        .HasColumnType("boolean");

                    b.Property<bool>("VerifiedEmail")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SoundShapesServer.Types.Database.DbCode", b =>
                {
                    b.HasOne("SoundShapesServer.Types.Database.DbUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SoundShapesServer.Types.Database.DbIp", b =>
                {
                    b.HasOne("SoundShapesServer.Types.Database.DbUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SoundShapesServer.Types.Database.DbRefreshToken", b =>
                {
                    b.HasOne("SoundShapesServer.Types.Database.DbUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SoundShapesServer.Types.Database.DbToken", b =>
                {
                    b.HasOne("SoundShapesServer.Types.Database.DbIp", "Ip")
                        .WithMany("Tokens")
                        .HasForeignKey("IpId");

                    b.HasOne("SoundShapesServer.Types.Database.DbRefreshToken", "RefreshToken")
                        .WithMany("Tokens")
                        .HasForeignKey("RefreshTokenId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SoundShapesServer.Types.Database.DbUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ip");

                    b.Navigation("RefreshToken");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SoundShapesServer.Types.Database.DbIp", b =>
                {
                    b.Navigation("Tokens");
                });

            modelBuilder.Entity("SoundShapesServer.Types.Database.DbRefreshToken", b =>
                {
                    b.Navigation("Tokens");
                });
#pragma warning restore 612, 618
        }
    }
}
