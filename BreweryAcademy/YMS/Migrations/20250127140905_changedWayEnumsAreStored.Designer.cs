﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using YMS.Data;

#nullable disable

namespace YMS.Migrations
{
    [DbContext(typeof(DefaultContext))]
    [Migration("20250127140905_changedWayEnumsAreStored")]
    partial class changedWayEnumsAreStored
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("YMS.Entities.CheckIn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DriverDocument")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("InvoiceReferenced")
                        .HasColumnType("int");

                    b.Property<string>("TruckPlate")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("nvarchar(14)");

                    b.HasKey("Id");

                    b.ToTable("CheckIns");
                });

            modelBuilder.Entity("YMS.Entities.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CheckInId")
                        .HasColumnType("int");

                    b.Property<int>("InvoiceId")
                        .HasColumnType("int");

                    b.Property<string>("InvoiceType")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("CheckInId")
                        .IsUnique();

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("YMS.Entities.InvoiceItem", b =>
                {
                    b.Property<int>("InvoiceItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InvoiceItemId"));

                    b.Property<int>("InvoiceId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("InvoiceItemId");

                    b.HasIndex("InvoiceId");

                    b.ToTable("InvoiceItem", (string)null);
                });

            modelBuilder.Entity("YMS.Entities.Invoice", b =>
                {
                    b.HasOne("YMS.Entities.CheckIn", "CheckIn")
                        .WithOne("Invoice")
                        .HasForeignKey("YMS.Entities.Invoice", "CheckInId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CheckIn");
                });

            modelBuilder.Entity("YMS.Entities.InvoiceItem", b =>
                {
                    b.HasOne("YMS.Entities.Invoice", null)
                        .WithMany("Items")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("YMS.Entities.CheckIn", b =>
                {
                    b.Navigation("Invoice")
                        .IsRequired();
                });

            modelBuilder.Entity("YMS.Entities.Invoice", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
