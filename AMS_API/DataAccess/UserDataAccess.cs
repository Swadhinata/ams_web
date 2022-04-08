using AMS_API.Models;
using System.Data;
using System.Data.SqlClient;

namespace AMS_API.DataAccess
{
    public partial class UserDataAccess
    {
        private string _connectionString { get; set; }

        public UserDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public User GetUser(User user)
        {
            User userResult = null;
            DataTable userDt = null;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _connectionString;
                conn.Open();
                try
                {
                    userDt = new DataTable();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = authnticationSqlQuery;
                    cmd.Parameters.AddWithValue("@emailId", user.EmailId);
                    cmd.Parameters.AddWithValue("@password", user.Password);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    
                    adapter.Fill(userDt);

                    if(userDt.Rows.Count > 0)
                    {
                        userResult = new User();
                        userResult.UserId = Convert.ToInt32(userDt.Rows[0][0]);
                        userResult.EmailId = (userDt.Rows[0][1]).ToString();
                        userResult.Role.RoleId = Convert.ToInt32(userDt.Rows[0][3]);
                        userResult.FirstName = (userDt.Rows[0][4]).ToString();
                        userResult.MiddleName = (userDt.Rows[0][5]).ToString();
                        userResult.LastName = (userDt.Rows[0][6]).ToString();
                        userResult.ProfilePhoto = (userDt.Rows[0][7]).ToString();
                        userResult.ContactNo = (userDt.Rows[0][8]).ToString();
                        userResult.RMId = Convert.ToInt32(userDt.Rows[0][9]);
                        userResult.Role.RoleName=(string)userDt.Rows[0][12]; 


                        //userResult.UserName = (userDt.Rows[0][3]).ToString();
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    conn.Close();
                }                
            }
            return userResult;
        }
    }
}
