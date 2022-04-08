using System.Text.Json.Serialization;

namespace AMS_API.Models
{
    public class Request
    {
        public int? RequestId { get; set; }

        public Asset Asset { get; set; }

        public User RequestedBy   { get; set; }

        public DateTime RequestedOn { get; set; }

        public string Reason  { get; set; }
        public  DateTime ExpectedDate { get; set; }
        public User RequestedFor { get; set; }
        public Status Status { get; set; }

        public Request()
        {
            Asset = new Asset();
            RequestedBy = new User();
            RequestedFor = new User();
            Status = new Status();
        }

    }
}
