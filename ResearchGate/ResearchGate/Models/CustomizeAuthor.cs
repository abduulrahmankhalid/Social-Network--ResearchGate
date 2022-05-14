using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace ResearchGate.Models
{
    [MetadataType (typeof(AuthorMetaData))]

    // add methods new properties
    public partial class Author
    {       
        [Required]
        [Compare("Password", ErrorMessage ="Passwords Do Not Match")]
        [Display(Name = "Confirm Passward")]
        public string ConfPass { get; set; }
    }

    // Edit Author Properties  

    public  class AuthorMetaData
    {
        
        public int AuthorID { get; set; }

        [Required]   //right email format
        [RegularExpression( @"\b[\w\.-]+@[\w\.-]+\.\w{2,4}\b", ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; }

        [Required]  // at least 8 charactersmust contain at least 1 uppercase letter, 1 lowercase letter, and 1 number- Can contain special characters
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$", ErrorMessage = "Password Is Not A Strong Password" )]
        public string Password { get; set; }

        [Required] //only text
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Invalid Name")]
        [Display(Name = "First Name" )]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$" , ErrorMessage ="Invalid Name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string University { get; set; }

        [Required]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Invalid Name")]
        [DataType(DataType.Text)]
        public string Department { get; set; }

        [Required]
        [RegularExpression(@"^\s*(?:\+?(\d{1,3}))?([-. (]*(\d{3})[-. )]*)?((\d{3})[-. ]*(\d{2,4})(?:[-.x ]*(\d+))?)\s*$", ErrorMessage ="Invalid Mobile Number" )]
        public string Mobile { get; set; }

        
        [Display(Name = "Profile Image")]
        public string ProfImage { get; set; }

    }
}