using System.Diagnostics.CodeAnalysis;

namespace Digizuite.Models.Metadata
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "CA1707")]
    public enum CodedFolder
    {
        Default = 0,
        Admin_Root = 1,
        Admin_Users_And_Groups = 2,
        Admin_Users = 3,
        Admin_Groups = 4,
        Admin_Profiles = 5,
        Admin_Destinations = 6,
        Admin_Dam = 7,
        Admin_DigizuiteConfig = 8,
        Admin_Mediaformat = 9,
        Admin_TranscodeSetting = 10,
        Admin_License = 11,
        Admin_ValidationFunctions = 12,
        Admin_Mediaformat_Types = 13,
        Admin_Mediaformat_Type_Extensions = 14,
        Admin_Application = 15,
        Admin_DigizuiteConfig_AssetType = 16,
        Admin_IdentifyRules = 17,
        Admin_IdentifyRuleConditions = 18,

//        Admin_Search = 19,
        Admin_Installer_Server = 20,
        Admin_Installer = 21,
        Admin_StreamAccessRules = 23,
        Admin_StreamAccessIPMasks = 24,
        Admin_Config = 25,

        Portal_Root = 30,
        Portal_Workfolder = 31,
        Portal_Layout = 32,
        Portal_Templates = 33,
        Portal_Pages = 34,

        DigiBatch_Standard = 40,

        Admin_FrontendUsers_And_Groups = 50,
        Admin_FrontendUsers = 51,
        Admin_FrontendGroups = 52,

        MemberSettings_Root = 60,
        MemberSettings_Preferences = 61,
        MemberSettings_Information = 62,
        Admin_Search_Stopwords = 63,
        Admin_ClientLicense = 64,

        Admin_MetaGroup = 65,
        Admin_MetaField = 66,

        Admin_Workflow = 67,
        Admin_Status = 68,
        Admin_Metadata_Language = 69
    };
}