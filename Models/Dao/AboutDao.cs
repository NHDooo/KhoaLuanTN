using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dao
{
   public class AboutDao
    {
        MovieProjectDbcontext db = null;
        public AboutDao()
        {
            db = new MovieProjectDbcontext();
        }
        public IEnumerable<About> ListPg(int page, int pageSize)
        {
            return db.Abouts.OrderByDescending(x => x.AboutID).ToPagedList(page, pageSize);
        }
        public List<About> ListAll()
        {
            return db.Abouts.Where(x => x.Status == true).ToList();
        }
        public About GetContentAbout()
        {
            return db.Abouts.FirstOrDefault(x => x.Status==true);
        }
        public About ViewDetail(long id)
        {
            return db.Abouts.Find(id);
        }
        public bool Delete(int id)
        {
            try
            {
                var user = db.Abouts.Find(id);
                db.Abouts.Remove(user);
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
            var ad = db.Abouts.Find(id);
            ad.Status = !ad.Status;
            db.SaveChanges();
            return ad.Status;
        }
    }
}