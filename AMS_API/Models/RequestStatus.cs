namespace AMS_API.Models
{
    public class RequestStatus
    {
        public int RequestStatusId { get; set; }
        public Request Request { get; set; }
        public Status Status { get; set; }
        public DateTime AllocatedDateTime { get; set; }
        public User User { get; set; }
        public DateTime ApprovedDate { get; set; }
        public User ApprovedBy { get; set; }

        public RequestStatus()
        {
            Request=new Request();
            Status=new Status();
            User=new User();
            ApprovedBy=new User();
        }

    }
}
