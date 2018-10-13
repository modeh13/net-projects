namespace EjercicioControles.Classes
{
    public class Place
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Place(string key, string name, double latitude, double longitude)
        {
            Key = key;
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}