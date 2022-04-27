using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations; 
namespace ResearchGate.Models
{ 
    [MetadataType(typeof(PaperMetaData))]
    public partial class Paper
    {
        // Add Methods to Paper or Add new property
    }

    public class PaperMetaData
    {
        public int PaperID { get; set; }
        public string Title { get; set; }

        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Date { get; set; }
        public string Abstract { get; set; }
        public string Image { get; set; }
        public Nullable<int> Likes { get; set; }
        public Nullable<int> Dislikes { get; set; }

    }
}