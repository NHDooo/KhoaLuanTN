using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dao
{
    public class ContactDao
    {
        MovieProjectDbcontext db = null;
        public ContactDao()
        {
            db = new MovieProjectDbcontext();
        }
        public IEnumerable<Contact> ListPg(int page, int pageSize)
        {
            return db.Contacts.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }
        public Contact GetContentContact()
        {
            return db.Contacts.FirstOrDefault(x => x.Status == true);
        }
        public int InsertFeedBack(Feedback fb)
        {
            db.Feedbacks.Add(fb);
            db.SaveChanges();
            return fb.ID;
        }
        public bool Delete(int id)
        {
            try
            {
                var user = db.Contacts.Find(id);
                db.Contacts.Remove(user);
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
            var ad = db.Contacts.Find(id);
            ad.Status = !ad.Status;
            db.SaveChanges();
            return ad.Status;
        }
    }
}