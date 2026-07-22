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
        ComboBox categoryBox;
        ListBox vehicleList;
        PictureBox vehicleImage;

        Label infoLabel;
        Label countLabel;

        Button copyButton;


        List<Vehicle> vehicles = new();
        List<Vehicle> filteredVehicles = new();



        public MainForm()
        {
            Text = "GTA V Vehicle Browser";
            Width = 950;
            Height = 650;



            categoryBox = new ComboBox();
            categoryBox.Left = 20;
            categoryBox.Top = 20;
            categoryBox.Width = 250;
            categoryBox.DropDownStyle =
                ComboBoxStyle.DropDownList;

            categoryBox.SelectedIndexChanged += FilterChanged;



            searchBox = new TextBox();
            searchBox.Left = 300;
            searchBox.Top = 20;
            searchBox.Width = 550;
            searchBox.Height = 25;
            searchBox.PlaceholderText =
                "Araç ara...";

            searchBox.TextChanged += FilterChanged;



            vehicleList = new ListBox();
            vehicleList.Left = 20;
            vehicleList.Top = 70;
            vehicleList.Width = 300;
            vehicleList.Height = 450;

            vehicleList.SelectedIndexChanged +=
                VehicleSelected;




            vehicleImage = new PictureBox();
            vehicleImage.Left = 370;
            vehicleImage.Top = 70;
            vehicleImage.Width = 350;
            vehicleImage.Height = 260;

            vehicleImage.SizeMode =
                PictureBoxSizeMode.Zoom;




            infoLabel = new Label();
            infoLabel.Left = 370;
            infoLabel.Top = 360;
            infoLabel.Width = 400;
            infoLabel.Height = 100;




            copyButton = new Button();
            copyButton.Left = 370;
            copyButton.Top = 480;
            copyButton.Width = 220;

            copyButton.Text =
                "Model Kopyala";

            copyButton.Click += CopyModel;




            countLabel = new Label();
            countLabel.Left = 20;
            countLabel.Top = 540;
            countLabel.Width = 300;
            countLabel.Height = 50;

            countLabel.Text =
                "Toplam Araç: 0";




            Controls.Add(categoryBox);
            Controls.Add(searchBox);
            Controls.Add(vehicleList);
            Controls.Add(vehicleImage);
            Controls.Add(infoLabel);
            Controls.Add(copyButton);
            Controls.Add(countLabel);



            LoadVehicles();
        }
                void LoadVehicles()
        {
            string file =
                Path.Combine(
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


            string json =
                File.ReadAllText(file);


            vehicles =
                JsonSerializer.Deserialize<List<Vehicle>>(json)
                ?? new List<Vehicle>();


            LoadCategories();

            ApplyFilter();
        }




        void LoadCategories()
        {
            categoryBox.Items.Clear();

            categoryBox.Items.Add("Tümü");


            foreach (var type in vehicles
                .Select(x => x.Type)
                .Distinct()
                .OrderBy(x => x))
            {
                categoryBox.Items.Add(type);
            }


            categoryBox.SelectedIndex = 0;
        }





        void FilterChanged(
            object? sender,
            EventArgs e)
        {
            ApplyFilter();
        }





        void ApplyFilter()
        {
            string search =
                searchBox.Text
                .ToLower();


            string category =
                categoryBox.SelectedItem?
                .ToString()
                ?? "Tümü";



            filteredVehicles =
                vehicles
                .Where(x =>
                    (
                    category == "Tümü"
                    ||
                    x.Type == category
                    )
                    &&
                    (
                    x.Name
                    .ToLower()
                    .Contains(search)
                    ||
                    x.Model
                    .ToLower()
                    .Contains(search)
                    )
                )
                .OrderBy(x => x.Name)
                .ToList();




            vehicleList.Items.Clear();


            foreach (var vehicle in filteredVehicles)
            {
                vehicleList.Items.Add(
                    vehicle.Name
                );
            }



            countLabel.Text =
                "Toplam Araç: "
                + vehicles.Count
                +
                "\nGösterilen: "
                + filteredVehicles.Count;
        }
                void VehicleSelected(
            object? sender,
            EventArgs e)
        {
            if (vehicleList.SelectedIndex < 0)
                return;


            Vehicle vehicle =
                filteredVehicles[
                    vehicleList.SelectedIndex
                ];


            infoLabel.Text =
                "Ad: " + vehicle.Name
                +
                "\nModel: " + vehicle.Model
                +
                "\nKategori: " + vehicle.Type;



            string imagePath =
                Path.Combine(
                    Application.StartupPath,
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





        void CopyModel(
            object? sender,
            EventArgs e)
        {
            if (vehicleList.SelectedIndex < 0)
                return;


            Vehicle vehicle =
                filteredVehicles[
                    vehicleList.SelectedIndex
                ];


            Clipboard.SetText(
                vehicle.Model
            );


            MessageBox.Show(
                vehicle.Model
                +
                " kopyalandı"
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
