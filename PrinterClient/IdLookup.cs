using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterClient
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class HardwareModel
    {
        public int type { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public object description { get; set; }
        public object tag { get; set; }
        public List<object> tags { get; set; }
    }

    public class OperatingSystem
    {
        public bool isTemplate { get; set; }
        public object parentOperatingSystemId { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public object tag { get; set; }
        public List<object> tags { get; set; }
    }

    public class Result
    {
        public object activeDirectoryOperatingSystemName { get; set; }
        public object activeDirectoryOperatingSystemVersion { get; set; }
        public HardwareModel hardwareModel { get; set; }
        public OperatingSystem operatingSystem { get; set; }
        public string displayName { get; set; }
        public int type { get; set; }
        public string securityIdentifier { get; set; }
        public bool hasGeneralMessage { get; set; }
        public string generalMessage { get; set; }
        public bool isActive { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public object tag { get; set; }
        public List<object> tags { get; set; }
    }

    public class Links
    {
        public object next { get; set; }
        public object nextSkip { get; set; }
        public object nextTake { get; set; }
        public object prev { get; set; }
        public object prevSkip { get; set; }
        public object prevTake { get; set; }
    }

    public class IdLookup
    {
        public List<Result> result { get; set; }
        public int totalCount { get; set; }
        public Links links { get; set; }
    }
}
