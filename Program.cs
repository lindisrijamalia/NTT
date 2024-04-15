using System;
using System.Collections.Generic;
using System.Linq;

class Vehicle
{
    public string RegistrationNumber { get; }
    public string Color { get; }
    public string Type { get; }

    public Vehicle(string registrationNumber, string color, string type)
    {
        RegistrationNumber = registrationNumber;
        Color = color;
        Type = type;
    }
}

class Program
{
    static void Main(string[] args)
    {
        ParkingLot parkingLot = new ParkingLot(6);

        while (true)
        {
            Console.Write("$ ");
            string command = Console.ReadLine().Trim();

            if (command.StartsWith("create_parking_lot"))
            {
                int totalLots = int.Parse(command.Split(' ')[1]);
                parkingLot = new ParkingLot(totalLots);
                Console.WriteLine($"Created a parking lot with {totalLots} slots");
            }
            else if (command.StartsWith("park"))
            {
                string[] parameters = command.Split(' ');
                string registrationNumber = parameters[1];
                string color = parameters[2];
                string vehicleType = parameters[3];
                Vehicle vehicle = new Vehicle(registrationNumber, color, vehicleType);
                int allocatedSlot = parkingLot.ParkVehicle(vehicle);
                if (allocatedSlot != -1)
                    Console.WriteLine($"Allocated slot number: {allocatedSlot}");
            }
            else if (command.StartsWith("leave"))
            {
                int slotNumber = int.Parse(command.Split(' ')[1]);
                parkingLot.Leave(slotNumber);
                Console.WriteLine($"Slot number {slotNumber} is free");
            }
            else if (command == "status")
            {
                parkingLot.PrintStatus();
            }
            else if (command.StartsWith("type_of_vehicles"))
            {
                string type = command.Split(' ')[1];
                int count = parkingLot.GetVehicleCountByType(type);
                Console.WriteLine(count);
            }
            else if (command == "registration_numbers_for_vehicles_with_odd_plate")
            {
                List<string> oddPlateNumbers = parkingLot.GetRegistrationNumbersWithOddPlate();
                Console.WriteLine(string.Join(", ", oddPlateNumbers));
            }
            else if (command == "registration_numbers_for_vehicles_with_even_plate")
            {
                List<string> evenPlateNumbers = parkingLot.GetRegistrationNumbersWithEvenPlate();
                Console.WriteLine(string.Join(", ", evenPlateNumbers));
            }
            else if (command.StartsWith("registration_numbers_for_vehicles_with_colour"))
            {
                string color = command.Split(' ')[1];
                List<string> registrationNumbers = parkingLot.GetRegistrationNumbersByColor(color);
                Console.WriteLine(string.Join(", ", registrationNumbers));
            }
            else if (command.StartsWith("slot_numbers_for_vehicles_with_colour"))
            {
                string color = command.Split(' ')[1];
                List<int> slotNumbers = parkingLot.GetSlotNumbersByColor(color);
                Console.WriteLine(string.Join(", ", slotNumbers));
            }
            else if (command.StartsWith("slot_number_for_registration_number"))
            {
                string registrationNumber = command.Split(' ')[1];
                int slotNumber = parkingLot.GetSlotNumberByRegistrationNumber(registrationNumber);
                if (slotNumber != -1)
                    Console.WriteLine(slotNumber);
                else
                    Console.WriteLine("Not found");
            }
            else if (command == "exit")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid command");
            }
        }
    }
}



class ParkingLot
{
    private int totalLots;
    private Dictionary<int, Vehicle> parkedVehicles;

    public ParkingLot(int totalLots)
    {
        this.totalLots = totalLots;
        parkedVehicles = new Dictionary<int, Vehicle>();
    }

    public int ParkVehicle(Vehicle vehicle)
    {
        for (int i = 1; i <= totalLots; i++)
        {
            if (!parkedVehicles.ContainsKey(i))
            {
                parkedVehicles[i] = vehicle;
                return i;
            }
        }
        Console.WriteLine("Sorry, parking lot is full");
        return -1;
    }

    public void Leave(int slotNumber)
    {
        if (parkedVehicles.ContainsKey(slotNumber))
            parkedVehicles.Remove(slotNumber);
    }

    public void PrintStatus()
    {
        Console.WriteLine("Slot\tNo.\tType\tRegistration No\tColour");
        foreach (var kvp in parkedVehicles)
        {
            Console.WriteLine($"{kvp.Key}\t{kvp.Value.RegistrationNumber}\t{kvp.Value.Type}\t{kvp.Value.Color}");
        }
    }

    public int GetVehicleCountByType(string type)
    {
        return parkedVehicles.Count(v => v.Value.Type == type);
    }

    public List<string> GetRegistrationNumbersWithOddPlate()
    {
        return parkedVehicles.Where(v => v.Value.RegistrationNumber.Last() % 2 != 0).Select(v => v.Value.RegistrationNumber).ToList();
    }

    public List<string> GetRegistrationNumbersWithEvenPlate()
    {
        return parkedVehicles.Where(v => v.Value.RegistrationNumber.Last() % 2 == 0).Select(v => v.Value.RegistrationNumber).ToList();
    }

    public List<string> GetRegistrationNumbersByColor(string color)
    {
        return parkedVehicles.Where(v => v.Value.Color.Equals(color, StringComparison.OrdinalIgnoreCase)).Select(v => v.Value.RegistrationNumber).ToList();
    }

    public List<int> GetSlotNumbersByColor(string color)
    {
        return parkedVehicles.Where(v => v.Value.Color.Equals(color, StringComparison.OrdinalIgnoreCase)).Select(v => v.Key).ToList();
    }

    public int GetSlotNumberByRegistrationNumber(string registrationNumber)
    {
        var vehicle = parkedVehicles.FirstOrDefault(v => v.Value.RegistrationNumber.Equals(registrationNumber, StringComparison.OrdinalIgnoreCase));
        return vehicle.Key;
    }
}
