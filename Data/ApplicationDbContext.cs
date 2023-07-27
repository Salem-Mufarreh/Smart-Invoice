﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Smart_Invoice.Models;
using Smart_Invoice.Models.Invoices;
using Smart_Invoice.Models.Products;
using Smart_Invoice.Models.Registered_Companies;
using Smart_Invoice.Models.Sales;
using Smart_Invoice.Models.Stock;
using Smart_Invoice.Models.Warehouse;

namespace Smart_Invoice.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ContactPerson> Contacts { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<UtilityInvoice> UtilityInvoices { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Product_Invoice> ProductInvoices { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Warehouse>Warehouses { get; set; }
        public DbSet<WarehouseProduct> WarehouseProducts { get; set;}
        public DbSet<Category> Categories { get; set; }
        public DbSet<InvoiceItem> InvoiceItem { get; set; }
        public DbSet<RegisteredCompany> RegisteredCompanies { get; set; }
        public DbSet<SalesInvoice> salesInvoices { get; set; }
        public DbSet<GinvoiceProp> ginvoices { get; set; }
    }
}