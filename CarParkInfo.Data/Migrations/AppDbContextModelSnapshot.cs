﻿// <auto-generated />
using System;
using CarParkInfo.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CarParkInfo.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.1");

            modelBuilder.Entity("CarParkInfo.Core.Models.CarPark", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CarParkBasement")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("CarParkDecks")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CarParkNo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CarParkType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FreeParking")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<float>("GantryHeight")
                        .HasColumnType("REAL");

                    b.Property<string>("NightParking")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ShortTermParking")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TypeOfParkingSystem")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<float>("XCoord")
                        .HasColumnType("REAL");

                    b.Property<float>("YCoord")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("CarParkNo")
                        .IsUnique();

                    b.HasIndex("FreeParking");

                    b.HasIndex("GantryHeight");

                    b.HasIndex("NightParking");

                    b.ToTable("CarParks");
                });

            modelBuilder.Entity("CarParkInfo.Core.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastLoginAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CarParkInfo.Core.Models.UserFavorite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CarParkNo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CarParkNo");

                    b.HasIndex("UserId", "CarParkNo")
                        .IsUnique();

                    b.ToTable("UserFavorites");
                });

            modelBuilder.Entity("CarParkInfo.Core.Models.UserFavorite", b =>
                {
                    b.HasOne("CarParkInfo.Core.Models.CarPark", "CarPark")
                        .WithMany()
                        .HasForeignKey("CarParkNo")
                        .HasPrincipalKey("CarParkNo")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarParkInfo.Core.Models.User", "User")
                        .WithMany("Favorites")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CarPark");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CarParkInfo.Core.Models.User", b =>
                {
                    b.Navigation("Favorites");
                });
#pragma warning restore 612, 618
        }
    }
}
