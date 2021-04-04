using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiUrbantz.Models
{
    public class Location
    {
        public string type { get; set; }
        public IList<object> geometry { get; set; }
    }

    public class Source
    {
        public Location location { get; set; }
        public IList<object> addressLines { get; set; }
        public int geocodeScore { get; set; }
        public int cleanScore { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string address { get; set; }
    }

    public class NotificationSettings
    {
        public bool sms { get; set; }
        public bool email { get; set; }
    }

    public class Dispatcher
    {
        public bool scan { get; set; }
    }

    public class Driver
    {
        public bool prepCheckList { get; set; }
        public bool prepScan { get; set; }
        public bool signatureAndComment { get; set; }
        public bool signatureAndItemConcerns { get; set; }
        public bool signature { get; set; }
        public bool scan { get; set; }
        public bool comment { get; set; }
        public bool photo { get; set; }
        public bool contactless { get; set; }
    }

    public class Failure
    {
        public bool photo { get; set; }
    }

    public class DropOff
    {
        public Driver driver { get; set; }
        public Stop stop { get; set; }
    }

    public class Requires
{
    public Dispatcher dispatcher { get; set; }
    public Driver driver { get; set; }
    public Stop stop { get; set; }
    public Failure failure { get; set; }
    public DropOff dropOff { get; set; }
}

    public class Arrive
{
    public Location location { get; set; }
public DateTime when { get; set; }
public bool forced { get; set; }
    }

    public class ActualTime
{
    public Arrive arrive { get; set; }
}

   public class Contactless
{
    public bool forced { get; set; }

}

   public class Timer
{
    public IList<object> timestamps { get; set; }
}

   public class Position
{
    public double accuracy { get; set; }
    public double heading { get; set; }
    public double altitude { get; set; }
    public double latitude { get; set; }
    public double longitude { get; set; }
    public double speed { get; set; }
    public long timestamp { get; set; }
}

    public class FailedReason
    {
        public string reason { get; set; }
        public string reasonId { get; set; }
        public string externalId { get; set; }
        public string picture { get; set; }
        public string custom { get; set; }
    }
    public class Execution
{
    public FailedReason failedReason { get; set; }
    public Contactless contactless { get; set; }
    public Timer timer { get; set; }
    public Position position { get; set; }
    public string successComment { get; set; }
    public string successPicture { get; set; }
    public  Signature signature { get; set; }
    public Execution()
        {
            failedReason = new FailedReason();
            contactless = new Contactless();
            timer = new Timer();
            position = new Position();

        }
    }

   public class Collect
{
    public bool activated { get; set; }
}

   public class Assets
{
    public IList<object> deliver { get; set; }
    public IList<object> returnn { get; set; }
    }




     public class TaskChangedModel
    {
    public Source source { get; set; }
    public Location location { get; set; }
    public NotificationSettings notificationSettings { get; set; }
    public Contact contact { get; set; }
    public Requires requires { get; set; }
    public Delay delay { get; set; }
    public TimeWindow timeWindow { get; set; }
    public ActualTime actualTime { get; set; }
    public Execution execution { get; set; }
    public Collect collect { get; set; }

    public Assets assets { get; set; }
      
    public string status { get; set; }
    public string activity { get; set; }
    public IList<object> skills { get; set; }
    public IList<string> labels { get; set; }
    public int attempts { get; set; }
    public int numberOfPlannings { get; set; }
    public int timeWindowMargin { get; set; }
    public int optimizationCount { get; set; }
    public object hasBeenPaid { get; set; }
    public DateTime closureDate { get; set; }
    public object lastOfflineUpdatedAt { get; set; }
    public bool replanned { get; set; }
    public bool archived { get; set; }
    public bool setToInvoice { get; set; }
    public object paymentType { get; set; }
    public double collectedAmount { get; set; }
    public IList<object> categories { get; set; }
    public Metadata metadata { get; set; }
   // public IList<Log> log { get; set; }
    public IList<object> issues { get; set; }
    public DateTime when { get; set; }
    public DateTime updated { get; set; }
    public IList<object> notifications { get; set; }
    public IList<object> attachments { get; set; }
    public IList<Item> items { get; set; }
    public IList<object> products { get; set; }
    public IList<object> returnedProducts { get; set; }
    public IList<object> customerCalls { get; set; }
    public IList<object> categories_ { get; set; }
    public string _id { get; set; }
    public DateTime date { get; set; }
    public string endpoint { get; set; }
    public string taskId { get; set; }
    public string type { get; set; }
    public string announcement { get; set; }
    public string announcementUpdate { get; set; }
    public string client { get; set; }
    public Dimensions dimensions{ get; set; }
    public string flux { get; set; }
    public bool hasRejectedProducts { get; set; }
    public string id { get; set; }
    public string imagePath { get; set; }
    public string instructions { get; set; }
    public string platform { get; set; }
    public string platformName { get; set; }
    public double price { get; set; }
    public string progress { get; set; }
    public int serviceTime { get; set; }
    public string trackingId { get; set; }
    public string hub { get; set; }
    public string hubName { get; set; }
    public string targetFlux { get; set; }
    public string order { get; set; }
    public DateTime arriveTime { get; set; }
    public object associated { get; set; }
    public object associatedName { get; set; }
    public string driver { get; set; }
    public int initialSequence { get; set; }
    public string optimizationGroup { get; set; }
    public string round { get; set; }
    public string roundColor { get; set; }
    public string roundName { get; set; }
    public int sequence { get; set; }
    public int __v { get; set; }
    public DateTime timestamp { get; set; }
    public string taskReference { get; set; }
        //constructeur
        public TaskChangedModel()
        {
            source = new Source();
            location = new Location();
            notificationSettings = new NotificationSettings();
            contact = new Contact();
            requires = new Requires();
            delay = new Delay();
            timeWindow = new TimeWindow();
            actualTime = new ActualTime();
            execution = new Execution();
            collect = new Collect();
            assets = new Assets();
            metadata = new Metadata();
            dimensions = new Dimensions();
            items = new List<Item>();
        }
    }

    public class ListTaskChanged
    {
        public List<TaskChangedModel> ListTaskChangedModel { get; set; }

    }

}