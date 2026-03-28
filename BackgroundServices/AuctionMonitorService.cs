using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using your_auction_api.Data;
using your_auction_api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using your_auction_api.Utility;

namespace your_auction_api.BackgroundServices
{
    public class AuctionMonitorService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AuctionMonitorService> _logger;
        private const int CheckIntervalMinutes = 10;

        public AuctionMonitorService(IServiceProvider serviceProvider, ILogger<AuctionMonitorService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        var now = DateTime.UtcNow;
                        var expiredAuctions = await dbContext.auctions
                            .Where(a => a.status == AuctionStatus.Active && a.End_date <= now)
                            .ToListAsync(stoppingToken);

                        foreach (var auction in expiredAuctions)
                        {
                            var highestBid = dbContext.auctionUsers
                                .Where(b => b.AuctionId == auction.Id)
                                .OrderByDescending(b => b.AuctionValue)
                                .FirstOrDefault();

                            if (highestBid != null)
                            {
                                // auction. = highestBid.UserId;
                                // يمكن إضافة منطق إضافي هنا (إشعار الفائز، تحديث المنتج، إلخ)
                            }

                            auction.status = AuctionStatus.Completed;
                            dbContext.auctions.Update(auction);
                        }

                        await dbContext.SaveChangesAsync(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in AuctionMonitorService");
                }

                await Task.Delay(TimeSpan.FromMinutes(CheckIntervalMinutes), stoppingToken);
            }
        }
    }
}
