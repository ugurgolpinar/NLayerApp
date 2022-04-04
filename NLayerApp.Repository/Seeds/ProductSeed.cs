﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayerApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayerApp.Repository.Seeds
{
    public class ProductSeed : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(
                new Product { Id = 1, Name = "Kalem1", CategoryId = 1, Price = 100, Stock = 20, CreatedDate = DateTime.Now },
                new Product { Id = 2, Name = "Kitap1", CategoryId = 2, Price = 40, Stock = 50, CreatedDate = DateTime.Now },
                new Product { Id = 3, Name = "Defter1", CategoryId = 3, Price = 20, Stock = 70, CreatedDate = DateTime.Now },
                new Product { Id = 4, Name = "Kitap2", CategoryId = 2, Price = 30, Stock = 60, CreatedDate = DateTime.Now }
                );
        }
    }
}
