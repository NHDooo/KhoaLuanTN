using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dao
{
   public class FeedBackDao
    {

        MovieProjectDbcontext db = null;
        public FeedBackDao()
        {
            db = new MovieProjectDbcontext();
        }
        public IEnumerable<Feedback> ListPg(int page, int pageSize)
        {
            return db.Feedbacks.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }
       
        public bool Delete(int id)
        {
            try
            {
                var user = db.Feedbacks.Find(id);
                db.Feedbacks.Remove(user);
                db.SaveChanges();
                return true;

            }
            catch (Exception)
            {
                return false;
            }

        }
      
    }
}