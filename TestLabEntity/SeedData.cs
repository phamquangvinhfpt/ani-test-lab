using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestLabEntity.AutoDB;

namespace TestLabEntity
{
    public class SeedData
    {
        public static void Seed()
        {
            using (var context = new TestLabContext())
            {
                //Migrate the database if needed
                context.EnsureDatabaseCreated();
                context.SaveChanges();
                if (!context.TlAdmins.Any())
                {
                    context.TlAdmins.Add(new TlAdmin { Id = 1, Fullname = "Administrator", Username = "admin", Password = "admin" });
                    context.SaveChanges();
                }
            }
        }
    }
}
