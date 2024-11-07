﻿// <auto-generated />
using System;
using HealthyPawsV2.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HealthyPawsV2.DAL.Migrations
{
    [DbContext(typeof(HPContext))]
    [Migration("20241107025304_typefileByte[]")]
    partial class typefileByte
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HealthyPawsV2.DAL.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AddressId"));

                    b.Property<string>("canton")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("district")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("province")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AddressId");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("HealthyPawsV2.DAL.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("addressId")
                        .HasColumnType("int");

                    b.Property<string>("canton")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("district")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("idNumber")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("idType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("lastConnection")
                        .HasColumnType("datetime2");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("phone1")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("phone2")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("phone3")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("province")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("status")
                        .HasColumnType("bit");

                    b.Property<string>("surnames")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("addressId");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("HealthyPawsV2.DAL.Appointment", b =>
                {
                    b.Property<int>("AppointmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AppointmentId"));

                    b.Property<string>("Additional")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("diagnostic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ownerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("petFileId")
                        .HasColumnType("int");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("vetId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("veterinarioId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("AppointmentId");

                    b.HasIndex("ownerId");

                    b.HasIndex("petFileId");

                    b.HasIndex("veterinarioId");

                    b.ToTable("Appointment");
                });

            modelBuilder.Entity("HealthyPawsV2.DAL.AppointmentInventory", b =>
                {
                    b.Property<int>("appointmentInventoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("appointmentInventoryId"));

                    b.Property<int>("appointmentId")
                        .HasColumnType("int");

                    b.Property<string>("dose")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("inventoryID")
                        .HasColumnType("int");

                    b.Property<string>("measuredose")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("status")
                        .HasColumnType("bit");

                    b.HasKey("appointmentInventoryId");

                    b.HasIndex("appointmentId");

                    b.HasIndex("inventoryID");

                    b.ToTable("AppointmentInventories");
                });

            modelBuilder.Entity("HealthyPawsV2.DAL.Contact", b =>
                {
                    b.Property<int>("contactId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("contactId"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("WhatsApp")
                        .HasColumnType("bit");

                    b.HasKey("contactId");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("HealthyPawsV2.DAL.Document", b =>
                {
                    b.Property<int>("documentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("documentId"));

                    b.Property<int>("AppointmentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("fileType")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("petFileId")
                        .HasColumnType("int");

                    b.Property<bool>("status")
                        .HasColumnType("bit");

                    b.HasKey("documentId");

                    b.HasIndex("AppointmentId");

                    b.HasIndex("petFileId");

                    b.ToTable("Document");
                });

            modelBuilder.Entity("HealthyPawsV2.DAL.Inventory", b =>
                {
                    b.Property<int>("inventoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("inventoryId"));

                    b.Property<bool>("State")
                        .HasColumnType("bit");

                    b.Property<int>("availableStock")
                        .HasColumnType("int");

                    b.Property<string>("brand")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("price")
                        .HasColumnType("float");

                    b.Property<string>("provider")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("providerInfo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("inventoryId");

                    b.ToTable("Inventories");
                });

            modelBuilder.Entity("HealthyPawsV2.DAL.LogReport", b =>
                {
                    b.Property<int>("LogReportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LogReportId"));

                    b.Property<string>("CreatorIdId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("creationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("creator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LogReportId");

                    b.HasIndex("CreatorIdId");

                    b.ToTable("LogReports");
                });

            modelBuilder.Entity("HealthyPawsV2.DAL.PetBreed", b =>
                {
                    b.Property<int>("petBreedId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("petBreedId"));

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("petTypeId")
                        .HasColumnType("int");

                    b.Property<bool>("status")
                        .HasColumnType("bit");

                    b.HasKey("petBreedId");

                    b.HasIndex("petTypeId");

                    b.ToTable("PetBreeds");
                });

            modelBuilder.Entity("HealthyPawsV2.DAL.PetFile", b =>
                {
                    b.Property<int>("petFileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("petFileId"));

                    b.Property<int>("age")
                        .HasColumnType("int");

                    b.Property<string>("castration")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("creationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("duenoId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("idNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("petBreedId")
                        .HasColumnType("int");

                    b.Property<int>("petTypeId")
                        .HasColumnType("int");

                    b.Property<bool>("status")
                        .HasColumnType("bit");

                    b.Property<string>("vaccineHistory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("weight")
                        .HasColumnType("float");

                    b.HasKey("petFileId");

                    b.HasIndex("duenoId");

                    b.HasIndex("petBreedId");

                    b.ToTable("PetFile");
                });

            modelBuilder.Entity("HealthyPawsV2.DAL.PetType", b =>
                {
                    b.Property<int>("petTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("petTypeId"));

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("status")
                        .HasColumnType("bit");

                    b.HasKey("petTypeId");

                    b.ToTable("PetTypes");
                });

            modelBuilder.Entity("HealthyPawsV2.DAL.ApplicationUser", b =>
                {
                    b.HasOne("HealthyPawsV2.DAL.Address", null)
                        .WithMany("ApplicationUsers")
                        .HasForeignKey("addressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HealthyPawsV2.DAL.Appointment", b =>
                {
                    b.HasOne("HealthyPawsV2.DAL.ApplicationUser", "owner")
                        .WithMany()
                        .HasForeignKey("ownerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthyPawsV2.DAL.PetFile", "PetFile")
                        .WithMany("Appointments")
                        .HasForeignKey("petFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthyPawsV2.DAL.ApplicationUser", "veterinario")
                        .WithMany()
                        .HasForeignKey("veterinarioId");

                    b.Navigation("PetFile");

                    b.Navigation("owner");

                    b.Navigation("veterinario");
                });

            modelBuilder.Entity("HealthyPawsV2.DAL.AppointmentInventory", b =>
                {
                    b.HasOne("HealthyPawsV2.DAL.Appointment", "Appointment")
                        .WithMany("AppointmentInventories")
                        .HasForeignKey("appointmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthyPawsV2.DAL.Inventory", "Inventory")
                        .WithMany("AppointmentInventories")
                        .HasForeignKey("inventoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Appointment");

                    b.Navigation("Inventory");
                });

            modelBuilder.Entity("HealthyPawsV2.DAL.Document", b =>
                {
                    b.HasOne("HealthyPawsV2.DAL.Appointment", "Appointment")
                        .WithMany("Documents")
                        .HasForeignKey("AppointmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthyPawsV2.DAL.PetFile", "PetFile")
                        .WithMany("Documents")
                        .HasForeignKey("petFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Appointment");

                    b.Navigation("PetFile");
                });

            modelBuilder.Entity("HealthyPawsV2.DAL.LogReport", b =>
                {
                    b.HasOne("HealthyPawsV2.DAL.ApplicationUser", "CreatorId")
                        .WithMany()
                        .HasForeignKey("CreatorIdId");

                    b.Navigation("CreatorId");
                });

            modelBuilder.Entity("HealthyPawsV2.DAL.PetBreed", b =>
                {
                    b.HasOne("HealthyPawsV2.DAL.PetType", "PetType")
                        .WithMany("PetBreeds")
                        .HasForeignKey("petTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PetType");
                });

            modelBuilder.Entity("HealthyPawsV2.DAL.PetFile", b =>
                {
                    b.HasOne("HealthyPawsV2.DAL.ApplicationUser", "dueno")
                        .WithMany()
                        .HasForeignKey("duenoId");

                    b.HasOne("HealthyPawsV2.DAL.PetBreed", "PetBreed")
                        .WithMany("PetFiles")
                        .HasForeignKey("petBreedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PetBreed");

                    b.Navigation("dueno");
                });

            modelBuilder.Entity("HealthyPawsV2.DAL.Address", b =>
                {
                    b.Navigation("ApplicationUsers");
                });

            modelBuilder.Entity("HealthyPawsV2.DAL.Appointment", b =>
                {
                    b.Navigation("AppointmentInventories");

                    b.Navigation("Documents");
                });

            modelBuilder.Entity("HealthyPawsV2.DAL.Inventory", b =>
                {
                    b.Navigation("AppointmentInventories");
                });

            modelBuilder.Entity("HealthyPawsV2.DAL.PetBreed", b =>
                {
                    b.Navigation("PetFiles");
                });

            modelBuilder.Entity("HealthyPawsV2.DAL.PetFile", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("Documents");
                });

            modelBuilder.Entity("HealthyPawsV2.DAL.PetType", b =>
                {
                    b.Navigation("PetBreeds");
                });
#pragma warning restore 612, 618
        }
    }
}
