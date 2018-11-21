using GildedRose.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GildedRose.Data.Configurations
{
    class StoreItemConfiguration : IEntityTypeConfiguration<StoreItemDto>
    {
        public void Configure(EntityTypeBuilder<StoreItemDto> builder)
        {
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Category).IsRequired();
            builder.Property(x => x.SellIn).IsRequired();
            builder.Property(x => x.Quality).IsRequired();
        }
    }
}