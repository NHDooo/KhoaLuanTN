using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Models.Dao
{
    public class MovieDao
    {
        MovieProjectDbcontext db = null;
        public MovieDao()
        {
            db = new MovieProjectDbcontext();
        }
        /// <summary>
        /// Phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<Movie> ListM(int page, int pageSize)
        {

            return db.Movies.OrderByDescending(x => x.MovieID).ToPagedList(page, pageSize);
        }
        /// <summary>
        /// Lấy những bộ phim mới
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<Movie> ListMovieNew(int top)
        {
            return db.Movies.Where(x => x.Status == true).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }
        /// <summary>
        /// Lấy những bộ phim top lượt xem
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<Movie> ListMovieTop(int top)
        {
         

            return db.Movies.Where(x => x.Status == true).OrderByDescending(x => x.Viewed).Take(top).ToList();
        }
        /// <summary>
        /// Lấy những bộ phim phổ biến
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public List<Movie> ListMoviePo(int top)
        {
            return db.Movies.Where(x => x.Status == true).OrderByDescending(x => x.Viewed & x.Rate).Take(top).ToList();
        }
        /// <summary>
        /// Lấy những bộ phim cùng thể loại
        /// </summary>
        /// <param name="movieid"></param>
        /// <returns></returns>
        public List<Movie> ListMovieRelated(int movieid, int top)
        {
            var movie = db.Movies.Find(movieid);
            return db.Movies.Where(x => x.MovieID != movieid && x.CategoryID == movie.CategoryID).OrderByDescending(y => y.CreatedDate).Take(top).ToList();
        }
        /// <summary>
        /// Lấy những bộ phim liên quan
        /// </summary>
        /// <param name="movieid"></param>
        /// <returns></returns>
        public List<Movie> ListMovieNew1(int top)
        {

            return db.Movies.Where(x => x.Status == true).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }
        /// <summary>
        /// Lấy phim bằng catetoryID
        /// </summary>
        /// <param name="cateID"></param>
        /// <returns></returns>
        public List<Movie> ListByCateId(long cateID)
        {

            return db.Movies.Where(x => x.CategoryID == cateID).OrderByDescending(x => x.CreatedDate).ToList();

        }
        public int GetData(int data)
        {
            return db.Movies.Where(x => x.Viewed == data).Count();
        }
        public List<Movie> ListByCountryID(long countryID)
        {

            return db.Movies.Where(x => x.CountryID == countryID).OrderByDescending(x => x.CreatedDate).ToList();

        }

        public List<Movie> SearchByKey(string key)
        {
            return db.Movies.SqlQuery("Select * from Movie where Name like '%"+key+"%'").ToList();
        }
        public IEnumerable<Movie> SearchByAny(string searchString, int page, int pageSize)
        {
            IOrderedQueryable<Movie> model = db.Movies;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString)).OrderByDescending(x => x.CreatedDate);
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public Movie ViewDetail(int id)
        {
            return db.Movies.Find(id);
        }
        public bool Delete(int id)
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
        public bool ChangeStatus(int id)
        {
            var ad = db.Movies.Find(id);
            ad.Status = !ad.Status;
            db.SaveChanges();
            return ad.Status;
        }
    }
}