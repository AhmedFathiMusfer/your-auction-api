

using your_auction_api.Data.Repository.IRepository;
using your_auction_api.Models;

namespace your_auction_api.Data.Repository
{
    public class StripeCustomerRepository : Repository<StripeCustomer>, IStripeCustomerRepository
    {
        private readonly ApplicationDbContext _db;

        public StripeCustomerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateStripeCustomer(StripeCustomer stripeCustomer)
        {
            _db.stripeCustomers.Update(stripeCustomer);
            await _db.SaveChangesAsync();
        }
    }

}