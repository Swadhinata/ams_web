namespace AMS_API.Models
{
    public class Asset
    {
        public int AssetId { get; set; }
        public string AssetName { get; set; }
        public AssetType AssetType { get; set; }

        public Asset()
        {
            AssetType = new AssetType();
        }
    }
}
