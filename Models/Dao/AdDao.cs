using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dao
{
   public class AdDao
    {
        MovieProjectDbcontext db = null;
        public AdDao()
        {
            db = new MovieProjectDbcontext();
        }
       public IEnumerable<Ad> ListAd(int page, int pageSize)
        {
            return db.Ads.OrderByDescending(x => x.AdsID).ToPagedList(page, pageSize);
        }
        public Ad GetContentAd()
        {
            return db.Ads.FirstOrDefault(x => x.Status == true);
        }
        public bool Delete(int id)
        {
            try
            {
                var user = db.Ads.Find(id);
                db.Ads.Remove(user);
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
            var ad = db.Ads.Find(id);
            ad.Status = !ad.Status;
            db.SaveChanges();
            return ad.Status;
        }
    }
}