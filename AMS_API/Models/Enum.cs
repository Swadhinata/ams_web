using System.ComponentModel.DataAnnotations;

namespace AMS_API.Models
{
    //public enum Role
    //{
    //    User = 1,
    //    Admin = 2,
    //    SuperAdmin = 3,
    //    [Display(Name = "IT Admin")]
    //    ITAdmin = 4
    //}

    public enum Department
    {
        Operation = 1,
        HR = 2,
        IT = 3,
        Finance = 4,
        SaleAndMarketing=5,
        Admin=6
    }

    public enum RStatus
    {
        Approved = 1,
        New = 2,
        Allocated = 3,
        Cancelled = 4
    }
    
}
