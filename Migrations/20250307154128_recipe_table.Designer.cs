﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VetManagement.Repositories;

#nullable disable

namespace VetManagement.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250307154128_recipe_table")]
    partial class recipe_table
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("VetManagement.Data.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<long>("Date")
                        .HasColumnType("bigint");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("ProductCount")
                        .HasColumnType("int");

                    b.Property<string>("Provider")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("VetManagement.Data.Med", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<long>("DateAdded")
                        .HasColumnType("bigint");

                    b.Property<long>("DateUpdated")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<int>("InvoiceNumber")
                        .HasColumnType("int");

                    b.Property<string>("LotID")
                        .HasColumnType("longtext");

                    b.Property<string>("MeasurmentUnit")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("PerPiece")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("PieceType")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("PieceType");

                    b.Property<int>("Pieces")
                        .HasColumnType("int");

                    b.Property<string>("Provider")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long>("Valability")
                        .HasColumnType("bigint");

                    b.Property<string>("WaitingTime")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("InvoiceNumber");

                    b.ToTable("Meds");
                });

            modelBuilder.Entity("VetManagement.Data.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<bool>("Read")
                        .HasColumnType("tinyint(1)");

                    b.Property<long>("ReadAt")
                        .HasColumnType("bigint");

                    b.Property<long>("SentAt")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("VetManagement.Data.Owner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<int>("DateAdded")
                        .HasColumnType("int");

                    b.Property<int>("DateUpdated")
                        .HasColumnType("int");

                    b.Property<string>("Details")
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("varchar(15)");

                    b.HasKey("Id");

                    b.ToTable("Owners");
                });

            modelBuilder.Entity("VetManagement.Data.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Color")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<int>("DateAdded")
                        .HasColumnType("int");

                    b.Property<int>("DateUpdated")
                        .HasColumnType("int");

                    b.Property<string>("Details")
                        .HasColumnType("mediumtext");

                    b.Property<int?>("Identifier")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.Property<string>("Race")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Species")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<float>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("VetManagement.Data.Recipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<long>("Date")
                        .HasColumnType("bigint");

                    b.Property<string>("MedName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("OwnerSignature")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("RegistryNumber")
                        .HasColumnType("int");

                    b.Property<int>("RegistryRecordId")
                        .HasColumnType("int");

                    b.Property<bool>("Signed")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("TreatmentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RegistryRecordId");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("VetManagement.Data.RegistryRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<long>("Date")
                        .HasColumnType("bigint");

                    b.Property<string>("MedName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Observations")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Outcome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("TreatmentDuration")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("TreatmentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TreatmentId");

                    b.ToTable("RegistryRecords");
                });

            modelBuilder.Entity("VetManagement.Data.Treatment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DateAdded")
                        .HasColumnType("int");

                    b.Property<int>("DateUpdated")
                        .HasColumnType("int");

                    b.Property<string>("Details")
                        .HasColumnType("mediumtext");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("PatientId");

                    b.ToTable("Treatments");
                });

            modelBuilder.Entity("VetManagement.Data.TreatmentMed", b =>
                {
                    b.Property<int>("TreatmentId")
                        .HasColumnType("int");

                    b.Property<int>("MedId")
                        .HasColumnType("int");

                    b.Property<string>("Administration")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<decimal>("Pieces")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("TreatmentId", "MedId");

                    b.HasIndex("MedId");

                    b.ToTable("TreatmentMed");
                });

            modelBuilder.Entity("VetManagement.Data.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Phone")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("VetManagement.Data.Med", b =>
                {
                    b.HasOne("VetManagement.Data.Invoice", "Invoice")
                        .WithMany("Meds")
                        .HasForeignKey("InvoiceNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Invoice");
                });

            modelBuilder.Entity("VetManagement.Data.Patient", b =>
                {
                    b.HasOne("VetManagement.Data.Owner", "Owner")
                        .WithMany("Patients")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("VetManagement.Data.Recipe", b =>
                {
                    b.HasOne("VetManagement.Data.RegistryRecord", "RegistryRecord")
                        .WithMany()
                        .HasForeignKey("RegistryRecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RegistryRecord");
                });

            modelBuilder.Entity("VetManagement.Data.RegistryRecord", b =>
                {
                    b.HasOne("VetManagement.Data.Treatment", "Treatment")
                        .WithMany()
                        .HasForeignKey("TreatmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Treatment");
                });

            modelBuilder.Entity("VetManagement.Data.Treatment", b =>
                {
                    b.HasOne("VetManagement.Data.Owner", "Owner")
                        .WithMany("Treatments")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VetManagement.Data.Patient", "Patient")
                        .WithMany("Treatments")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("VetManagement.Data.TreatmentMed", b =>
                {
                    b.HasOne("VetManagement.Data.Med", "Med")
                        .WithMany("TreatmentMeds")
                        .HasForeignKey("MedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VetManagement.Data.Treatment", "Treatment")
                        .WithMany("TreatmentMeds")
                        .HasForeignKey("TreatmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Med");

                    b.Navigation("Treatment");
                });

            modelBuilder.Entity("VetManagement.Data.Invoice", b =>
                {
                    b.Navigation("Meds");
                });

            modelBuilder.Entity("VetManagement.Data.Med", b =>
                {
                    b.Navigation("TreatmentMeds");
                });

            modelBuilder.Entity("VetManagement.Data.Owner", b =>
                {
                    b.Navigation("Patients");

                    b.Navigation("Treatments");
                });

            modelBuilder.Entity("VetManagement.Data.Patient", b =>
                {
                    b.Navigation("Treatments");
                });

            modelBuilder.Entity("VetManagement.Data.Treatment", b =>
                {
                    b.Navigation("TreatmentMeds");
                });
#pragma warning restore 612, 618
        }
    }
}
