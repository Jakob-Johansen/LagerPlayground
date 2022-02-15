﻿// <auto-generated />
using System;
using LagerPlayground.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LagerPlayground.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20220209221224_ReceivingBoxItemsCreate")]
    partial class ReceivingBoxItemsCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("LagerPlayground.Models.Custommer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MobileNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Zipcode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Custommers");
                });

            modelBuilder.Entity("LagerPlayground.Models.Order_Details", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("CustommerID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrderStatus")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("CustommerID")
                        .IsUnique();

                    b.ToTable("Order_Details");
                });

            modelBuilder.Entity("LagerPlayground.Models.Order_Items", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<int>("Order_DetailsID")
                        .HasColumnType("int");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Order_DetailsID");

                    b.HasIndex("ProductID");

                    b.ToTable("Order_Items");
                });

            modelBuilder.Entity("LagerPlayground.Models.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("BarcodeID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BrandName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("LagerPlayground.Models.ReceiveBox_Items", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("ReceiveBoxID")
                        .HasColumnType("int");

                    b.Property<int?>("ReceivingBoxID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ProductID");

                    b.HasIndex("ReceivingBoxID");

                    b.ToTable("ReceiveBox_Items");
                });

            modelBuilder.Entity("LagerPlayground.Models.ReceiveCustommer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Vendor")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("ReceiveCustommers");
                });

            modelBuilder.Entity("LagerPlayground.Models.ReceivingBox", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Barcode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<string>("Warehouse")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("ReceivingBoxes");
                });

            modelBuilder.Entity("LagerPlayground.Models.ReceivingOrder_Details", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<DateTime?>("Closed")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Expected")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrderStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReceiveCustommerID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ReceiveCustommerID")
                        .IsUnique();

                    b.ToTable("ReceivingOrder_Details");
                });

            modelBuilder.Entity("LagerPlayground.Models.ReceivingOrder_Items", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("Accepted")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("ReceivingOrder_DetailsID")
                        .HasColumnType("int");

                    b.Property<int>("Rejected")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ProductID");

                    b.HasIndex("ReceivingOrder_DetailsID");

                    b.ToTable("ReceivingOrder_Items");
                });

            modelBuilder.Entity("LagerPlayground.Models.Tote", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Barcode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<string>("Warehouse")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Totes");
                });

            modelBuilder.Entity("LagerPlayground.Models.Order_Details", b =>
                {
                    b.HasOne("LagerPlayground.Models.Custommer", "Custommer")
                        .WithOne("OrderDetail")
                        .HasForeignKey("LagerPlayground.Models.Order_Details", "CustommerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Custommer");
                });

            modelBuilder.Entity("LagerPlayground.Models.Order_Items", b =>
                {
                    b.HasOne("LagerPlayground.Models.Order_Details", "Order_Details")
                        .WithMany("Order_Items")
                        .HasForeignKey("Order_DetailsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LagerPlayground.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order_Details");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("LagerPlayground.Models.ReceiveBox_Items", b =>
                {
                    b.HasOne("LagerPlayground.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LagerPlayground.Models.ReceivingBox", "ReceivingBox")
                        .WithMany()
                        .HasForeignKey("ReceivingBoxID");

                    b.Navigation("Product");

                    b.Navigation("ReceivingBox");
                });

            modelBuilder.Entity("LagerPlayground.Models.ReceivingOrder_Details", b =>
                {
                    b.HasOne("LagerPlayground.Models.ReceiveCustommer", "ReceiveCustommer")
                        .WithOne("ReceivingOrder_Details")
                        .HasForeignKey("LagerPlayground.Models.ReceivingOrder_Details", "ReceiveCustommerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ReceiveCustommer");
                });

            modelBuilder.Entity("LagerPlayground.Models.ReceivingOrder_Items", b =>
                {
                    b.HasOne("LagerPlayground.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LagerPlayground.Models.ReceivingOrder_Details", "ReceivingOrder_Details")
                        .WithMany("ReceivingOrder_Items")
                        .HasForeignKey("ReceivingOrder_DetailsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("ReceivingOrder_Details");
                });

            modelBuilder.Entity("LagerPlayground.Models.Custommer", b =>
                {
                    b.Navigation("OrderDetail");
                });

            modelBuilder.Entity("LagerPlayground.Models.Order_Details", b =>
                {
                    b.Navigation("Order_Items");
                });

            modelBuilder.Entity("LagerPlayground.Models.ReceiveCustommer", b =>
                {
                    b.Navigation("ReceivingOrder_Details");
                });

            modelBuilder.Entity("LagerPlayground.Models.ReceivingOrder_Details", b =>
                {
                    b.Navigation("ReceivingOrder_Items");
                });
#pragma warning restore 612, 618
        }
    }
}