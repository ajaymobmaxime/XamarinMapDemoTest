﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Map = Xamarin.Forms.Maps.Map;

namespace MapDemo
{
    public partial class MainPage : ContentPage
    {
        Geocoder geocoder;

        public MainPage()
        {
            InitializeComponent();
            geocoder = new Geocoder();
            SetupMap();
        }

        public void SetupMap()
        {
            try {

                addressEntry.Text = "Taj Mahal, Agra, Uttar Pradesh, India";

                SetPinByAddress();

            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }

        private void BtnClear_Clicked(object sender, EventArgs e)
        {
            mapObject.Pins.Clear();
            addressEntry.Text = "";
        }

        private void BtnAdd_Clicked(object sender, EventArgs e)
        {
            SetPinByAddress();
        }

        private void MapObject_MapClicked(object sender, MapClickedEventArgs e)
        {
            var postion = e.Position;
            AddPins(postion);
            SetAddress(postion);
        }

        public void AddPins(Position position)
        {
            var pin = new Pin {
                Type = PinType.Place,
                Position = position,
                Label = addressEntry.Text
            };

            mapObject.Pins.Add(pin);
            mapObject.MoveToRegion(MapSpan.FromCenterAndRadius(position, new Distance(5000)));

        }

        private async void SetAddress(Position p)
        {
            var addrs = (await Geocoding.GetPlacemarksAsync(new Location(p.Latitude, p.Longitude))).FirstOrDefault();
            string Street = $"{addrs.Thoroughfare} {addrs.SubThoroughfare}";
            string City = $"{addrs.PostalCode} {addrs.Locality}";
            string Country = addrs.CountryName;

            addressEntry.Text = Street + City + Country;
        }

        private async void SetPinByAddress() {
            var addressPosition = new Position();
            var addressLocation = await geocoder.GetPositionsForAddressAsync(addressEntry.Text);

            foreach (var position in addressLocation) {
                addressPosition = new Position(position.Latitude, position.Longitude);
            }

            AddPins(addressPosition);
        }
    }
}
