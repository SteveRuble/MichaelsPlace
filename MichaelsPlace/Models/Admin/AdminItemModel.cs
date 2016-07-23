using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MichaelsPlace.Models.Admin
{
    public class AdminItemModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Content { get; set; }

        [Required]
        public int Order { get; set; }

        [Display(Name = "Created At")]
        [ReadOnly(true)]
        public DateTimeOffset CreatedUtc { get; set; }

        [Display(Name = "Created By")]
        [ReadOnly(true)]
        public string CreatedBy { get; set; }

        public List<int> ContextTagIds { get; set; }
        public List<int> RelationshipTagIds { get; set; }
        public List<int> LossTagIds { get; set; }
    }

    public class AdminArticleModel : AdminItemModel
    {
        
    }
    
    public class AdminToDoModel : AdminItemModel
    {
        
    }


    public class OrderingModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
    }
}
