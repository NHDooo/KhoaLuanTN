using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Models.Dao
{
    public class PermissionDao
    {
        MovieProjectDbcontext db = null;
        public PermissionDao()
        {
            db = new MovieProjectDbcontext();
        }
        public IEnumerable<Permission> ListPg(int page, int pageSize)
        {
            return db.Permissions.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }
        public bool Delete(int id)
        {
            try
            {
                var user = db.Permissions.Find(id);
                db.Permissions.Remove(user);
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