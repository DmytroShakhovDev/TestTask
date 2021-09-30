using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject.WebAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace TestProject.WebAPI.SeedData
{
    public class Seed
    {
        public static void EnsureSeedData(IServiceProvider provider)
        {
            using (var context = provider.GetService<TestProjectDbContext>())
            {

                if (!context.Database.GetDbConnection().ConnectionString
                    .Contains("EFProviders.InMemory"))
                {

                    context.Database.Migrate();

                    using (var transaction = context.Database.BeginTransaction())
                    {
                        CreateUsers(context);
                        CreateAcounts(context);
                        transaction.Commit();
                    }

                }
            }
            GC.Collect();
        }

        private static void CreateAcounts(TestProjectDbContext context)
        {
            List<Account> accounts = new List<Account>();
            List<User> users = new List<User>();
            foreach(var item in context.Users)
            {
                users.Add(new User()
                {
                    Id=item.Id,
                    Email=item.Email,
                    MonthlyExpenses=item.MonthlyExpenses,
                    MonthlySalary=item.MonthlySalary,
                    Name=item.Name
                });
            }

            foreach (var user in users)
            {
                if (!context.Accounts.Where(x => x.UserId == user.Id).Any())
                {
                    accounts.Add(new Account()
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        Credit = user.MonthlySalary
                    });
                }
            }
           
            bool created = false;
            foreach (var item in accounts)
            {
                if (!context.Accounts.Where(x => x.UserId == item.UserId).Any())
                {
                    context.Accounts.Add(item);
                    context.SaveChanges();
                    created = true;
                }
            }
            if (created)
            {
                Console.WriteLine("Accounts created.");
            }
        }

        private static void CreateUsers(TestProjectDbContext context)
        {
            List<User> users = new List<User>();

            users.Add(new User()
            {
                Id = Guid.NewGuid(),
                Name = "Foo",
                Email = "Foo@example.com",
                MonthlyExpenses = 100,
                MonthlySalary = 100
            });
            users.Add(new User()
            {
                Id = Guid.NewGuid(),
                Name = "Bar",
                Email = "Bar@example.com",
                MonthlyExpenses = 190,
                MonthlySalary = 200
            });
            bool created = false;
            foreach (var item in users)
            {
                if (!context.Users.Where(x => x.Email == item.Email).Any())
                {
                    context.Users.Add(item);
                    context.SaveChanges();
                    created = true;
                }
            }
            if (created)
            {
                Console.WriteLine("Users created.");
            }
        }
    }
}
