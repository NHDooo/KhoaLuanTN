using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dao
{
    public class SlideDao
    {
        MovieProjectDbcontext db = null;
        public SlideDao()
        {
            db = new MovieProjectDbcontext();
        }
        public IEnumerable<Slide> ListPg(int page, int pageSize)
        {
            return db.Slides.OrderByDescending(x => x.SlideID).ToPagedList(page, pageSize);
        }
        public List<Slide> ListAllSlide(int top)
        {
            return db.Slides.Where(X => X.Status == true).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }
        public bool Delete(int id)
        {
            try
            {
                var user = db.Slides.Find(id);
                db.Slides.Remove(user);
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
            var ad = db.Slides.Find(id);
            ad.Status = !ad.Status;
            db.SaveChanges();
            return ad.Status;
        }
    }
}