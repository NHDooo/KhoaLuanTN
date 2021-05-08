using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dao
{
   public class CommentDao
    {
        MovieProjectDbcontext db = null;
        public CommentDao()
        {
            db = new MovieProjectDbcontext();
        }
      

        public bool Delete(int id)
        {
            try
            {
                var user = db.Comments.Find(id);
                db.Comments.Remove(user);
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
            var ad = db.Comments.Find(id);
            ad.Status = !ad.Status;
            db.SaveChanges();
            return ad.Status;
        }
    }
}