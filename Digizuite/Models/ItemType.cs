using System;
using System.Collections.Generic;
using System.Text;
// ReSharper disable IdentifierTypo

namespace Digizuite.Models
{
    public enum ItemType
    {
        Undefined = -1,
        Asset = 0,
        Member = 1,
        MemberGroup = 2,
        Profile = 3,
        Destination = 4,
        AssetDigiUpload = 5,
        Basket = 6,
        ChannelFolder = 7,
        CatalogFolder = 8,
        Metafield = 9,
        MetafieldLabel = 10,
        MetafieldGroup = 11,
        ComboValue = 12,
        TreeValue = 13,
        MetafieldValidation = 14,
        MediaTranscode = 15,
        MediaFormat = 16,
        MediaFormatType = 17,

        //damcatalog = 18
        Product = 19,

        //versioned_tree = 20
        Search = 21,
        Wizard = 22,

        DigizuiteConfig = 23
        //playertemplate = 24
    }
}
