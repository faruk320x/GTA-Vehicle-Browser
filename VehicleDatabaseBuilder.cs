using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class Program
{
    static void Main()
    {
        string source = "vehicles_source.txt";

        if (!File.Exists(source))
        {
            Console.WriteLine("vehicles_source.txt bulunamadı!");
            Console.ReadKey();
            return;
        }

        List<Vehicle> vehicles = new();


        foreach (string line in File.ReadAllLines(source))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;


            string[] parts = line.Split('|');


            if (parts.Length < 3)
                continue;


            vehicles.Add(new Vehicle
            {
                Name = parts[0],
                Model = parts[1],
                Type = parts[2],
                Image = parts[1] + ".jpg"
            });
        }


        string json =
            JsonSerializer.Serialize(
                vehicles,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });


        File.WriteAllText(
            "vehicles.json",
            json);


        Console.WriteLine(
            vehicles.Count + " araç oluşturuldu."
        );

        Console.ReadKey();
    }
}


class Vehicle
{
    public string Name { get; set; } = "";
    public string Model { get; set; } = "";
    public string Type { get; set; } = "";
    public string Image { get; set; } = "";
}
