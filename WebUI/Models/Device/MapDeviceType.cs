using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CameraMap.Models.Device
{
    public enum MapDeviceType
    {
        Point,
        Line
    }
    public struct BMapPoint
    {
        public decimal Lat;
        public decimal Lng;
    }
    public interface IDevice
    {
        MapDeviceType DeviceType { get; }
        string DeviceTypeName { get; }
        long DeviceId { get; }
        string DeviceKey { get; }
        string DeviceName { get; }
        BMapPoint[] MapPoints { get; }
        long LayerId { get; }
        string LayerKey { get; }
        string LayerName { get; }
        int DisplayFlag { get; }
    }
}
