using EventMate.Core.Model.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventMate.Repository.DataSeed
{
    public class CategorySeed : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(
                new Category { Id = 1, Name = "Cinema", CreatedDate = DateTime.Now, CreatedBy = "SYSTEM",IsActive = true },
                new Category { Id = 2, Name = "Music", CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true },
                new Category { Id = 3, Name = "Technology", CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true },
                new Category {Id = 4, Name = "Science", CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true },
                new Category { Id = 5, Name = "Sport", CreatedDate = DateTime.Now, CreatedBy = "SYSTEM", IsActive = true }

                );
        }
    }
}
