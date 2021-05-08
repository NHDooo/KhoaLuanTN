using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.EF;
using PagedList;

namespace Models.Dao
{
    public class NewDao
    {

        MovieProjectDbcontext db = null;
        public NewDao()
        {
            db = new MovieProjectDbcontext();
        }
        public IEnumerable<News> ListPg(int page, int pageSize)
        {
            return db.News.OrderByDescending(x => x.CategoryID).ToPagedList(page, pageSize);
        }
        public List<News> ListNews(int top)
        {
            return db.News.Where(x => x.Status == true).OrderBy(x => x.CreatedDate).Take(top).ToList();
        }
        public List<News> ListNewsNew(int top)
        {
            return db.News.Where(y => y.Status == true).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }
        public List<News> ListNewsTop(int top)
        {
            return db.News.Where(x => x.Status == true).OrderByDescending(x => x.Viewed).Take(top).ToList();
        }
        public News ViewDetail(int id)
        {
            return db.News.Find(id);
        }
        public bool Delete(int id)
        {
            try
            {
                var user = db.News.Find(id);
                db.News.Remove(user);
                db.SaveChanges();
                return true;

            }
            catch (Exception )
            {
                return false;
            }

        }
        public bool ChangeStatus(int id)
        {
            var ad = db.News.Find(id);
            ad.Status = !ad.Status;
            db.SaveChanges();
            return ad.Status;
        }
    }
}
