﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Smart_Invoice.Data;

#nullable disable

namespace Smart_Invoice.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230710101058_update invoice number to string")]
    partial class updateinvoicenumbertostring
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

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
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Smart_Invoice.Models.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CompanyId"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Company_License_Registration_Number")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Company_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Company_Name_English")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Company_Name_Normilized")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ContactPersonId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CompanyId");

                    b.HasIndex("ContactPersonId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Smart_Invoice.Models.ContactPerson", b =>
                {
                    b.Property<int>("ContactPersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ContactPersonId"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ContactPersonId");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("Smart_Invoice.Models.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerId"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Customer_Name_English")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Customer_Name_Normilized")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Store_Tax_Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ZipCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("customer_number")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Smart_Invoice.Models.Invoices.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<long?>("CompanyCode")
                        .HasColumnType("bigint");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int?>("CompanyId1")
                        .HasColumnType("int");

                    b.Property<string>("Invoice_Date")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Invoice_Id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Invoice_Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Invoice_Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("RegisteredCompanyCompanyCode")
                        .HasColumnType("bigint");

                    b.Property<double?>("Subtotal")
                        .IsRequired()
                        .HasColumnType("float");

                    b.Property<double?>("Tax")
                        .IsRequired()
                        .HasColumnType("float");

                    b.Property<double?>("Total")
                        .IsRequired()
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CompanyId1");

                    b.HasIndex("RegisteredCompanyCompanyCode");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("Smart_Invoice.Models.Invoices.InvoiceItem", b =>
                {
                    b.Property<int?>("InvoiceItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("InvoiceItemId"), 1L, 1);

                    b.Property<double>("Discount")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductInvoiceId")
                        .HasColumnType("int");

                    b.Property<int?>("Quantity")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("SalesInvoiceId")
                        .HasColumnType("int");

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.Property<string>("Unit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("UnitPrice")
                        .HasColumnType("float");

                    b.Property<int?>("productId")
                        .HasColumnType("int");

                    b.HasKey("InvoiceItemId");

                    b.HasIndex("ProductInvoiceId");

                    b.HasIndex("SalesInvoiceId");

                    b.HasIndex("productId");

                    b.ToTable("InvoiceItem");
                });

            modelBuilder.Entity("Smart_Invoice.Models.Products.Category", b =>
                {
                    b.Property<long>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("CategoryId"), 1L, 1);

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Smart_Invoice.Models.Products.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"), 1L, 1);

                    b.Property<long?>("CategoryId")
                        .HasColumnType("bigint");

                    b.Property<double?>("CostPrice")
                        .HasColumnType("float");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Price")
                        .HasColumnType("float");

                    b.Property<string>("SKU")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Smart_Invoice.Models.Registered_Companies.RegisteredCompany", b =>
                {
                    b.Property<long>("CompanyCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("CompanyCode"), 1L, 1);

                    b.Property<string>("CompanyAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyPhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("GovCompanyRegistration")
                        .HasColumnType("bigint");

                    b.HasKey("CompanyCode");

                    b.ToTable("RegisteredCompanies");
                });

            modelBuilder.Entity("Smart_Invoice.Models.Sales.SalesInvoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<double>("AmountPaid")
                        .HasColumnType("float");

                    b.Property<double>("BalanceDue")
                        .HasColumnType("float");

                    b.Property<int?>("CustomerId")
                        .HasColumnType("int");

                    b.Property<double>("Discount")
                        .HasColumnType("float");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Invoice_number")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("SubTotal")
                        .HasColumnType("float");

                    b.Property<double>("Tax")
                        .HasColumnType("float");

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("salesInvoices");
                });

            modelBuilder.Entity("Smart_Invoice.Models.Stock.Inventory", b =>
                {
                    b.Property<int>("InventoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InventoryId"), 1L, 1);

                    b.Property<long?>("CompanyCode")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("LastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductCount")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("PurchaseDate")
                        .HasColumnType("datetime2");

                    b.Property<long?>("RegisteredCompanyCompanyCode")
                        .HasColumnType("bigint");

                    b.Property<string>("SKU")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("SellingPrice")
                        .IsRequired()
                        .HasColumnType("float");

                    b.Property<int?>("WarehouseId")
                        .HasColumnType("int");

                    b.HasKey("InventoryId");

                    b.HasIndex("RegisteredCompanyCompanyCode");

                    b.HasIndex("WarehouseId");

                    b.ToTable("Inventories");
                });

            modelBuilder.Entity("Smart_Invoice.Models.Warehouse.Warehouse", b =>
                {
                    b.Property<int>("WarehouseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WarehouseId"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("AvailableSpace")
                        .HasColumnType("int");

                    b.Property<int?>("Capacity")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<double?>("OccupancyRate")
                        .HasColumnType("float");

                    b.Property<long?>("RCompanyCode")
                        .HasColumnType("bigint");

                    b.Property<long?>("RegisteredCompanyCompanyCode")
                        .HasColumnType("bigint");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WarehouseCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WarehouseName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("WarehouseId");

                    b.HasIndex("RegisteredCompanyCompanyCode");

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("Smart_Invoice.Models.Warehouse.WarehouseProduct", b =>
                {
                    b.Property<int>("WarehouseProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WarehouseProductId"), 1L, 1);

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("WarehouseId")
                        .HasColumnType("int");

                    b.HasKey("WarehouseProductId");

                    b.HasIndex("ProductId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("WarehouseProducts");
                });

            modelBuilder.Entity("Smart_Invoice.Models.Invoices.Product_Invoice", b =>
                {
                    b.HasBaseType("Smart_Invoice.Models.Invoices.Invoice");

                    b.ToTable("ProductInvoice");
                });

            modelBuilder.Entity("Smart_Invoice.Models.Invoices.UtilityInvoice", b =>
                {
                    b.HasBaseType("Smart_Invoice.Models.Invoices.Invoice");

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Current_Reading")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Meter_Number")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Previous_Debt")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Previous_Reading_Date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Service_Number")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("UtilityInvoices");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Smart_Invoice.Models.Company", b =>
                {
                    b.HasOne("Smart_Invoice.Models.ContactPerson", "person")
                        .WithMany()
                        .HasForeignKey("ContactPersonId");

                    b.Navigation("person");
                });

            modelBuilder.Entity("Smart_Invoice.Models.Invoices.Invoice", b =>
                {
                    b.HasOne("Smart_Invoice.Models.Company", "CompanyID")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Smart_Invoice.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId1");

                    b.HasOne("Smart_Invoice.Models.Registered_Companies.RegisteredCompany", "RegisteredCompany")
                        .WithMany()
                        .HasForeignKey("RegisteredCompanyCompanyCode");

                    b.Navigation("Company");

                    b.Navigation("CompanyID");

                    b.Navigation("RegisteredCompany");
                });

            modelBuilder.Entity("Smart_Invoice.Models.Invoices.InvoiceItem", b =>
                {
                    b.HasOne("Smart_Invoice.Models.Invoices.Product_Invoice", "ProductInvoice")
                        .WithMany("Items")
                        .HasForeignKey("ProductInvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Smart_Invoice.Models.Sales.SalesInvoice", null)
                        .WithMany("Products")
                        .HasForeignKey("SalesInvoiceId");

                    b.HasOne("Smart_Invoice.Models.Products.Product", "Product")
                        .WithMany()
                        .HasForeignKey("productId");

                    b.Navigation("Product");

                    b.Navigation("ProductInvoice");
                });

            modelBuilder.Entity("Smart_Invoice.Models.Products.Product", b =>
                {
                    b.HasOne("Smart_Invoice.Models.Products.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Smart_Invoice.Models.Sales.SalesInvoice", b =>
                {
                    b.HasOne("Smart_Invoice.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Smart_Invoice.Models.Stock.Inventory", b =>
                {
                    b.HasOne("Smart_Invoice.Models.Registered_Companies.RegisteredCompany", "RegisteredCompany")
                        .WithMany()
                        .HasForeignKey("RegisteredCompanyCompanyCode");

                    b.HasOne("Smart_Invoice.Models.Warehouse.Warehouse", "Warehouse")
                        .WithMany()
                        .HasForeignKey("WarehouseId");

                    b.Navigation("RegisteredCompany");

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("Smart_Invoice.Models.Warehouse.Warehouse", b =>
                {
                    b.HasOne("Smart_Invoice.Models.Registered_Companies.RegisteredCompany", "RegisteredCompany")
                        .WithMany()
                        .HasForeignKey("RegisteredCompanyCompanyCode");

                    b.Navigation("RegisteredCompany");
                });

            modelBuilder.Entity("Smart_Invoice.Models.Warehouse.WarehouseProduct", b =>
                {
                    b.HasOne("Smart_Invoice.Models.Products.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Smart_Invoice.Models.Warehouse.Warehouse", "Warehouse")
                        .WithMany("WarehouseProducts")
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("Smart_Invoice.Models.Invoices.Product_Invoice", b =>
                {
                    b.HasOne("Smart_Invoice.Models.Invoices.Invoice", "Invoice")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Invoice");
                });

            modelBuilder.Entity("Smart_Invoice.Models.Invoices.UtilityInvoice", b =>
                {
                    b.HasOne("Smart_Invoice.Models.Invoices.Invoice", null)
                        .WithOne()
                        .HasForeignKey("Smart_Invoice.Models.Invoices.UtilityInvoice", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Smart_Invoice.Models.Sales.SalesInvoice", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Smart_Invoice.Models.Warehouse.Warehouse", b =>
                {
                    b.Navigation("WarehouseProducts");
                });

            modelBuilder.Entity("Smart_Invoice.Models.Invoices.Product_Invoice", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
