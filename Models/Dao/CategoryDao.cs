using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dao
{
   public class CategoryDao
    {
        MovieProjectDbcontext db = null;
        public CategoryDao()
        {
            db = new MovieProjectDbcontext();
        }
        public IEnumerable<Category> ListPg(int page, int pageSize)
        {
            return db.Categories.OrderByDescending(x => x.CategoryID).ToPagedList(page, pageSize);
        }
        public  List<Category> ListAll()
        {
            return db.Categories.Where(x => x.Status == true).ToList();
        }
        public Category ViewDetail(long id)
        {
            return db.Categories.Find(id);
        }
        public bool Delete(int id)
        {
            try
            {
                var user = db.Categories.Find(id);
                db.Categories.Remove(user);
                db.SaveChanges();
                return true;

            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool ChangeStatus(int id)
        {
            var ad = db.Categories.Find(id);
            ad.Status = !ad.Status;
            db.SaveChanges();
            return ad.Status;
        }
    }
}