using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

namespace TestTask
{
    public partial class Form1 : Form
    {
        private GMapOverlay markers = new GMapOverlay("markers");
        private GMapMarker currentMarker;


        private bool isHoveringMarker = false;
        private bool isDragging = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {
            GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;

            gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;         
            gMapControl1.MinZoom = 2;
            gMapControl1.MaxZoom = 16;
            gMapControl1.Zoom = 2;
            gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter;
            gMapControl1.CanDragMap = true;
            gMapControl1.DragButton = MouseButtons.Left;
            gMapControl1.ShowCenter = false;
            

            gMapControl1.Overlays.Add(markers);
            RestoreMarkers(DB.GetMarkers());
        }

        private void RestoreMarkers(List<Marker> markersList)
        {
            foreach(Marker marker in markersList)
            {
                PointLatLng p = new PointLatLng(marker.lat, marker.lng);
                GMarkerGoogle gMarker = GMarker(p, GMarkerGoogleType.blue);
                gMarker.Tag = marker.id;
                gMarker.ToolTipText = marker.vehicle;
                markers.Markers.Add(gMarker);
            }
        }

        private void AddMarker(PointLatLng pointClick, MouseEventArgs e)
        {
            GMarkerGoogle gMarker = GMarker(pointClick, GMarkerGoogleType.blue);
            Marker marker = new Marker { id = gMarker.Tag.ToString(), lat = gMarker.Position.Lat, lng = gMarker.Position.Lng, vehicle = gMarker.ToolTipText };
            DB.AddMarker(marker);
            markers.Markers.Add(gMarker);
        }

        #region DragNDrop
        private void StartDragNDrop(object sender, MouseEventArgs e)
        {
            if (!isHoveringMarker)
            {
                return;
            }
            isDragging = true;
        }
        private void DragNDrop(object sender, MouseEventArgs e)
        {
            if (!isDragging)
            {
                return;
            }
            double mouseLat = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat;
            double mouseLng = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng;
            currentMarker.Position = new PointLatLng(mouseLat, mouseLng);
            
        }
        private void StopDragNDrop(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                DB.UpdateMarkerPosition(new Marker { id = currentMarker.Tag.ToString(), lat = currentMarker.Position.Lat, lng = currentMarker.Position.Lng, vehicle = currentMarker.ToolTipText });
                isDragging = false;
            }
        }
       

        private void HoveringMarker(GMapMarker item)
        {
            if (isDragging)
            {
                return;
            }
            currentMarker = item;
            isHoveringMarker = true;
        }

        private void NotHoveringMarker(GMapMarker item)
        {
            if (isDragging)
            {  
                return;
            }
           
            currentMarker = null;
            isHoveringMarker = false;
        }
        #endregion

        private GMarkerGoogle GMarker(PointLatLng latLng, GMarkerGoogleType markerType)
        {
            GMarkerGoogle marker = new GMarkerGoogle(latLng, markerType);
            marker.ToolTipText = "Vehicle";
            marker.Tag = Guid.NewGuid().ToString();
            return marker;
        }
    }
}
