namespace MacPolloApp.Model.Entities
{
    public sealed class ImageHome
    {
        #region "Properties"
        public string Name { get; set; }

        public string ImageUrl { get; set; }        
        #endregion

        #region "Constructors"
        public ImageHome() { }

        public ImageHome(string name, string imageUrl)
        {
            Name = name;
            ImageUrl = imageUrl;
        }
        #endregion
    }
}