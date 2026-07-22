using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace GTAVehicleBrowser
{
    public class MainForm : Form
    {
        TextBox searchBox;
        ListBox vehicleList;
        PictureBox vehicleImage;
        Label infoLabel;
        Button copyButton;

        List<Vehicle> vehicles = new();


        public MainForm()
        {
            Text = "GTA V Vehicle Browser";
            Width = 900;
            Height = 600;


            searchBox = new TextBox();
            searchBox.Left = 20;
            searchBox.Top = 20;
            searchBox.Width = 820;
            searchBox.PlaceholderText = "Araç adı veya model kodu ara...";
            searchBox.TextChanged += SearchBox_TextChanged;


            vehicleList = new ListBox();
            vehicleList.Left = 20;
            vehicleList.Top = 60;
            vehicleList.Width = 300;
            vehicleList.Height = 450;
            vehicleList.SelectedIndexChanged += VehicleList_SelectedIndexChanged;


            vehicleImage = new PictureBox();
            vehicleImage.Left = 370;
            vehicleImage.Top = 60;
            vehicleImage.Width = 350;
            vehicleImage.Height = 250;
            vehicleImage.SizeMode = PictureBoxSizeMode.Zoom;


            infoLabel = new Label();
            infoLabel.Left = 370;
            infoLabel.Top = 340;
            infoLabel.Width = 400;
            infoLabel.Height = 100;


            copyButton = new Button();
            copyButton.Left = 370;
            copyButton.Top = 450;
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
            string file = Path.Combine(
                Application.StartupPath,
                "vehicles.json"
            );


            if (!File.Exists(file))
            {
                MessageBox.Show(
                    "vehicles.json bulunamadı!"
                );
                return;
            }


            string json = File.ReadAllText(file);


            vehicles =
                JsonSerializer.Deserialize<List<Vehicle>>(json)
                ?? new List<Vehicle>();


            RefreshList(vehicles);
        }


        void RefreshList(List<Vehicle> list)
        {
            vehicleList.Items.Clear();

            foreach (var vehicle in list)
            {
                vehicleList.Items.Add(vehicle.Name);
            }
        }


        void SearchBox_TextChanged(
            object? sender,
            EventArgs e)
        {
            string text = searchBox.Text.ToLower();


            var result = vehicles
                .Where(x =>
                    x.Name.ToLower().Contains(text) ||
                    x.Model.ToLower().Contains(text)
                )
                .ToList();


            RefreshList(result);
        }


        Vehicle? GetSelectedVehicle()
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
            var vehicle = GetSelectedVehicle();

            if (vehicle == null)
                return;


            infoLabel.Text =
                "Araç: " + vehicle.Name +
                "\nModel: " + vehicle.Model +
                "\nKategori: " + vehicle.Type;


            string image =
                Path.Combine(
                    Application.StartupPath,
                    "images",
                    vehicle.Image
                );


            if (File.Exists(image))
            {
                vehicleImage.Image =
                    Image.FromFile(image);
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
            var vehicle = GetSelectedVehicle();

            if (vehicle == null)
                return;


            Clipboard.SetText(vehicle.Model);

            MessageBox.Show(
                vehicle.Model + " kopyalandı"
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
