using System.Diagnostics.CodeAnalysis;

namespace Digizuite.Models
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum AssetType
    {
        All = 0,
        Video = 1,
        Audio = 2,
        Image = 4,
        PowerPoint = 5,
        Html = 6,
        Text = 7,
        Word = 8,
        Excel = 9,
        InDesign = 10,
        Zip = 11,
        META = 12,
        PDF = 14,
        Archive = 15,
        Photoshop = 16,
        Illustrator = 17,
        Visio = 18,
        Cad = 19,
		Font = 20,
        AfterEffects = 21,
        PremierePro = 22,
        ODF = 110,
        ODG = 107,
        ODB = 109,
        ODM = 111,
        ODP = 105,
        ODS = 102,
        OTH = 112,
        OTP = 106,
        ODT = 100,
        OTG = 108,
        OTS = 103,
        OTT = 101,
        Live = 1000,
    }
}