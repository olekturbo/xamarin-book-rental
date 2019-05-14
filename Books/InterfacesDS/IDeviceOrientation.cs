using System;
using System.Collections.Generic;
using System.Text;

namespace Books.InterfacesDS
{
    public enum DeviceOrientations
    {
        Undefined,
        Landscape,
        Portrait
    }

    public interface IDeviceOrientation
    {
        DeviceOrientations GetOrientation();
    }
}
