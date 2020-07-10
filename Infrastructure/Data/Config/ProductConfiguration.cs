using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;


namespace Infrastructure.Data.Config
{

    // allows you to configure how the migrations should happen 
    class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            
            builder.Property(p => p.Id).IsRequired(); // makes sure id is required 
            // makes sure name and description is required and sets a maximum length of characters
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(180);
            // sets the decimal property to only have 2 decimals 
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Property(p => p.PictureUrl).IsRequired();

            // sets up the relationships between product and brand/types 
            // a one to many relationship using brand/type id as its foreign key
            // usually EF works this out but can be explicitly stated if not 
            builder.HasOne(b => b.ProductBrand).WithMany()
                .HasForeignKey(p => p.ProductBrandId);
            builder.HasOne(b => b.ProductType).WithMany()
                .HasForeignKey(p => p.ProductTypeId); 
        }
    }
}
