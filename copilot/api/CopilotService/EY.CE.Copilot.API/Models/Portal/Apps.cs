using EY.SAT.CE.SharePoint.Models;
using Newtonsoft.Json;

namespace EY.CE.Copilot.API.Models.Portal
{
     public class AppBundle
    {
        [JsonProperty("appClassIds")]
        public List<AppClassId> AppClassIds { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }
    }

    public class AppBundleClassDetail
    {
        [JsonProperty("appBundleClassId")]
        public string AppBundleClassId { get; set; }

        [JsonProperty("appBundleClassName")]
        public string AppBundleClassName { get; set; }

        [JsonProperty("appBundleClassDescription")]
        public string AppBundleClassDescription { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("appClassIds")]
        public List<AppClassId> AppClassIds { get; set; }
    }

    public class AppClassId
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class AppDetail
    {
        [JsonProperty("appClassId")]
        public string AppClassId { get; set; }

        [JsonProperty("appClassName")]
        public string AppClassName { get; set; }

        [JsonProperty("appURL")]
        public string AppURL { get; set; }

        [JsonProperty("isPublished")]
        public bool IsPublished { get; set; }

        [JsonProperty("appCategoryId")]
        public string AppCategoryId { get; set; }

        [JsonProperty("appPlatformId")]
        public string AppPlatformId { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("appDescription")]
        public string AppDescription { get; set; }

        [JsonProperty("appOwners")]
        public List<AppOwner> AppOwners { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("createdOn")]
        public object CreatedOn { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("modifiedOn")]
        public object ModifiedOn { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("deleted")]
        public object Deleted { get; set; }
    }

    public class AppOwner
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("appOwnerEmail")]
        public string AppOwnerEmail { get; set; }
    }

    public class AppPlatformDetail
    {
        [JsonProperty("appPlatformClassId")]
        public string AppPlatformClassId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("siteURL")]
        public string SiteURL { get; set; }

        [JsonProperty("apiUrl")]
        public string ApiUrl { get; set; }

        [JsonProperty("statusId")]
        public string StatusId { get; set; }

        [JsonProperty("initialProvisioningStatusId")]
        public string InitialProvisioningStatusId { get; set; }

        [JsonProperty("provisioningStartedOn")]
        public object ProvisioningStartedOn { get; set; }

        [JsonProperty("provisioningEndedOn")]
        public object ProvisioningEndedOn { get; set; }

        [JsonProperty("externalIds")]
        public List<object> ExternalIds { get; set; }

        [JsonProperty("isEmailEnabled")]
        public object IsEmailEnabled { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("createdOn")]
        public object CreatedOn { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("modifiedOn")]
        public object ModifiedOn { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("deleted")]
        public object Deleted { get; set; }
    }

    public class Area
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class CopilotDetails
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("siteURL")]
        public string SiteURL { get; set; }

        [JsonProperty("apiUrl")]
        public string ApiUrl { get; set; }

        [JsonProperty("createdOn")]
        public long CreatedOn { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("modifiedOn")]
        public long ModifiedOn { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }
    }

    public class Country
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class AppsData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("paceId")]
        public object PaceId { get; set; }

        [JsonProperty("additionalPaceIds")]
        public object AdditionalPaceIds { get; set; }

        [JsonProperty("isDraft")]
        public bool IsDraft { get; set; }

        [JsonProperty("clientName")]
        public string ClientName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("sectorId")]
        public string SectorId { get; set; }

        [JsonProperty("sectorName")]
        public string SectorName { get; set; }

        [JsonProperty("channelId")]
        public object ChannelId { get; set; }

        [JsonProperty("clientSizeId")]
        public string ClientSizeId { get; set; }

        [JsonProperty("regionName")]
        public string RegionName { get; set; }

        [JsonProperty("stepProgress")]
        public string StepProgress { get; set; }

        [JsonProperty("projectTypeId")]
        public string ProjectTypeId { get; set; }

        [JsonProperty("projectStatus")]
        public ProjectStatus ProjectStatus { get; set; }

        [JsonProperty("bundleClassId")]
        public string BundleClassId { get; set; }

        [JsonProperty("bundleName")]
        public object BundleName { get; set; }

        [JsonProperty("bundleDescription")]
        public object BundleDescription { get; set; }

        [JsonProperty("appBundle")]
        public AppBundle AppBundle { get; set; }

        [JsonProperty("appBundleClassDetail")]
        public AppBundleClassDetail AppBundleClassDetail { get; set; }

        [JsonProperty("appDetails")]
        public List<AppDetail> AppDetails { get; set; }

        [JsonProperty("appPlatformDetails")]
        public List<AppPlatformDetail> AppPlatformDetails { get; set; }

        [JsonProperty("demoURL")]
        public object DemoURL { get; set; }

        [JsonProperty("closeDate")]
        public object CloseDate { get; set; }

        [JsonProperty("mfaEnabled")]
        public bool MfaEnabled { get; set; }

        [JsonProperty("projectFriendlyId")]
        public string ProjectFriendlyId { get; set; }

        [JsonProperty("isConfidential")]
        public bool IsConfidential { get; set; }

        [JsonProperty("infrastructureStatusId")]
        public string InfrastructureStatusId { get; set; }

        [JsonProperty("deprovisionedStatusId")]
        public object DeprovisionedStatusId { get; set; }

        [JsonProperty("numberOfUsers")]
        public int NumberOfUsers { get; set; }

        [JsonProperty("createdByUserName")]
        public string CreatedByUserName { get; set; }

        [JsonProperty("createdByUserEmail")]
        public string CreatedByUserEmail { get; set; }

        [JsonProperty("projectCategoryId")]
        public string ProjectCategoryId { get; set; }

        [JsonProperty("projectCategoryName")]
        public string ProjectCategoryName { get; set; }

        [JsonProperty("expirationDate")]
        public long ExpirationDate { get; set; }

        [JsonProperty("iconId")]
        public string IconId { get; set; }

        [JsonProperty("locationDetails")]
        public LocationDetails LocationDetails { get; set; }

        [JsonProperty("isDemoProject")]
        public object IsDemoProject { get; set; }

        [JsonProperty("defaultUserRoleId")]
        public object DefaultUserRoleId { get; set; }

        [JsonProperty("ppeddApproverDetails")]
        public object PpeddApproverDetails { get; set; }

        [JsonProperty("ppeddApproverId")]
        public object PpeddApproverId { get; set; }

        [JsonProperty("provisioningQuestionsDetails")]
        public object ProvisioningQuestionsDetails { get; set; }

        [JsonProperty("geoLocation")]
        public GeoLocation GeoLocation { get; set; }

        [JsonProperty("isSharePointSyncEnabled")]
        public bool IsSharePointSyncEnabled { get; set; }

        [JsonProperty("accessRestrictionKey")]
        public object AccessRestrictionKey { get; set; }

        [JsonProperty("isRestricted")]
        public bool IsRestricted { get; set; }

        [JsonProperty("serviceLineId")]
        public string ServiceLineId { get; set; }

        [JsonProperty("isWelcomeNotificationEnabled")]
        public bool IsWelcomeNotificationEnabled { get; set; }

        [JsonProperty("projectCategoryConfirmation")]
        public object ProjectCategoryConfirmation { get; set; }

        [JsonProperty("projectReports")]
        public List<ProjectReport> ProjectReports { get; set; }

        [JsonProperty("copilotDetails")]
        public CopilotDetails CopilotDetails { get; set; }

        [JsonProperty("projectAdmins")]
        public List<string> ProjectAdmins { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("createdOn")]
        public long CreatedOn { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("modifiedOn")]
        public long ModifiedOn { get; set; }

        [JsonProperty("modifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("deleted")]
        public object Deleted { get; set; }
    }

    public class ExternalId
    {
        [JsonProperty("appPlatformClassKey")]
        public string AppPlatformClassKey { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class GeoLocation
    {
        [JsonProperty("area")]
        public Area Area { get; set; }

        [JsonProperty("region")]
        public Region Region { get; set; }

        [JsonProperty("country")]
        public Country Country { get; set; }
    }

    public class LocationDetails
    {
        [JsonProperty("locationId")]
        public string LocationId { get; set; }

        [JsonProperty("locationName")]
        public string LocationName { get; set; }

        [JsonProperty("locationDescription")]
        public string LocationDescription { get; set; }

        [JsonProperty("locationBaseURL")]
        public string LocationBaseURL { get; set; }

        [JsonProperty("sharePointBaseUrl")]
        public object SharePointBaseUrl { get; set; }

        [JsonProperty("externalIds")]
        public List<ExternalId> ExternalIds { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }
    }

    public class ProjectReport
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("pbiReportId")]
        public string PbiReportId { get; set; }

        [JsonProperty("pbiReportName")]
        public string PbiReportName { get; set; }

        [JsonProperty("pbiReportDatasetId")]
        public string PbiReportDatasetId { get; set; }

        [JsonProperty("pbiReportEmbedUrl")]
        public string PbiReportEmbedUrl { get; set; }
    }

    public class ProjectStatus
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("projectStatusName")]
        public string ProjectStatusName { get; set; }
    }

    public class Region
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Apps
    {
        [JsonProperty("data")]
        public AppsData Data { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }


}
