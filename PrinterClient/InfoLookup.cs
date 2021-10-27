using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrinterClient
{
    public class Application
    {
        public string type { get; set; }
        public bool isMappedToTargets { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public object description { get; set; }
        public object tag { get; set; }
        public List<object> tags { get; set; }
    }

    public class InstalledSystem
    {
        public string type { get; set; }
        public int targetId { get; set; }
        public string installedDate { get; set; }
        public List<Application> applications { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public object tag { get; set; }
        public List<object> tags { get; set; }
    }

    public class InstalledApplication
    {
        public bool hasLicense { get; set; }
        public bool showLicenseDescriptionOnInstallation { get; set; }
        public bool showLicenseDescriptionOnUninstallation { get; set; }
        public bool isActive { get; set; }
        public bool isDiscontinued { get; set; }
        public string installationDate { get; set; }
        public string uninstallationDate { get; set; }
        public bool hasGeneralMessage { get; set; }
        public object generalMessage { get; set; }
        public object licenseDescription { get; set; }
        public bool showGeneralMessageOnInstallation { get; set; }
        public bool showGeneralMessageOnUninstallation { get; set; }
        public string type { get; set; }
        public bool isMappedToTargets { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public object tag { get; set; }
        public List<object> tags { get; set; }
    }

    public class SccmSoftware
    {
        public string installDate { get; set; }
        public string installedLocation { get; set; }
        public string installSource { get; set; }
        public string packageCode { get; set; }
        public string productName { get; set; }
        public string productVersion { get; set; }
        public string publisher { get; set; }
        public string uninstallString { get; set; }
        public string name { get; set; }
    }

    public class ApplicationDeployment
    {
        public string softwareName { get; set; }
        public string collectionId { get; set; }
        public string collectionDisplayId { get; set; }
        public int configurationItemId { get; set; }
        public string resourceName { get; set; }
        public int enforcementState { get; set; }
        public string enforcementStateMessage { get; set; }
        public string startTime { get; set; }
        public bool isUser { get; set; }
        public int deploymentIntent { get; set; }
    }

    public class TargetInformation
    {
        public int id { get; set; }
        public string securityIdentifier { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }
        public string distinguishedName { get; set; }
        public string createdDate { get; set; }
        public string lastModifiedDate { get; set; }
        public string lastModifiedBy { get; set; }
        public string source { get; set; }
        public bool isActive { get; set; }
        public int type { get; set; }
        public object tag { get; set; }
        public List<object> tags { get; set; }
    }

    public class TargetIdentity
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public object securityIdentifer { get; set; }
        public string distinguishedName { get; set; }
        public string assetTag { get; set; }
        public string uuid { get; set; }
        public string serialNumber { get; set; }
        public string macAddress { get; set; }
        public List<object> roles { get; set; }
        public bool isMt { get; set; }
        public int hardwareModelId { get; set; }
        public HardwareModel hardwareModel { get; set; }
        public int operatingSystemId { get; set; }
        public OperatingSystem operatingSystem { get; set; }
        public List<object> owners { get; set; }
        public object activeDirectoryOperatingSystemName { get; set; }
        public object activeDirectoryOperatingSystemVersion { get; set; }
    }

    public class InstalledPrinter
    {
        public string location { get; set; }
        public string server { get; set; }
        public bool isDefault { get; set; }
        public bool canBeDefault { get; set; }
        public bool canBeRemoved { get; set; }
        public string installedDate { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public object tag { get; set; }
        public List<object> tags { get; set; }
    }

    public class SysManInformation
    {
        public TargetInformation targetInformation { get; set; }
        public TargetIdentity targetIdentity { get; set; }
        public Deployment deployment { get; set; }
        public List<Role> roles { get; set; }
        public List<InstalledPrinter> installedPrinters { get; set; }
        public List<object> tags { get; set; }
    }

    public class ActiveDirectoryInformation
    {
        public string securityIdentifier { get; set; }
        public string distinguishedName { get; set; }
        public string displayName { get; set; }
        public string operatingSystem { get; set; }
        public string operatingSystemVersion { get; set; }
        public string operatingSystemServicePack { get; set; }
        public object managedBy { get; set; }
        public string description { get; set; }
    }

    public class WarrantyInformation
    {
        public object make { get; set; }
        public object model { get; set; }
        public object serialNumber { get; set; }
        public object startDate { get; set; }
        public object endDate { get; set; }
        public object description { get; set; }
    }

    public class InfoLookup
    {
        public bool existsInExternal { get; set; }
        public bool existsInSccm { get; set; }
        public string model { get; set; }
        public string alternativeModel { get; set; }
        public string systemType { get; set; }
        public string serial { get; set; }
        public string lastHardwareScan { get; set; }
        public string lastUser { get; set; }
        public string lastUserDomain { get; set; }
        public string operatingSystem { get; set; }
        public string operatingSystemVersion { get; set; }
        public string processorArchitecture { get; set; }
        public string lastBootTime { get; set; }
        public string firstRegistration { get; set; }
        public string latestHeartbeat { get; set; }
        public List<object> logs { get; set; }
        public List<InstalledSystem> installedSystems { get; set; }
        public List<InstalledApplication> installedApplications { get; set; }
        public List<string> collections { get; set; }
        public List<SccmSoftware> sccmSoftwares { get; set; }
        public List<ApplicationDeployment> applicationDeployments { get; set; }
        public string clientName { get; set; }
        public List<object> inventoryData { get; set; }
        public SysManInformation sysManInformation { get; set; }
        public object health { get; set; }
        public ActiveDirectoryInformation activeDirectoryInformation { get; set; }
        public object upgradeResult { get; set; }
        public WarrantyInformation warrantyInformation { get; set; }
    }
}
