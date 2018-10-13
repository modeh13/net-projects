using MacPolloApp.Model.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacPolloApp.ViewModel
{
    public class HomeViewModel : BaseObservable
    {
        #region "Properties"
        private ObservableCollection<ImageHome> images;
        public ObservableCollection<ImageHome> Images
        {
            get { return images; }
            set { images = value; }
        }

        private int position;

        public int Position
        {
            get { return position; }
            set { position = value; OnPropertyChange(); }
        }
        #endregion

        #region "Constructors"

        public HomeViewModel()
        {
            //Get ImagesHome
            List <ImageHome> list = new List<ImageHome>()
            {
                new ImageHome { ImageUrl = "http://www.macpollo.com/sites/default/files/styles/img_slide_home/public/Slider/pollo_entero.png?itok=T13v55BB", Name="Pollo" },
                new ImageHome { ImageUrl = "http://www.macpollo.com/sites/default/files/styles/img_slide_home/public/Slider/fajitas_de_pollo.png?itok=YLHSEk7d", Name="Pollo"},
                new ImageHome { ImageUrl = "http://www.macpollo.com/sites/default/files/styles/img_slide_home/public/Slider/para_picar_salsamentaria_mac_pollo.png?itok=VGRYcsOA", Name="Pollo"},
            };
            Images = new ObservableCollection<ImageHome>(list);
        }
        #endregion
    }
}