using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using Repository.IRepo;

namespace Repository.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser
    {
        public User()
        {
            //this.Classifieds = new HashSet<Classifieds>();
            this.Currency = new HashSet<Currency>();
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public int? Age { get; set; }

        #region additional field notmapped

        [NotMapped]
        [Display(Name = "Mr/Mrs: ")]
        public string FullName
        {
            get { return $"{Name} + {Surname}"; }
        }

        #endregion

        //public virtual ICollection<Classifieds> Classifieds { get; private set; }
        public virtual ICollection<Currency> Currency { get; private set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class OglContext : IdentityDbContext, IOglContext
    {
        public OglContext()
            : base("DefaultConnection")
        {
        }

        public static OglContext Create()
        {
            return new OglContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            /*
             Wylacza konwencje, ktora automatycznie tworzy liczbe mnoga dla nazw tabel w bazie danych
             Zamiast Category zostalaby utworzona tabela o nazwie Kategories
             */
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //wylacza konwencje CascadeDelete
            //CascadeDelete zostanie wylaczone za pomoca Fluent API
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //Uzywa sie Fluten API, aby ustawic powiazanie pomiedzy tabelami
            //i wlaczyc CascadeDelete dla tego powiazania
            //modelBuilder.Entity<Classifieds>().HasRequired(x =>
            //x.User).WithMany(x => x.Classifieds)
            //.HasForeignKey(x => x.UserId)
            //.WillCascadeOnDelete(true);

            modelBuilder.Entity<Currency>().HasRequired(x =>
            x.User).WithMany(x => x.Currency)
            .HasForeignKey(x => x.UserId)
            .WillCascadeOnDelete(true);
        }

        public DbSet<User> User { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
    }
}