﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using backend.Data.Models.DataBase;

#nullable disable

namespace backend.Data.Migrations
{
    [DbContext(typeof(DataBase))]
    [Migration("20240321232117_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("backend.Data.Models.AccountSettings", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("AccountSettings");
                });

            modelBuilder.Entity("backend.Data.Models.Auction", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("UserId1")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId1");

                    b.ToTable("Auction");
                });

            modelBuilder.Entity("backend.Data.Models.Bucket", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Bucket");
                });

            modelBuilder.Entity("backend.Data.Models.BuyerRate", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("BuyerRates");
                });

            modelBuilder.Entity("backend.Data.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("backend.Data.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("time(6)");

                    b.Property<int>("UserId1")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId1");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("backend.Data.Models.Delivery", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("Price")
                        .HasColumnType("double");

                    b.Property<string>("deliveryName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Delivery");
                });

            modelBuilder.Entity("backend.Data.Models.Discount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountSettingsId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountSettingsId");

                    b.ToTable("Discount");
                });

            modelBuilder.Entity("backend.Data.Models.Favourite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Favourite");
                });

            modelBuilder.Entity("backend.Data.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountSettingsId")
                        .HasColumnType("int");

                    b.Property<int>("ApartmentNumber")
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("HomeNumber")
                        .HasColumnType("int");

                    b.Property<int>("PostalCode")
                        .HasColumnType("int");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AccountSettingsId");

                    b.ToTable("Location");
                });

            modelBuilder.Entity("backend.Data.Models.ManyToManyConnections.AuctionUser", b =>
                {
                    b.Property<int>("AuctionId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("AuctionId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("AuctionUser");
                });

            modelBuilder.Entity("backend.Data.Models.ManyToManyConnections.CategoryProduct", b =>
                {
                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("CategoryId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("CategoryProduct");
                });

            modelBuilder.Entity("backend.Data.Models.ManyToManyConnections.OrderProduct", b =>
                {
                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("OrderId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderProduct");
                });

            modelBuilder.Entity("backend.Data.Models.ManyToManyConnections.UserFavourite", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("FavouriteId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "FavouriteId");

                    b.HasIndex("FavouriteId");

                    b.ToTable("UserFavourite");
                });

            modelBuilder.Entity("backend.Data.Models.OnSale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("OnSale");
                });

            modelBuilder.Entity("backend.Data.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("backend.Data.Models.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountSettingsId")
                        .HasColumnType("int");

                    b.Property<int>("CardNumber")
                        .HasColumnType("int");

                    b.Property<string>("ExpirationDate")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("SecureCode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountSettingsId");

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("backend.Data.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("BucketId")
                        .HasColumnType("int");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BucketId");

                    b.HasIndex("UserId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("backend.Data.Models.Rate", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("OverallRate")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("time(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Rate");
                });

            modelBuilder.Entity("backend.Data.Models.Receipt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Receipt");
                });

            modelBuilder.Entity("backend.Data.Models.SellerRate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("SellerRate");
                });

            modelBuilder.Entity("backend.Data.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "JPrzybysz@mail.com",
                            LastName = "Przybysz",
                            Name = "Joanna",
                            Password = "JPrzybysz123.",
                            Username = "JPrzybysz"
                        });
                });

            modelBuilder.Entity("backend.Data.Models.AccountSettings", b =>
                {
                    b.HasOne("backend.Data.Models.User", "User")
                        .WithOne("AccountSettings")
                        .HasForeignKey("backend.Data.Models.AccountSettings", "Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("backend.Data.Models.Auction", b =>
                {
                    b.HasOne("backend.Data.Models.Product", "Product")
                        .WithOne("Auction")
                        .HasForeignKey("backend.Data.Models.Auction", "Id");

                    b.HasOne("backend.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("backend.Data.Models.Bucket", b =>
                {
                    b.HasOne("backend.Data.Models.User", "User")
                        .WithOne("Bucket")
                        .HasForeignKey("backend.Data.Models.Bucket", "Id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("backend.Data.Models.BuyerRate", b =>
                {
                    b.HasOne("backend.Data.Models.Rate", "Rate")
                        .WithOne("BuyerRate")
                        .HasForeignKey("backend.Data.Models.BuyerRate", "Id");

                    b.Navigation("Rate");
                });

            modelBuilder.Entity("backend.Data.Models.Comment", b =>
                {
                    b.HasOne("backend.Data.Models.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("backend.Data.Models.Delivery", b =>
                {
                    b.HasOne("backend.Data.Models.Order", "Order")
                        .WithOne("Delivery")
                        .HasForeignKey("backend.Data.Models.Delivery", "Id");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("backend.Data.Models.Discount", b =>
                {
                    b.HasOne("backend.Data.Models.AccountSettings", "AccountSettings")
                        .WithMany("Discounts")
                        .HasForeignKey("AccountSettingsId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("AccountSettings");
                });

            modelBuilder.Entity("backend.Data.Models.Location", b =>
                {
                    b.HasOne("backend.Data.Models.AccountSettings", "AccountSettings")
                        .WithMany("Locations")
                        .HasForeignKey("AccountSettingsId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("AccountSettings");
                });

            modelBuilder.Entity("backend.Data.Models.ManyToManyConnections.AuctionUser", b =>
                {
                    b.HasOne("backend.Data.Models.Auction", "Auction")
                        .WithMany("AuctionUsers")
                        .HasForeignKey("AuctionId");

                    b.HasOne("backend.Data.Models.User", "User")
                        .WithMany("AuctionUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Auction");

                    b.Navigation("User");
                });

            modelBuilder.Entity("backend.Data.Models.ManyToManyConnections.CategoryProduct", b =>
                {
                    b.HasOne("backend.Data.Models.Category", "Category")
                        .WithMany("CategoryProducts")
                        .HasForeignKey("CategoryId");

                    b.HasOne("backend.Data.Models.Product", "Product")
                        .WithMany("CategoryProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Category");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("backend.Data.Models.ManyToManyConnections.OrderProduct", b =>
                {
                    b.HasOne("backend.Data.Models.Order", "Order")
                        .WithMany("OrderProducts")
                        .HasForeignKey("OrderId");

                    b.HasOne("backend.Data.Models.Product", "Product")
                        .WithMany("OrderProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("backend.Data.Models.ManyToManyConnections.UserFavourite", b =>
                {
                    b.HasOne("backend.Data.Models.Favourite", "Favourite")
                        .WithMany("UserFavourite")
                        .HasForeignKey("FavouriteId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("backend.Data.Models.User", "User")
                        .WithMany("UserFavourite")
                        .HasForeignKey("UserId");

                    b.Navigation("Favourite");

                    b.Navigation("User");
                });

            modelBuilder.Entity("backend.Data.Models.Order", b =>
                {
                    b.HasOne("backend.Data.Models.Receipt", "Receipt")
                        .WithOne("Order")
                        .HasForeignKey("backend.Data.Models.Order", "Id");

                    b.HasOne("backend.Data.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId");

                    b.Navigation("Receipt");

                    b.Navigation("User");
                });

            modelBuilder.Entity("backend.Data.Models.Payment", b =>
                {
                    b.HasOne("backend.Data.Models.AccountSettings", "AccountSettings")
                        .WithMany("Payments")
                        .HasForeignKey("AccountSettingsId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("AccountSettings");
                });

            modelBuilder.Entity("backend.Data.Models.Product", b =>
                {
                    b.HasOne("backend.Data.Models.Bucket", "Bucket")
                        .WithMany("Products")
                        .HasForeignKey("BucketId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("backend.Data.Models.OnSale", "OnSale")
                        .WithOne("Product")
                        .HasForeignKey("backend.Data.Models.Product", "Id");

                    b.HasOne("backend.Data.Models.User", "User")
                        .WithMany("Products")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Bucket");

                    b.Navigation("OnSale");

                    b.Navigation("User");
                });

            modelBuilder.Entity("backend.Data.Models.Rate", b =>
                {
                    b.HasOne("backend.Data.Models.SellerRate", "SellerRate")
                        .WithOne("Rate")
                        .HasForeignKey("backend.Data.Models.Rate", "Id");

                    b.HasOne("backend.Data.Models.User", "User")
                        .WithMany("Rates")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SellerRate");

                    b.Navigation("User");
                });

            modelBuilder.Entity("backend.Data.Models.AccountSettings", b =>
                {
                    b.Navigation("Discounts");

                    b.Navigation("Locations");

                    b.Navigation("Payments");
                });

            modelBuilder.Entity("backend.Data.Models.Auction", b =>
                {
                    b.Navigation("AuctionUsers");
                });

            modelBuilder.Entity("backend.Data.Models.Bucket", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("backend.Data.Models.Category", b =>
                {
                    b.Navigation("CategoryProducts");
                });

            modelBuilder.Entity("backend.Data.Models.Favourite", b =>
                {
                    b.Navigation("UserFavourite");
                });

            modelBuilder.Entity("backend.Data.Models.OnSale", b =>
                {
                    b.Navigation("Product")
                        .IsRequired();
                });

            modelBuilder.Entity("backend.Data.Models.Order", b =>
                {
                    b.Navigation("Delivery")
                        .IsRequired();

                    b.Navigation("OrderProducts");
                });

            modelBuilder.Entity("backend.Data.Models.Product", b =>
                {
                    b.Navigation("Auction")
                        .IsRequired();

                    b.Navigation("CategoryProducts");

                    b.Navigation("OrderProducts");
                });

            modelBuilder.Entity("backend.Data.Models.Rate", b =>
                {
                    b.Navigation("BuyerRate")
                        .IsRequired();
                });

            modelBuilder.Entity("backend.Data.Models.Receipt", b =>
                {
                    b.Navigation("Order");
                });

            modelBuilder.Entity("backend.Data.Models.SellerRate", b =>
                {
                    b.Navigation("Rate");
                });

            modelBuilder.Entity("backend.Data.Models.User", b =>
                {
                    b.Navigation("AccountSettings");

                    b.Navigation("AuctionUsers");

                    b.Navigation("Bucket");

                    b.Navigation("Comments");

                    b.Navigation("Orders");

                    b.Navigation("Products");

                    b.Navigation("Rates");

                    b.Navigation("UserFavourite");
                });
#pragma warning restore 612, 618
        }
    }
}