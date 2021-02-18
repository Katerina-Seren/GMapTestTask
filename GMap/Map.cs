using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GMap
{
    public partial class Map : Form
    {
        public Map()
        {
            InitializeComponent();
        }

        private void gMapControl_Load(object sender, EventArgs e)
        {
            gMapControl.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gMapControl.Position = new GMap.NET.PointLatLng(55, 83);
            gMapControl.ShowCenter = false;
            gMapControl.MinZoom = 3;
            gMapControl.MaxZoom = 18;
            gMapControl.Zoom = 10;
            gMapControl.DragButton = MouseButtons.Right;

            mouseIsDown = false;
            IsMarkerEnter = false;
            currentMarker = null;

            unitService = new UnitService(@"Data Source=DESKTOP-OF4FFJE\SQLEXPRESS;Initial Catalog=unitsdb;Integrated Security=True");
            if (!unitService.IsConnectionOpen())
            {
                MessageBox.Show("Ошибка подключения к бд", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            var units = unitService.GetAll();
            var markers = CreateMarkers(units);

            gMapControl.Overlays.Add(markers);
        }

        private GMapOverlay CreateMarkers(List<Unit> units)
        {
            var markers = new GMapOverlay("markers");

            foreach (var u in units)
            {
                var marker = new GMarkerGoogle(
                    new GMap.NET.PointLatLng(u.Lat, u.Lng), GMarkerGoogleType.red)
                {
                    ToolTipText = u.Name,
                    Tag = u.Id.ToString()
                };

                markers.Markers.Add(marker);
            }

            return markers;
        }

        private void gMapControl_OnMarkerEnter(GMapMarker item)
        {
            if (currentMarker == null)
            {
                currentMarker = Convert.ToInt32(item.Tag);
                IsMarkerEnter = true;
            }
        }

        private void gMapControl_OnMarkerLeave(GMapMarker item)
        {
            if (!mouseIsDown)
            {
                currentMarker = null;
                IsMarkerEnter = false;
            }
        }

        private void gMapControl_MouseDown(object sender, MouseEventArgs e)
        {
            mouseIsDown = true;
            mouseDownPoint = new Point(e.Location.X, e.Location.Y);
        }

        private void gMapControl_MouseUp(object sender, MouseEventArgs e)
        {
            mouseIsDown = false;
            if (currentMarker != null)
            {
                var marker = GetMarkerById(currentMarker);
                unitService.ChangePosition(Convert.ToInt32(marker.Tag), marker.Position.Lat, marker.Position.Lng);
            }
        }

        private void gMapControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsMarkerEnter && mouseIsDown)
            {
                var marker = GetMarkerById(currentMarker);

                if (marker != null)
                {
                    var point = gMapControl.FromLocalToLatLng(e.Location.X, e.Location.Y);
                    marker.Position = new GMap.NET.PointLatLng(point.Lat, point.Lng);
                }
            }
        }

        private GMapMarker GetMarkerById(int? id)
        {
            return gMapControl
                    .Overlays
                    .FirstOrDefault(x => x.Id == "markers")
                    .Markers
                    .FirstOrDefault(m => Convert.ToInt32(m.Tag) == id);
        }

        private void Map_FormClosed(object sender, FormClosedEventArgs e)
        {
            unitService.CloseConnection();
        }
    }
}