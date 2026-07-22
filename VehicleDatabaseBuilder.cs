using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class Program
{
    static void Main()
    {
        var vehicles = new List<Vehicle>();

        string[] models =
        {
            "adder",
            "akuma",
            "alpha",
            "baller",
            "baller2",
            "banshee",
            "banshee2",
            "bati",
            "bati2",
            "buffalo",
            "buffalo2",
            "carbonizzare",
            "cheetah",
            "comet2",
            "coquette",
            "dominator",
            "elegy",
            "elegy2",
            "entityxf",
            "feltzer2",
            "furoregt",
            "gauntlet",
            "infernus",
            "jester",
            "kuruma",
            "kuruma2",
            "massacro",
            "oracle",
            "osiris",
            "rapidgt",
            "reaper",
            "sultan",
            "sultanrs",
            "tempesta",
            "t20",
            "turismor",
            "zentorno"
        };


        foreach(var model in models)
        {
            vehicles.Add(new Vehicle
            {
                Name = model.ToUpper(),
                Model = model,
                Type = "Vehicle",
                Image = model + ".jpg"
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
            json
        );


        Console.WriteLine(
            "vehicles.json oluşturuldu."
        );
    }
}


class Vehicle
{
    public string Name {get;set;} = "";
    public string Model {get;set;} = "";
    public string Type {get;set;} = "";
    public string Image {get;set;} = "";
}
