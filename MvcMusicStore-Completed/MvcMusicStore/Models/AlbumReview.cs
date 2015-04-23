using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MvcMusicStore.Models
{
    public class AlbumReview
    {
        [Key]
        public int AlbumReviewId { get; set; }

        public int AlbumId { get; set; }
        
        [AllowHtml]
        public string Description { get; set; }
        
        [ForeignKey("AlbumId")]
        public virtual Album Album { get; set; }
    }
}
