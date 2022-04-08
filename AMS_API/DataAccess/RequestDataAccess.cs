using AMS_API.Models;
using System.Data;
using System.Data.SqlClient;

namespace AMS_API.DataAccess
{
    public class RequestDataAccess
    {
        private string _connectionString { get; set; }

        public RequestDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Request GetRequest(int requestId)
        {
            Request request = null;
            DataTable requestDt = null;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _connectionString;
                conn.Open();
                try
                {
                    requestDt = new DataTable();
                    SqlCommand cmd = conn.CreateCommand();
                    string sqlQuery = @"SELECT TR.RequestId, TA.AssetId, TA.AssetName, TAC.AssetCategoryId, TAC.AssetCategory, TU.UserId, TU.Name, TR.CreatedOn, TR.IsResolved
                                        FROM tbl_Requests TR
                                        JOIN tbl_Assets TA ON TR.AssetId = TA.AssetId
                                        JOIN tbl_AssetCategory TAC ON TA.AssetCategoryId = TAC.AssetCategoryId
                                        JOIN tbl_Users TU ON TR.CreatedBy = TU.UserId
                                        WHERE TR.RequestId = @requestId";
                    cmd.CommandText = sqlQuery;
                    cmd.Parameters.AddWithValue("@requestId", requestId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    adapter.Fill(requestDt);

                    if (requestDt.Rows.Count > 0)
                    {
                        request = new Request();
                        request.RequestId = Convert.ToInt32(requestDt.Rows[0][0]);

                        request.Asset.AssetId = Convert.ToInt32(requestDt.Rows[0][1]);
                        request.RequestedBy.UserId = Convert.ToInt32(requestDt.Rows[0][2]);

                        request.RequestedOn = Convert.ToDateTime(requestDt.Rows[0][3]);
                        request.Reason = (requestDt.Rows[0][4]).ToString();



                        request.ExpectedDate = (DateTime)(requestDt.Rows[0][5]);
                        request.RequestedFor.UserId = Convert.ToInt32(requestDt.Rows[0][6]);

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
            return request;
        }

        public IList<Request> GetRequests()
        {
            List<Request> requests = null;
            DataTable requestDt = null;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _connectionString; 
                conn.Open();
                try
                {
                    requestDt = new DataTable();
                    SqlCommand cmd = conn.CreateCommand();
                    string sqlQuery = @"SELECT TR.RequestId, TU.UserId, TU.FirstName,TU.LastName,TR.AssetId, TAT.AssetTypeName, TAC.AssetCategory,TR.RequestedOn, TS.Status
                                        FROM tbl_Requests TR
                                        JOIN tbl_Assets TA ON TR.AssetId = TA.AssetId
										JOIN tbl_AssetType TAT ON TA.AssetTypeId=TAT.AssetTypeId
                                        JOIN tbl_AssetCategory TAC ON TAC.AssetCategoryId = TAT.AssetCategoryId
                                        JOIN tbl_Request_Status TRS ON TR.RequestId=TRS.RequestId
                                        JOIN tbl_Status TS ON TRS.StatusId=TS.StatusId
                                        JOIN tbl_Users TU ON TR.RequestedBy = TU.UserId";
                    cmd.CommandText = sqlQuery;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    adapter.Fill(requestDt);

                    if (requestDt.Rows.Count > 0)
                    {
                        requests = new List<Request>();
                        foreach (DataRow row in requestDt.Rows)
                        {
                            var request = new Request();

                            request.RequestId = Convert.ToInt32(row[0]);                            
                            request.RequestedBy.UserId = Convert.ToInt32(row[1]);
                            request.RequestedBy.FirstName = row[2].ToString();
                            request.RequestedBy.LastName = row[3].ToString();
                            request.Asset.AssetId = Convert.ToInt32(row[4]);
                            request.Asset.AssetType.AssetTypeName= row[5].ToString();

                            //request.Asset.AssetType.AssetCategory.AssetCategoryId = Convert.ToInt32(row[3]);
                            request.Asset.AssetType.AssetCategory.AssetCategoryName = (row[6]).ToString();
                            request.RequestedOn = (DateTime)row[7];
                            request.Status.StatusName = row[8].ToString();

                            //request.User.UserId = Convert.ToInt32(row[5]);
                            //request.User.FirstName = (row[6]).ToString();

                            //request.CreatedOn = Convert.ToDateTime(row[7]);
                            //request.IsResolved = Convert.ToBoolean(row[8] == DBNull.Value ? false : row[8]);

                            requests.Add(request);
                        }

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
            return requests;
        }

        public int InsertRequest(Request request)
        {
            int noOfRowsInserted = 0;
            int result;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _connectionString;
                try
                {
                    string sqlQuery = @"Insert into tbl_Requests(AssetId,RequestedBy,ExpextedDate,Reason,RequestedFor) values(@AssetId,@RequestedBy,@ExpextedDate,@Reason,@RequestedFor)";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, conn))
                    {
                        cmd.CommandText = sqlQuery;
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@AssetId", request.Asset.AssetId);
                        cmd.Parameters.AddWithValue("@RequestedBy", request.RequestedBy.UserId);
                        cmd.Parameters.AddWithValue("@ExpextedDate", request.ExpectedDate);
                        cmd.Parameters.AddWithValue("@Reason", request.Reason);
                        cmd.Parameters.AddWithValue("@RequestedFor", request.RequestedFor.UserId);

                        conn.Open();
                        result = cmd.ExecuteNonQuery();
                    }



                    if (result != 0)
                    {
                        var requestStatus = new RequestStatus();
                        requestStatus.Request.RequestId = result;
                        requestStatus.Status.StatusId = (int)RStatus.New;
                        noOfRowsInserted = InsertRequestStatus(requestStatus);
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
            return noOfRowsInserted;
        }

        public int InsertRequestStatus(RequestStatus status)
        {
            int noOfRowsInserted = 0;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _connectionString;
                try
                {
                    SqlCommand cmd = conn.CreateCommand();
                    string sqlQuery = @"INSERT INTO tbl_Request_Status(RequestId, StatusId) VALUES(@requestId, @statusId)";
                    cmd.CommandText = sqlQuery;
                    cmd.Parameters.AddWithValue("@requestId", status.Request.RequestId);
                    cmd.Parameters.AddWithValue("@statusId", status.Status.StatusId);

                    conn.Open();
                    noOfRowsInserted = cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {

                }
                finally
                {
                    conn.Close();
                }
            }
            return noOfRowsInserted;
        }

        public int UpdateRequest(Request request)
        {
            int noOfRowsUpdated = 0;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _connectionString;
                try
                {
                    SqlCommand cmd = conn.CreateCommand();
                    string sqlQuery = @"UPDATE tbl_Requests SET AssetId = @assetId, CreatedBy = createdBy, IsResolved = @isResolved WHERE RequestId = @requestId";
                    cmd.CommandText = sqlQuery;
                    cmd.Parameters.AddWithValue("@assetId", request.Asset.AssetId);
                    cmd.Parameters.AddWithValue("@createdBy", request.RequestedBy.UserId);
                    //cmd.Parameters.AddWithValue("@isResolved", request.IsResolved);
                    cmd.Parameters.AddWithValue("@requestId", request.RequestId);

                    conn.Open();
                    noOfRowsUpdated = cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {

                }
                finally
                {
                    conn.Close();
                }
            }
            return noOfRowsUpdated;
        }

        public int DeleteRequest(int requestId)
        {
            int noOfRowsDeleted = 0;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _connectionString;
                try
                {
                    SqlCommand cmd = conn.CreateCommand();
                    string sqlQuery = @"DELETE FROM tbl_Requests WHERE RequestId = @requestId; DELETE FROM tbl_Request_Status WHERE RequestId = @requestId";
                    cmd.CommandText = sqlQuery;
                    cmd.Parameters.AddWithValue("@requestId", requestId);

                    conn.Open();
                    noOfRowsDeleted = cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {

                }
                finally
                {
                    conn.Close();
                }
            }
            return noOfRowsDeleted;
        }

        public IList<AssetCategory> GetAssetCategory()
        {
            List<AssetCategory> categories = null;
            DataTable categoryDt = null;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _connectionString;
                conn.Open();
                try
                {
                    categoryDt = new DataTable();
                    SqlCommand cmd = conn.CreateCommand();
                    string sqlQuery = @"SELECT * FROM tbl_AssetCategory";
                    cmd.CommandText = sqlQuery;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    adapter.Fill(categoryDt);

                    if (categoryDt.Rows.Count > 0)
                    {
                        categories = new List<AssetCategory>();
                        foreach (DataRow row in categoryDt.Rows)
                        {
                            var category = new AssetCategory();

                            category.AssetCategoryId = Convert.ToInt32(row[0]);
                            category.AssetCategoryName = (row[1]).ToString();

                            categories.Add(category);
                        }

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
            return categories;
        }

        public IList<Asset> GetAsset(int assetTypeId)
        {
            List<Asset> assets = null;
            DataTable assetDt = null;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _connectionString;
                conn.Open();
                try
                {
                    assetDt = new DataTable();
                    SqlCommand cmd = conn.CreateCommand();
                    string sqlQuery = @"SELECT * FROM tbl_Assets WHERE AssetTypeId = @assetTypeId";
                    cmd.CommandText = sqlQuery;
                    cmd.Parameters.AddWithValue("@assetTypeId", assetTypeId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    adapter.Fill(assetDt);

                    if (assetDt.Rows.Count > 0)
                    {
                        assets = new List<Asset>();
                        foreach (DataRow row in assetDt.Rows)
                        {
                            var asset = new Asset();

                            asset.AssetId = Convert.ToInt32(row[0]);
                            asset.AssetName = (row[1]).ToString();

                            assets.Add(asset);
                        }

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
            return assets;
        }

        public IList<AssetType> GetAssetTypes(int AssetCategoryId)
        {
            List<AssetType> assets = null;
            DataTable assetDt = null;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _connectionString;
                conn.Open();
                try
                {
                    assetDt = new DataTable();
                    SqlCommand cmd = conn.CreateCommand();
                    string sqlQuery = @"SELECT * FROM tbl_AssetType WHERE AssetCategoryId = @assetCategoryId";
                    cmd.CommandText = sqlQuery;
                    cmd.Parameters.AddWithValue("@assetCategoryId", AssetCategoryId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    adapter.Fill(assetDt);

                    if (assetDt.Rows.Count > 0)
                    {
                        assets = new List<AssetType>();
                        foreach (DataRow row in assetDt.Rows)
                        {
                            var asset = new AssetType();

                            asset.AssetTypeId = Convert.ToInt32(row[0]);
                            asset.AssetTypeName = (row[1]).ToString();

                            assets.Add(asset);
                        }

                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    conn.Close();
                }
                return assets;
            }
        }
    }
}
