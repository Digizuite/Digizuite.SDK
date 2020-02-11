// ReSharper disable InconsistentNaming
namespace Digizuite.BatchUpdate.Models
{
    #pragma warning disable CA1707
    public enum RepositoryType
    {
        Default = 0,
        Catalog = 1,

        //[Obsolete("RepositoryType UserFolder obsolete")]
        Userfolder = 2,

        //[Obsolete("RepositoryType Recyclebin obsolete")]
        Recyclebin = 3,
        SharedFolder = 4,
        SearchFolder = 5,
        UploadFolder = 6,

        //[Obsolete("RepositoryType ExportFolder obsolete")]
        ExportFolder = 7,
        AdminFolder = 8,
        SettingsFolder = 9,
        Portal = 10,
        DigiBatch = 11,
        BackendUsers = 12,
        BackendGroups = 13,
        media_format = 16,
        transcode = 17,
        LanguageLabel = 18,
        Profile = 19,
        MetaGroup = 20,
        Config = 21,
        Product = 22,
        Collection = 23
    }
    #pragma warning restore CA1707
}