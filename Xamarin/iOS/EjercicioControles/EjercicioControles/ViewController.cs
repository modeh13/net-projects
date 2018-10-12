using System;
using System.Linq;

using UIKit;
using CoreLocation;
using MapKit;
using Foundation;
using System.Collections.Generic;
using EjercicioControles.Classes;

namespace EjercicioControles
{
    public partial class ViewController : UIViewController
    {
        public List<Place> List;

        public ViewController(IntPtr handle) : base(handle)
        {
            List = PlacesList();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            string html = "<html><body><p><center><h1>Web Embebido</h1></center></p></body></html>";

            ivwVisor.Image = UIImage.FromFile("img/fp.png");
            wvwVisorWeb.LoadHtmlString(html, new NSUrl("./", true));
            mvwMap.CenterCoordinate = new CLLocationCoordinate2D(21.152676, -101.711698);

            sgmSelector.ValueChanged += (sender, e) => 
            {
                switch (sgmSelector.SelectedSegment)
                {
                    case 0:
                        mvwMap.MapType = MKMapType.Standard;
                        break;

                    case 1:
                        mvwMap.MapType = MKMapType.Satellite;
                        break;

                    case 2:
                        mvwMap.MapType = MKMapType.Hybrid;
                        break;
                }
            };

            List.ForEach(x =>
            {
                mvwMap.AddAnnotation(new MKPointAnnotation() {
                    Title = x.Name,
                    Coordinate = new CLLocationCoordinate2D() {
                        Latitude = x.Latitude,
                        Longitude = x.Longitude
                    }                    
                });
            });

            var cond = List.Count;
            // Defininito of routes to MKMap
            Place leMX = List.FirstOrDefault(x => x.Key == "LEMX");
            Place caMX = List.FirstOrDefault(x => x.Key == "CAMX");
            Place tjMX = List.FirstOrDefault(x => x.Key == "TJMX");

            // Coordinates
            var leon = new CLLocationCoordinate2D(leMX.Latitude, leMX.Longitude);
            var cancun = new CLLocationCoordinate2D(caMX.Latitude, caMX.Longitude);
            var tijuana = new CLLocationCoordinate2D(tjMX.Latitude, tjMX.Longitude);

            var info = new NSDictionary();
            var request1 = new MKDirectionsRequest()
            {
                Source = new MKMapItem(new MKPlacemark(leon, info)),
                Destination = new MKMapItem(new MKPlacemark(cancun, info)),
            };

            var request2 = new MKDirectionsRequest()
            {
                Source = new MKMapItem(new MKPlacemark(leon, info)),
                Destination = new MKMapItem(new MKPlacemark(tijuana, info)),
            };

            var routeLeonCancun = new MKDirections(request1);
            routeLeonCancun.CalculateDirections((response, error) =>
            {
                if (error == null)
                {
                    var route = response.Routes[0];
                    var line = new MKPolylineRenderer(route.Polyline)
                    {
                        LineWidth = 5.0f,
                        StrokeColor = UIColor.Red
                    };

                    mvwMap.OverlayRenderer = (res, err) => line;
                    mvwMap.AddOverlay(route.Polyline, MKOverlayLevel.AboveRoads);
                }
            });

            var routeLeonTijuana = new MKDirections(request2);
            routeLeonTijuana.CalculateDirections((response, error) =>
            {
                if (error == null)
                {
                    var route = response.Routes[0];
                    var line = new MKPolylineRenderer(route.Polyline)
                    {
                        LineWidth = 5.0f,
                        StrokeColor = UIColor.Blue
                    };

                    mvwMap.OverlayRenderer = (res, err) => line;
                    mvwMap.AddOverlay(route.Polyline, MKOverlayLevel.AboveRoads);
                }
            });
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }


        #region "Métodos"
        private List<Place> PlacesList()
        {
            var places = new List<Place>()
            {
                new Place("LEMX", "León, México", 21.152676, -101.711698),
                new Place("CAMX", "Cancún, México", 21.052743, -86.847242),
                new Place("TJMX", "Tijuana, México", 32.526384, -117.028983)
            };
            
            return places;
        }

        private Place GetPlaceByKey(string key)
        {
            foreach (Place place in List)
            {
                if (place.Key == key)
                {
                    return place;
                }
            }
            return null;
        }
        #endregion
    }
}