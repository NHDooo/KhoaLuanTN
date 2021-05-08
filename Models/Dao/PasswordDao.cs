using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.EF;

namespace Models.Dao
{
    public class PasswordDao
    {
        MovieProjectDbcontext db = null;

        public PasswordDao()
        {
            db = new MovieProjectDbcontext();
        }

        public int UpdateUser(User us)
        {
            db.Users.Add(us);
            db.SaveChanges();
            return us.UserID; 
        }
        public int ChangePass(string username, string password)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == username);
            if(result == null)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

    }
}
