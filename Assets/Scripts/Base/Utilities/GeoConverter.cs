using UnityEngine;

namespace Base.Utilities
{
    public class GeoConverter
    {
        private readonly double _originLat;
        private readonly double _originLon;

        public GeoConverter(double originLat, double originLon)
        {
            _originLat = originLat;
            _originLon = originLon;
        }

        public Vector3 LatLonToXY(double lat, double lon)
        {
            float x = (float)((lon - _originLon) * Mathf.Cos((float)(_originLat * Mathf.Deg2Rad)) * 111320);
            float z = (float)((lat - _originLat) * 110574);
            return new Vector3(x, 0, z);
        }
    }
}