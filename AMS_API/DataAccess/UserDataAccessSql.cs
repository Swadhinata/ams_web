namespace AMS_API.DataAccess
{
    public partial class UserDataAccess
    {
        private  string authnticationSqlQuery = @"SELECT * FROM tbl_Users tu inner join tbl_Roles tr on tu.RoleId=tr.RoleId  WHERE EmailId=@emailId and Password=@password";
    }
}
