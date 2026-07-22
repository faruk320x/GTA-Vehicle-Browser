using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Drawing;
using System.Windows.Forms;

namespace GTAVehicleBrowser
{
    public class MainForm : Form
    {
        TextBox searchBox;
        ListBox vehicleList;
        PictureBox picture;
        Label info;
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
            searchBox.Width = 800;
            searchBox.PlaceholderText = "Araç ara...";
            searchBox.TextChanged += Search;


            vehicleList = new ListBox();
            vehicleList.Left = 20;
            vehicleList.Top = 60;
            vehicleList.Width = 300;
            vehicleList.Height = 450;
            vehicleList.SelectedIndexChanged += SelectVehicle;


            picture = new PictureBox();
            picture.Left = 370;
            picture.Top = 60;
            picture.Width = 350;
            picture.Height = 250;
            picture.SizeMode = PictureBoxSizeMode.Zoom;


            info = new Label();
            info.Left = 370;
            info.Top = 330;
            info.Width = 350;
            info.Height = 100;


            copyButton = new Button();
            copyButton.Left = 370;
            copyButton.Top = 450;
            copyButton.Width = 200;
            copyButton.Text = "Model Kopyala";
            copyButton.Click += Copy;


            Controls.Add(searchBox);
            Controls.Add(vehicleList);
            Controls.Add(picture);
            Controls.Add(info);
            Controls.Add(copyButton);


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
                    "vehicles.json bulunamadı"
                );
                return;
            }


            string json = File.ReadAllText(file);


            vehicles =
                JsonSerializer.Deserialize<List<Vehicle>>(json)
                ?? new();


            FillList(vehicles);
        }


        void FillList(List<Vehicle> list)
        {
            vehicleList.Items.Clear();

            foreach(var v in list)
            {
                vehicleList.Items.Add(v.Name);
            }
        }


        void Search(object? sender, EventArgs e)
        {
            string text =
                searchBox.Text.ToLower();


            FillList(
                vehicles.Where(x =>
                x.Name.ToLower().Contains(text) ||
                x.Model.ToLower().Contains(text)
                ).ToList()
            );
        }


        Vehicle? GetVehicle()
        {
            if(vehicleList.SelectedItem == null)
                return null;


            return vehicles.FirstOrDefault(
                x => x.Name ==
                vehicleList.SelectedItem.ToString()
            );
        }


        void SelectVehicle(object? sender, EventArgs e)
        {
            var v = GetVehicle();

            if(v == null)
                return;


            info.Text =
                "Araç: " + v.Name +
                "\nModel: " + v.Model +
                "\nTür: " + v.Type;


            string img =
                Path.Combine(
                    Application.StartupPath,
                    "images",
                    v.Image
                );


            if(File.Exists(img))
            {
                picture.Image =
                    Image.FromFile(img);
            }
            else
            {
                picture.Image = null;
            }
        }


        void Copy(object? sender, EventArgs e)
        {
            var v = GetVehicle();

            if(v == null)
                return;


            Clipboard.SetText(v.Model);

            MessageBox.Show(
                v.Model + " kopyalandı"
            );
        }
    }


    public class Vehicle
    {
        public string Name {get;set;} = "";
        public string Model {get;set;} = "";
        public string Type {get;set;} = "";
        public string Image {get;set;} = "";
    }
}
