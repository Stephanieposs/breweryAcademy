﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WMS.Data;

#nullable disable

namespace WMS.Migrations
{
    [DbContext(typeof(DefaultContext))]
    [Migration("20250128093424_OperationTypeUpdate")]
    partial class OperationTypeUpdate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WMS.Entities.Item", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int?>("StockId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StockId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("WMS.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("WMS.Entities.Stock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("InvoiceId")
                        .HasColumnType("int");

                    b.Property<string>("OperationType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("WMS.Entities.Item", b =>
                {
                    b.HasOne("WMS.Entities.Stock", null)
                        .WithMany("Products")
                        .HasForeignKey("StockId");
                });

            modelBuilder.Entity("WMS.Entities.Stock", b =>
                {
                    b.HasOne("WMS.Entities.Product", null)
                        .WithMany("Stocks")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("WMS.Entities.Product", b =>
                {
                    b.Navigation("Stocks");
                });

            modelBuilder.Entity("WMS.Entities.Stock", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
