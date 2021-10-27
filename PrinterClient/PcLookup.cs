using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterClient
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Variable
    {
        public int id { get; set; }
        public int deploymentRoleId { get; set; }
        public string createdDate { get; set; }
        public string lastModifiedDate { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public int dataType { get; set; }
    }

    public class Role
    {
        public int id { get; set; }
        public string name { get; set; }
        public string identifier { get; set; }
        public int type { get; set; }
        public int categoryId { get; set; }
        public List<Variable> variables { get; set; }
        public string description { get; set; }
        public bool isInternal { get; set; }
        public string createdDate { get; set; }
        public string lastModifiedDate { get; set; }
    }

    public class Image
    {
        public string displayName { get; set; }
        public string packageId { get; set; }
        public int index { get; set; }
        public object version { get; set; }
        public object id { get; set; }
        public object name { get; set; }
        public object description { get; set; }
        public object tag { get; set; }
        public List<object> tags { get; set; }
    }

    public class Metadata
    {
        public List<object> primaryUsers { get; set; }
        public Image image { get; set; }
    }

    public class Deployment
    {
        public int targetId { get; set; }
        public object targetName { get; set; }
        public object deploymentTemplateId { get; set; }
        public object deploymentTemplate { get; set; }
        public bool cleanupResources { get; set; }
        public string createdDate { get; set; }
        public string createdBy { get; set; }
        public string executionDate { get; set; }
        public int installationType { get; set; }
        public bool isUpgrade { get; set; }
        public Metadata metadata { get; set; }
    }

    public class PcLookup
    {
        public int id { get; set; }
        public string securityIdentifier { get; set; }
        public string displayName { get; set; }
        public string distinguishedName { get; set; }
        public string name { get; set; }
        public string createdDate { get; set; }
        public string lastModifiedDate { get; set; }
        public string lastModifiedBy { get; set; }
        public string source { get; set; }
        public bool isActive { get; set; }
        public int type { get; set; }
        public string description { get; set; }
        public string assetTag { get; set; }
        public string uuid { get; set; }
        public string serialNumber { get; set; }
        public string macAddress { get; set; }
        public List<Role> roles { get; set; }
        public List<object> primaryUsers { get; set; }
        public object managedBy { get; set; }
        public Deployment deployment { get; set; }
        public bool isMt { get; set; }
        public int hardwareModelId { get; set; }
        public int operatingSystemId { get; set; }
        public OperatingSystem operatingSystem { get; set; }
        public List<object> owners { get; set; }
        public bool hasGeneralMessage { get; set; }
        public string generalMessage { get; set; }
        public object activeDirectoryOperatingSystemName { get; set; }
        public object activeDirectoryOperatingSystemVersion { get; set; }
        public List<object> tags { get; set; }
    }

}
