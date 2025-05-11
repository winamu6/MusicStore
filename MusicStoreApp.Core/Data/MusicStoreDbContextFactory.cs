using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MusicStoreApp.Core.Data;

namespace MusicStoreApp
{
    public class MusicStoreDbContextFactory : IDesignTimeDbContextFactory<MusicStoreDbContext>
    {
        public MusicStoreDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MusicStoreDbContext>();
            optionsBuilder.UseSqlServer(
                "Data Source=DESKTOP-67UPOEU;Initial Catalog=MusicStore;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

            return new MusicStoreDbContext(optionsBuilder.Options);
        }
    }
}

