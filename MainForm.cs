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
        TextBox searchBox;
        ListBox vehicleList;
        Label infoLabel;
        Button copyButton;

        List<Vehicle> vehicles = new();

        public MainForm()
        {
            Text = "GTA V Vehicle Browser";
            Width = 600;
            Height = 500;

            searchBox = new TextBox();
            searchBox.Top = 20;
            searchBox.Left = 20;
            searchBox.Width = 540;
            searchBox.PlaceholderText = "Araç ara...";
            searchBox.TextChanged += SearchBox_TextChanged;

            vehicleList = new ListBox();
            vehicleList.Top = 60;
            vehicleList.Left = 20;
            vehicleList.Width = 250;
            vehicleList.Height = 350;
            vehicleList.SelectedIndexChanged += VehicleList_SelectedIndexChanged;

            infoLabel = new Label();
            infoLabel.Top = 80;
            infoLabel.Left = 300;
            infoLabel.Width = 250;
            infoLabel.Height = 150;

            copyButton = new Button();
            copyButton.Text = "Model Kodunu Kopyala";
            copyButton.Top = 250;
            copyButton.Left = 300;
            copyButton.Click += CopyButton_Click;

            Controls.Add(searchBox);
            Controls.Add(vehicleList);
            Controls.Add(infoLabel);
            Controls.Add(copyButton);

            LoadVehicles();
        }

        void LoadVehicles()
        {
            if (File.Exists("vehicles.json"))
            {
                string json = File.ReadAllText("vehicles.json");
                vehicles = JsonSerializer.Deserialize<List<Vehicle>>(json) ?? new();
            }

            RefreshList(vehicles);
        }

        void RefreshList(List<Vehicle> list)
        {
            vehicleList.Items.Clear();

            foreach (var v in list)
            {
                vehicleList.Items.Add(v.Name);
            }
        }

        void SearchBox_TextChanged(object? sender, EventArgs e)
        {
            var result = vehicles
                .Where(x => x.Name.ToLower()
                .Contains(searchBox.Text.ToLower()))
                .ToList();

            RefreshList(result);
        }

        void VehicleList_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (vehicleList.SelectedIndex >= 0)
            {
                var v = vehicles[vehicleList.SelectedIndex];

                infoLabel.Text =
                    $"Araç:\n{v.Name}\n\nModel:\n{v.Model}\n\nTür:\n{v.Type}";
            }
        }

        void CopyButton_Click(object? sender, EventArgs e)
        {
            if (vehicleList.SelectedIndex >= 0)
            {
                Clipboard.SetText(
                    vehicles[vehicleList.SelectedIndex].Model
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
