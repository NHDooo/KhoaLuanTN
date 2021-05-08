using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.EF;
using PagedList;

namespace Models.Dao
{
    class Class1
    {
        MovieProjectDbcontext db = null;

        public Class1()
        {
            db = new MovieProjectDbcontext();
        }
        public IEnumerable<Movie> ListM(int page, int pagesize)
        {
            return db.Movies.OrderByDescending(x => x.MovieID).ToPagedList(page, pagesize);
        }
        // lay nhung bo phim moi
        public List<Movie> ListMovieNew(int top)
        {
            return db.Movies.Where(x => x.Status == true).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }
        // lay nhưng bo phim co nhieu luot xem
        public List<Movie> ListHotMovie(int top)
        {
            return db.Movies.Where(x => x.Status == true).OrderByDescending(x => x.Viewed).Take(top).ToList();
        }
        //
        public List<Movie> ListMoviePi(int top)
        {
            return db.Movies.Where(x => x.Status == true).OrderByDescending(x => x.Viewed & x.Rate).Take(top).ToList();
        }    
        // nhung bo phim lien quan
        public List<Movie> ListMovieRelated(int movieid,int top)
        {
            var movie = db.Movies.Find(movieid);
            return db.Movies.Where(x => x.MovieID != movieid && x.CategoryID == movie.CategoryID).OrderByDescending(y => y.CreatedDate).Take(top).ToList();
        }
        // danh sach theo the loai
        public List<Movie> ListByCated(long cate)
        {
            return db.Movies.Where(x => x.CategoryID == cate).OrderByDescending(x => x.CreatedDate).ToList();
        }
        // theo quoc gia
        public List<Movie> ListByCountry(long country)
        {
            return db.Movies.Where(x => x.CategoryID == country).OrderByDescending(x => x.CreatedDate).ToList();
        }
        // tim theo en
        public List<Movie> SearchByKey(int key)
        {
            return db.Movies.SqlQuery("Select * from Movie where Name like '%" + key + "%'").ToList();
        }
        // xoa phim
        public bool DeleteMovie(int id)
        {
            try
            {
                var user = db.Movies.Find(id);
                db.Movies.Remove(user);
                db.SaveChanges();
                return true;

            }
            catch (Exception)
            {
                return false;
            }

        }
        // thay doi trang thai
        public bool ChangeStatus(int id)
        {
            var ad = db.Movies.Find(id);
            ad.Status = !ad.Status;
            return ad.Status;
        }
    }
}
