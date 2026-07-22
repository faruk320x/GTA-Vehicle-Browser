using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Windows.Forms;
using System.Drawing;

namespace GTAVehicleBrowser
{
    public class MainForm : Form
    {
        TextBox searchBox;
        ListBox vehicleList;
        Label infoLabel;
        Button copyButton;
        PictureBox vehicleImage;

        List<Vehicle> vehicles = new();


        public MainForm()
        {
            Text = "GTA V Vehicle Catalog";
            Width = 900;
            Height = 600;


            searchBox = new TextBox();
            searchBox.Left = 20;
            searchBox.Top = 20;
            searchBox.Width = 820;
            searchBox.PlaceholderText = "Araç ara...";
            searchBox.TextChanged += SearchBox_TextChanged;


            vehicleList = new ListBox();
            vehicleList.Left = 20;
            vehicleList.Top = 60;
            vehicleList.Width = 300;
            vehicleList.Height = 450;
            vehicleList.SelectedIndexChanged += VehicleList_SelectedIndexChanged;


            vehicleImage = new PictureBox();
            vehicleImage.Left = 360;
            vehicleImage.Top = 60;
            vehicleImage.Width = 350;
            vehicleImage.Height = 220;
            vehicleImage.SizeMode = PictureBoxSizeMode.Zoom;


            infoLabel = new Label();
            infoLabel.Left = 360;
            infoLabel.Top = 300;
            infoLabel.Width = 350;
            infoLabel.Height = 120;


            copyButton = new Button();
            copyButton.Left = 360;
            copyButton.Top = 430;
            copyButton.Width = 220;
            copyButton.Height = 40;
            copyButton.Text = "Model Kodunu Kopyala";
            copyButton.Click += CopyButton_Click;


            Controls.Add(searchBox);
            Controls.Add(vehicleList);
            Controls.Add(vehicleImage);
            Controls.Add(infoLabel);
            Controls.Add(copyButton);


            LoadVehicles();
        }


        void LoadVehicles()
        {
            string file =
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "vehicles.json"
                );


            if (File.Exists(file))
            {
                string json = File.ReadAllText(file);

                vehicles =
                    JsonSerializer.Deserialize<List<Vehicle>>(json)
                    ?? new();
            }

            RefreshList(vehicles);
        }


        void RefreshList(List<Vehicle> list)
        {
            vehicleList.Items.Clear();

            foreach (var v in list)
                vehicleList.Items.Add(v.Name);
        }


        void SearchBox_TextChanged(object? sender, EventArgs e)
        {
            string text = searchBox.Text.ToLower();

            var result = vehicles
                .Where(x =>
                x.Name.ToLower().Contains(text) ||
                x.Model.ToLower().Contains(text))
                .ToList();

            RefreshList(result);
        }


        Vehicle? GetSelected()
        {
            if (vehicleList.SelectedItem == null)
                return null;

            return vehicles.FirstOrDefault(
                x => x.Name ==
                vehicleList.SelectedItem.ToString()
            );
        }


        void VehicleList_SelectedIndexChanged(
            object? sender,
            EventArgs e)
        {
            var vehicle = GetSelected();

            if (vehicle == null)
                return;


            infoLabel.Text =
                "Araç: " + vehicle.Name +
                "\n\nModel: " + vehicle.Model +
                "\n\nKategori: " + vehicle.Type;


            string imagePath =
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "images",
                    vehicle.Image
                );


            if (File.Exists(imagePath))
            {
                vehicleImage.Image =
                    Image.FromFile(imagePath);
            }
            else
            {
                vehicleImage.Image = null;
            }
        }


        void CopyButton_Click(
            object? sender,
            EventArgs e)
        {
            var vehicle = GetSelected();

            if (vehicle == null)
                return;


            Clipboard.SetText(vehicle.Model);

            MessageBox.Show(
                "Kopyalandı: " + vehicle.Model
            );
        }
    }


    public class Vehicle
    {
        public string Name { get; set; } = "";
        public string Model { get; set; } = "";
        public string Type { get; set; } = "";
        public string Image { get; set; } = "";
    }
}
