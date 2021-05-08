using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Models.Dao
{
    public class TrailerDao
    {
        MovieProjectDbcontext db = null;
        public TrailerDao()
        {
            db = new MovieProjectDbcontext();
        }
        public IEnumerable<Trailer> ListPg(int page, int pageSize)
        {
            return db.Trailers.OrderByDescending(x => x.TrailerID).ToPagedList(page, pageSize);
        }
        public bool Delete(int id)
        {
            try
            {
                var user = db.Trailers.Find(id);
                db.Trailers.Remove(user);
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
            var ad = db.Trailers.Find(id);
            ad.Status = !ad.Status;
            db.SaveChanges();
            return ad.Status;
        }
    }
}