using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Utils.EntityFramework;

namespace Test
{
    class EntityFrameWorkTest
    {
        static void Main(string[] args)
        {
            TestDbcontext contex = new TestDbcontext();

            IUserRepository repository = new UserRepository(contex);

            repository.Insert(new User { Age = 10, Name = "Jordan" });

            repository.Insert(new List<User> { new User { Age = 9, Name = "Kobe" }, new User { Age = 8, Name = "James" } });

            repository.Delete(1);

            repository.Delete(d => d.Name == "Jordan");

            repository.Delete(repository.Fetch(a => a.Name == "James"));

            var kobe = repository.Fetch(a => a.Name == "Kobe");

            kobe.Age = 10;
            repository.Update(kobe, new string[] { "Age" });

            kobe.Age = 11;
            repository.Update(kobe, a => a.Age);

            repository.Update(a => a.Name == "Kobe", new Dictionary<string, object> { { "Age", 12 } });

            repository.Update(a => a.Name == "Kobe", o => o.Age, 13);

            kobe.Age = 14;
            kobe.Name = "koko";
            repository.UpdateWithout(kobe, new string[] { "Name" });

            kobe.Age = 15;
            kobe.Name = "keke";
            repository.UpdateWithout(kobe, a => a.Name);

            var any = repository.Any(o => o.WithPredict(a => a.Name == "Kobe"));

            var count = repository.Count(o => o.Age > 5);

            var longcount = repository.LongCount(o => o.Age > 5);

            var exist = repository.Exist(o => o.Age == 8);

            var fetch = repository.Fetch(o => o.Name == "Kobe");

            var fetchorderby = repository.Fetch(a => a.Age > 8, o => o.Id, true);

            var find = repository.Find(9);

            var firstordefault = repository.FirstOrDefault(o => o.WithPredict(a => a.Age == 15));

            var firstordefaultresult = repository.FirstOrDefaultResult(o => o.Name, a => a.WithPredict(b => b.Age == 15));

            var get = repository.Get(o => o.WithPredict(a => a.Age > 8));

            var getresult = repository.GetResult(o => o.Name, a => a.WithPredict(b => true));

            var getpagedlist = repository.GetPagedList(o => o.WithPredict(a => true).WithOrderBy(b => b.OrderBy(c => c.Id)), 1, 2);

            var getpagedlistresult = repository.GetPagedListResult(o => o.Name, a => a.WithPredict(b => true).WithOrderBy(c => c.OrderByDescending(d => d.Id)), 1, 3);

            var paged = repository.Paged(1, 2, a => a.Age > 5, o => o.Id, true);

            var query = repository.Query(o => o.WithPredict(a => a.Id > 0)).ToList();

            var select = repository.Select(o => o.Id > 2);

            var selectorder = repository.Select(5, o => o.Id > 1, a => a.Age, true);

            using (IBaseUintOfWork uintOfWork = new BaseUintOfWork(contex))
            {
                uintOfWork.DbContext.Update(new User { Id = 9, Age = 55 }, "Age");
                uintOfWork.DbContext.Update(new User { Id = 13, Age = 55 }, a => a.Age);
                uintOfWork.DbSet<User>().Remove(new User { Id = 2 });
                uintOfWork.DbSet<User>().Add(new User { Age = 20, Name = "Yaoming" });
                uintOfWork.Commit();
            }

            Console.ReadKey();
        }
    }

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(TestDbcontext dbContext) : base(dbContext)
        {
        }
    }
    public interface IUserRepository : IBaseRepository<User>
    {

    }

    [Table("User")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
    public class BaseRepository<TEntity> : EFRepository<TestDbcontext, TEntity>, IBaseRepository<TEntity> where TEntity : class
    {
        public BaseRepository(TestDbcontext dbContext) : base(dbContext)
        {
        }
    }
    public interface IBaseRepository<TEntity>:IEFRepository<TestDbcontext,TEntity> where TEntity:class
    {

    }
    public class BaseUintOfWork : EFUnitOfWork<TestDbcontext>,IBaseUintOfWork
    {
        public BaseUintOfWork(TestDbcontext dbContext) : base(dbContext)
        {
        }
    }
    public interface IBaseUintOfWork : IEFUnitOfWork<TestDbcontext>
    {

    }
    public class TestDbcontext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=(local);Initial Catalog=TestDb;Persist Security Info=True;User ID=sa;Password=123456;");
        }
        public DbSet<User> Users { get; set; }
    }
}
