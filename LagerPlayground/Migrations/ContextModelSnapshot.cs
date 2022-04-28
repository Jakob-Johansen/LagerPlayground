﻿// <auto-generated />
using System;
using LagerPlayground.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LagerPlayground.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<DateTime?>("Created")
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

            modelBuilder.Entity("LagerPlayground.Models.Locations", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Dynamic")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Row")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Section")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Warehouse")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("LagerPlayground.Models.Locations_Positions", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullLocationBarcode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Locations_ShelfsID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("Pickable")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<int>("PositionNumber")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Locations_ShelfsID");

                    b.ToTable("Locations_Positions");
                });

            modelBuilder.Entity("LagerPlayground.Models.Locations_Racks", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("LocationsID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<int>("RackNumber")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("LocationsID");

                    b.ToTable("Locations_Racks");
                });

            modelBuilder.Entity("LagerPlayground.Models.Locations_Shelfs", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("Locations_RacksID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<int>("ShelfNumber")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Locations_RacksID");

                    b.ToTable("Locations_Shelfs");
                });

            modelBuilder.Entity("LagerPlayground.Models.Order_Details", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<DateTime?>("Created")
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

                    b.Property<DateTime?>("Created")
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

            modelBuilder.Entity("LagerPlayground.Models.Product_Locations", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("LocationBarcode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Locations_PositionsID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("Locations_PositionsID")
                        .IsUnique()
                        .HasFilter("[Locations_PositionsID] IS NOT NULL");

                    b.HasIndex("ProductID");

                    b.ToTable("Product_Locations");
                });

            modelBuilder.Entity("LagerPlayground.Models.ReceiveCustommer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Vendor")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("ReceiveCustommers");
                });

            modelBuilder.Entity("LagerPlayground.Models.ReceiveRejected", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("ReceiveRejectedReasonsID")
                        .HasColumnType("int");

                    b.Property<int>("ReceivingOrder_ItemsID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ReceiveRejectedReasonsID");

                    b.HasIndex("ReceivingOrder_ItemsID");

                    b.ToTable("ReceiveRejecteds");
                });

            modelBuilder.Entity("LagerPlayground.Models.ReceiveRejectedReasons", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("ReceiveRejectedReasons");
                });

            modelBuilder.Entity("LagerPlayground.Models.ReceivingBox", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Barcode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Created")
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

            modelBuilder.Entity("LagerPlayground.Models.ReceivingBox_Items", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("ReceivingBoxID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ProductID");

                    b.HasIndex("ReceivingBoxID");

                    b.ToTable("ReceiveBox_Items");
                });

            modelBuilder.Entity("LagerPlayground.Models.ReceivingOrder_Details", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<DateTime?>("Closed")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Expected")
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

                    b.Property<DateTime?>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Modified")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("ReceivingOrder_DetailsID")
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

                    b.Property<DateTime?>("Created")
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

            modelBuilder.Entity("LagerPlayground.Models.Locations_Positions", b =>
                {
                    b.HasOne("LagerPlayground.Models.Locations_Shelfs", "Locations_Shelf")
                        .WithMany("Locations_Positions")
                        .HasForeignKey("Locations_ShelfsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Locations_Shelf");
                });

            modelBuilder.Entity("LagerPlayground.Models.Locations_Racks", b =>
                {
                    b.HasOne("LagerPlayground.Models.Locations", "Locations")
                        .WithMany("locations_Racks")
                        .HasForeignKey("LocationsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Locations");
                });

            modelBuilder.Entity("LagerPlayground.Models.Locations_Shelfs", b =>
                {
                    b.HasOne("LagerPlayground.Models.Locations_Racks", "Locations_Rack")
                        .WithMany("Locations_Shelfs")
                        .HasForeignKey("Locations_RacksID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Locations_Rack");
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

            modelBuilder.Entity("LagerPlayground.Models.Product_Locations", b =>
                {
                    b.HasOne("LagerPlayground.Models.Locations_Positions", "Locations_Positions")
                        .WithOne("Product_Locations")
                        .HasForeignKey("LagerPlayground.Models.Product_Locations", "Locations_PositionsID");

                    b.HasOne("LagerPlayground.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Locations_Positions");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("LagerPlayground.Models.ReceiveRejected", b =>
                {
                    b.HasOne("LagerPlayground.Models.ReceiveRejectedReasons", "ReceiveRejectedReasons")
                        .WithMany()
                        .HasForeignKey("ReceiveRejectedReasonsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LagerPlayground.Models.ReceivingOrder_Items", "ReceivingOrder_Items")
                        .WithMany("ReceiveRejecteds")
                        .HasForeignKey("ReceivingOrder_ItemsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ReceiveRejectedReasons");

                    b.Navigation("ReceivingOrder_Items");
                });

            modelBuilder.Entity("LagerPlayground.Models.ReceivingBox_Items", b =>
                {
                    b.HasOne("LagerPlayground.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LagerPlayground.Models.ReceivingBox", "ReceivingBox")
                        .WithMany()
                        .HasForeignKey("ReceivingBoxID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

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

            modelBuilder.Entity("LagerPlayground.Models.Locations", b =>
                {
                    b.Navigation("locations_Racks");
                });

            modelBuilder.Entity("LagerPlayground.Models.Locations_Positions", b =>
                {
                    b.Navigation("Product_Locations");
                });

            modelBuilder.Entity("LagerPlayground.Models.Locations_Racks", b =>
                {
                    b.Navigation("Locations_Shelfs");
                });

            modelBuilder.Entity("LagerPlayground.Models.Locations_Shelfs", b =>
                {
                    b.Navigation("Locations_Positions");
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

            modelBuilder.Entity("LagerPlayground.Models.ReceivingOrder_Items", b =>
                {
                    b.Navigation("ReceiveRejecteds");
                });
#pragma warning restore 612, 618
        }
    }
}
