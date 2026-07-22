using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace GTAVehicleBrowser
{
    public class MainForm : Form
    {
        private TextBox searchBox;
        private ListBox vehicleList;
        private Label infoLabel;
        private Button copyButton;

        private List<Vehicle> vehicles = new();

        public MainForm()
        {
            Text = "GTA V Vehicle Browser";
            Width = 700;
            Height = 500;

            searchBox = new TextBox();
            searchBox.Left = 20;
            searchBox.Top = 20;
            searchBox.Width = 620;
            searchBox.PlaceholderText = "Araç ara...";
            searchBox.TextChanged += SearchBox_TextChanged;

            vehicleList = new ListBox();
            vehicleList.Left = 20;
            vehicleList.Top = 60;
            vehicleList.Width = 300;
            vehicleList.Height = 350;
            vehicleList.SelectedIndexChanged += VehicleList_SelectedIndexChanged;

            infoLabel = new Label();
            infoLabel.Left = 350;
            infoLabel.Top = 80;
            infoLabel.Width = 250;
            infoLabel.Height = 150;

            copyButton = new Button();
            copyButton.Left = 350;
            copyButton.Top = 250;
            copyButton.Width = 200;
            copyButton.Text = "Model Kodunu Kopyala";
            copyButton.Click += CopyButton_Click;

            Controls.Add(searchBox);
            Controls.Add(vehicleList);
            Controls.Add(infoLabel);
            Controls.Add(copyButton);

            LoadVehicles();
        }


        private void LoadVehicles()
        {
            string file = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "vehicles.json"
            );

            if (File.Exists(file))
            {
                string json = File.ReadAllText(file);

                vehicles = JsonSerializer.Deserialize<List<Vehicle>>(json)
                           ?? new List<Vehicle>();
            }

            RefreshList(vehicles);
        }


        private void RefreshList(List<Vehicle> list)
        {
            vehicleList.Items.Clear();

            foreach (var vehicle in list)
            {
                vehicleList.Items.Add(vehicle.Name);
            }
        }


        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            string text = searchBox.Text.ToLower();

            var result = vehicles
                .Where(x =>
                    x.Name.ToLower().Contains(text) ||
                    x.Model.ToLower().Contains(text))
                .ToList();

            RefreshList(result);
        }


        private void VehicleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (vehicleList.SelectedItem == null)
                return;


            var vehicle = vehicles.FirstOrDefault(
                x => x.Name == vehicleList.SelectedItem.ToString()
            );


            if (vehicle != null)
            {
                infoLabel.Text =
                    "Araç:\n" +
                    vehicle.Name +
                    "\n\nModel:\n" +
                    vehicle.Model +
                    "\n\nTür:\n" +
                    vehicle.Type;
            }
        }


        private void CopyButton_Click(object sender, EventArgs e)
        {
            if (vehicleList.SelectedItem == null)
                return;


            var vehicle = vehicles.FirstOrDefault(
                x => x.Name == vehicleList.SelectedItem.ToString()
            );


            if (vehicle != null)
            {
                Clipboard.SetText(vehicle.Model);
                MessageBox.Show(
                    "Model kodu kopyalandı: " + vehicle.Model
                );
            }
        }
    }


    public class Vehicle
    {
        public string Name { get; set; } = "";
        public string Model { get; set; } = "";
        public string Type { get; set; } = "";
    }
}
