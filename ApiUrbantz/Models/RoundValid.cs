using ApiUrbantz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiUrbantz.Models
{
    public class Delay
    {
        public int time { get; set; }
        public DateTime when { get; set; }
    }

    public class Stop
    {
        public IList<double> coordinates { get; set; }
        public int travelTime { get; set; }
        public int serviceTime { get; set; }
        public int waitTime { get; set; }
        public string violationTime { get; set; }
        public int stopType { get; set; }
        public int stopSequence { get; set; }
        public double travelDistance { get; set; }
        public DateTime arriveTime { get; set; }
        public DateTime departTime { get; set; }
        public string type { get; set; }
        public string taskId { get; set; }
        public string parcel { get; set; }
        public int? sequence { get; set; }
        public bool onSite { get; set; }
        public string taskReference { get; set; }
    }

    public class Reloading
    {
        public bool enabled { get; set; }
        public int time { get; set; }
    }

    public class VehicleData
    {
        public Reloading reloading { get; set; }
        public bool custom { get; set; }
        public string type { get; set; }
        public bool breaks { get; set; }
        public IList<string> labels { get; set; }
        public string _id { get; set; }
        public string platform { get; set; }
        public string name { get; set; }
        public int maxOrders { get; set; }
        public int maxDuration { get; set; }
        public int maxDistance { get; set; }
        public int accelerationTime { get; set; }
        public int fixedCost { get; set; }
        public int costPerUnitTime { get; set; }
        public double costPerUnitDistance { get; set; }
        public Dimensions dimensions { get; set; }
     public int ordersByHour { get; set; }
        public VehicleData()
        {
            dimensions = new Dimensions();
            reloading = new Reloading();

        }
    }

    public class RealInfo
{
    public object hasPrepared { get; set; }
    public object hasStarted { get; set; }
    public object hasFinished { get; set; }
    public object preparationTime { get; set; }
    public object hasLasted { get; set; }
}

    public class Round
{
    public string _id { get; set; }
    public Delay delay { get; set; }
    public IList<object> reloads { get; set; }
    public string activity { get; set; }
    public string status { get; set; }
    public int orderCount { get; set; }
    public double totalCost { get; set; }
    public int totalTime { get; set; }
    public int totalOrderServiceTime { get; set; }
    public int totalBreakServiceTime { get; set; }
    public int totalTravelTime { get; set; }
    public double totalDistance { get; set; }
    public IList<string> labelsAndSkills { get; set; }
    public int totalWaitTime { get; set; }
    public string  totalViolationTime { get; set; }
    public bool validated { get; set; }
    public string name { get; set; }
    public string startLocation { get; set; }
    public string endLocation { get; set; }
    public DateTime startTime { get; set; }
    public DateTime endTime { get; set; }
    public DateTime updated { get; set; }
    public IList<Stop> stops { get; set; }
    public Dimensions dimensions { get; set; }
    public DateTime date { get; set; }
    public string platform { get; set; }
    public string targetFlux { get; set; }
    public VehicleData vehicleData { get; set; }
    public Metadata metadata { get; set; }
    public RealInfo realInfo { get; set; }
        public Round()
        {
            vehicleData = new VehicleData();
            stops = new List<Stop>();
            realInfo = new RealInfo();
            metadata = new Metadata();
            delay = new Delay();

        }
}

    public class RoundValid
    {
        public string type { get; set; }
        public Round round { get; set; }
        public string eventType { get; set; }
        public DateTime timestamp { get; set; }
        public RoundValid()
        {
            round = new Round();
        }
            
}
    public class ListRoundValidated
    {
        public List<RoundValid> ListeRoundValid { get; set; }
    }


}