namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Movie")]
    public partial class Movie
    {
        public int MovieID { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        public string Image { get; set; }

        [Column(TypeName = "xml")]
        public string MoreImages { get; set; }

        [StringLength(50)]
        public string Actor { get; set; }

        public string Description { get; set; }

        [StringLength(50)]
        public string Directors { get; set; }

        [Column(TypeName = "text")]
        public string Time { get; set; }

        public int? Year { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        public string MovieLink { get; set; }

        public string TrailerLink { get; set; }

        public int? CategoryID { get; set; }

        public int? Rate { get; set; }

        public int? TrailerID { get; set; }

        public int? Viewed { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        [StringLength(250)]
        public string MetaKeywords { get; set; }

        [StringLength(250)]
        public string MetaDescriptions { get; set; }

        public bool Status { get; set; }

        public DateTime? TopHot { get; set; }

        public int? CountryID { get; set; }
    }
}
