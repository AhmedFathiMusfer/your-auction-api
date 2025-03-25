using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using your_auction_api.Models;

namespace your_auction_api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions dbContext) : base(dbContext)
        {

        }

        public DbSet<ApplicationUser> users { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Category> categories { get; set; }

        public DbSet<Auction> auctions { get; set; }

        public DbSet<Feedback> feedbacks { get; set; }
        public DbSet<Notice> notices { get; set; }

        public DbSet<Commission> commissions { get; set; }

        public DbSet<Payment> payments { get; set; }
        public DbSet<ReportArchive> reportArchives { get; set; }

        public DbSet<ProductImage> productImages { get; set; }

        public DbSet<AuctionUser> auctionUsers { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Auction>().Property(a => a.state).HasConversion<int>();
            base.OnModelCreating(builder);
        }

    }
}