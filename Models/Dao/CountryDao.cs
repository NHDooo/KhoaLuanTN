using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dao
{
   public class CountryDao
    {
        MovieProjectDbcontext db = null;
        public CountryDao()
        {
            db = new MovieProjectDbcontext();
        }
        public IEnumerable<Country> ListPg(int page, int pageSize)
        {
            return db.Countries.OrderByDescending(x => x.CountryID).ToPagedList(page, pageSize);
        }
        public List<Country> ListAll()
        {
            return db.Countries.Where(x => x.Status == true).ToList();
        }
        public Country ViewDetail(long id)
        {
            return db.Countries.Find(id);
        }
        public bool Delete(int id)
        {
            try
            {
                var user = db.Countries.Find(id);
                db.Countries.Remove(user);
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
            var ad = db.Countries.Find(id);
            ad.Status = !ad.Status;
            db.SaveChanges();
            return ad.Status;
        }
    }
}