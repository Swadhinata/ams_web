namespace AMS_API.Models
{
    public class AssetType
    {
        public int AssetTypeId { get; set; }
        public string AssetTypeName { get; set; }

        public AssetCategory AssetCategory { get; set; }

        public AssetType()
        {
            AssetCategory = new AssetCategory();
        }

    }
}
