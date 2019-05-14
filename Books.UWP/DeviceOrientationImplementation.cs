using Books.InterfacesDS;
using Books.UWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceOrientationImplementation))]
namespace Books.UWP
{
    public class DeviceOrientationImplementation : IDeviceOrientation
    {
        public DeviceOrientationImplementation() { }

        public DeviceOrientations GetOrientation()
        {
            var orientation = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().Orientation;
            if (orientation == Windows.UI.ViewManagement.ApplicationViewOrientation.Landscape)
            {
                return DeviceOrientations.Landscape;
            }
            else
            {
                return DeviceOrientations.Portrait;
            }
        }
    }
}
