using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using your_auction_api.Models;

namespace your_auction_api.Data.Repository.IRepository
{
    public interface IStripeCustomerRepository : IRepository<StripeCustomer>
    {

        Task UpdateStripeCustomer(StripeCustomer stripeCustomer);

    }

}