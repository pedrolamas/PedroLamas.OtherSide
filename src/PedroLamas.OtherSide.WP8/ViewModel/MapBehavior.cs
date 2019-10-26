using System;
using System.Device.Location;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Phone.Maps.Controls;
using PedroLamas.OtherSide.Model;

namespace PedroLamas.OtherSide.ViewModel
{
    public class MapBehavior : Behavior<Map>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.CenterChanged += MapCenterChanged;
            AssociatedObject.ZoomLevelChanged += MapZoomLevelChanged;
            AssociatedObject.Hold += MapHold;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.CenterChanged -= MapCenterChanged;
            AssociatedObject.ZoomLevelChanged -= MapZoomLevelChanged;
            AssociatedObject.Hold -= MapHold;

            base.OnDetaching();
        }

        public Coordinate Position
        {
            get { return (Coordinate)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(Coordinate), typeof(MapBehavior), new PropertyMetadata(null, OnPositionChanged));

        private static void OnPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mapBehavior = (MapBehavior)d;
            var coordinate = (Coordinate)e.NewValue;

            mapBehavior.DrawPosition(new GeoCoordinate(coordinate.Latitude, coordinate.Longitude));
        }

        public Coordinate Center
        {
            get { return (Coordinate)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }

        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register("Center", typeof(Coordinate), typeof(MapBehavior), new PropertyMetadata(null, OnCenterChanged));

        private static void OnCenterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mapBehavior = (MapBehavior)d;
            var coordinate = (Coordinate)e.NewValue;

            if (coordinate != null)
            {
                mapBehavior.AssociatedObject.Center = new GeoCoordinate(coordinate.Latitude, coordinate.Longitude);
            }
        }

        public double ZoomLevel
        {
            get { return (double)GetValue(ZoomLevelProperty); }
            set { SetValue(ZoomLevelProperty, value); }
        }

        public static readonly DependencyProperty ZoomLevelProperty =
            DependencyProperty.Register("ZoomLevel", typeof(double), typeof(MapBehavior), new PropertyMetadata(1.0, OnZomLevelChanged));

        private static void OnZomLevelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mapBehavior = (MapBehavior)d;
            var zoomLevel = (double)e.NewValue;

            if (zoomLevel >= 1 && zoomLevel <= 20)
            {
                mapBehavior.AssociatedObject.ZoomLevel = zoomLevel;
            }
        }

        private void MapCenterChanged(object sender, MapCenterChangedEventArgs e)
        {
            Center = new Coordinate(AssociatedObject.Center.Latitude, AssociatedObject.Center.Longitude);
        }

        private void MapZoomLevelChanged(object sender, MapZoomLevelChangedEventArgs e)
        {
            ZoomLevel = AssociatedObject.ZoomLevel;
        }

        private void MapHold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var position = e.GetPosition(AssociatedObject);
            var coordinate = AssociatedObject.ConvertViewportPointToGeoCoordinate(position);

            Position = new Coordinate(coordinate.Latitude, coordinate.Longitude);
        }

        private void DrawPosition(GeoCoordinate coordinate)
        {
            AssociatedObject.Layers.Clear();

            var polygon = new Polygon()
            {
                Fill = new SolidColorBrush(Colors.Black)
            };

            polygon.Points.Add(new Point(0, 0));
            polygon.Points.Add(new Point(0, 60));
            polygon.Points.Add(new Point(25, 35));
            polygon.Points.Add(new Point(25, 0));

            //polygon.Tag = new GeoCoordinate(coordinate.Latitude, coordinate.Longitude);
            //polygon.MouseLeftButtonUp += new MouseButtonEventHandler(Marker_Click);

            var mapLayer = new MapLayer
            {
                new MapOverlay
                {
                    Content = polygon,
                    GeoCoordinate = new GeoCoordinate(coordinate.Latitude, coordinate.Longitude),
                    PositionOrigin = new Point(0.0, 1.0)
                }
            };

            AssociatedObject.Layers.Add(mapLayer);
        }
    }
}