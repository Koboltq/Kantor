using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Models;
using System.Data.Entity.Infrastructure;

namespace Repository.IRepo
{
    public interface IOglContext
    {
        DbSet<User> User { get; set; }
        DbSet<Currency> Currencies { get; set; }
        DbSet<Wallet> Wallets { get; set; }

        int SaveChanges();
        Database Database { get; }
        DbEntityEntry Entry(object entity);
    }
}
