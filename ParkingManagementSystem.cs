using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingManagementSystem
{


    class AlvinParkir
    {
        private int capacity;
        private List<ParkingSlot> slots;

        public AlvinParkir(int capacity)
        {
            this.capacity = capacity;
            slots = new List<ParkingSlot>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                slots.Add(null);
            }
        }

        public int Park(string registrationNumber, string color, string vehicleType)
        {
            for (int i = 0; i < capacity; i++)
            {
                if (slots[i] == null)
                {
                    slots[i] = new ParkingSlot(registrationNumber, color, vehicleType);
                    return i + 1; 
                }
            }
            return -1; 
        }

        public void Leave(int slotNumber)
        {
            slots[slotNumber - 1] = null;
        }

        public List<ParkingSlot> GetParkingStatus()
        {
            return slots.Where(slot => slot != null).ToList();
        }

        public int CountVehicle(string vehicleType)
        {
            return slots.Count(slot => slot.VehicleType.ToLower() == vehicleType.ToLower());
        }

        public List<string> GetRegistrationNumbersByPlateType(string plateType)
        {
            return slots.Where(slot => int.Parse(slot.RegistrationNumber.Split('-')[1]) % 2 == (plateType.ToLower() == "odd" ? 1 : 0))
                        .Select(slot => slot.RegistrationNumber).ToList();
        }

        public List<string> GetRegistrationNumbersByColor(string color)
        {
            return slots.Where(slot => slot.Color.ToLower() == color.ToLower())
                        .Select(slot => slot.RegistrationNumber).ToList();
        }

        public List<int> GetSlotNumbersByColor(string color)
        {
            return slots.Where(slot => slot.Color.ToLower() == color.ToLower())
                        .Select((slot, index) => index + 1)
                        .ToList();
        }

        public int GetSlotNumberByRegistrationNumber(string registrationNumber)
        {
            for (int i = 0; i < capacity; i++)
            {
                if (slots[i] != null && slots[i].RegistrationNumber.ToLower() == registrationNumber.ToLower())
                {
                    return i + 1;
                }
            }
            return -1; 
        }
    }

    class ParkingSlot
    {
        public string RegistrationNumber { get; }
        public string Color { get; }
        public string VehicleType { get; }

        public ParkingSlot(string registrationNumber, string color, string vehicleType)
        {
            RegistrationNumber = registrationNumber;
            Color = color;
            VehicleType = vehicleType;
        }
    }

    class Main
    {
        static void Main(string[] args)
        {
            AlvinParkir alvinParkir = null;

            while (true)
            {
                Console.Write("$ ");
                string command = Console.ReadLine();
                string[] parts = command.Split(' ');

                switch (parts[0])
                {
                    case "create_parking_lot":
                        int capacity = int.Parse(parts[1]);
                        alvinParkir = new AlvinParkir(capacity);
                        Console.WriteLine($"Created a parking lot with {capacity} slots");
                        break;
                    case "park":
                        string registrationNumber = parts[1];
                        string color = parts[3];
                        string vehicleType = parts[2];
                        if (alvinParkir == null)
                        {
                            Console.WriteLine("Parking lot hasn't Created");
                            break;
                        }
                        int slotNumber = alvinParkir.Park(registrationNumber, color, vehicleType);
                        if (slotNumber == -1)
                        {
                            Console.WriteLine("Sorry, parking lot is full");
                        }
                        else
                        {
                            Console.WriteLine($"Allocated slot number: {slotNumber}");
                        }
                        break;
                    case "leave":
                        int slot = int.Parse(parts[1]);
                        alvinParkir.Leave(slot);
                        Console.WriteLine($"Slot number {slot} is free");
                        break;
                    case "status":
                        if (alvinParkir == null)
                        {
                            Console.WriteLine("Parking lot hasn't Created");
                            break;
                        }
                        Console.WriteLine("Slot \tNo. \t\tType \t\tRegistration No Colour");
                        foreach (var slot in alvinParkir.GetParkingStatus())
                        {
                            Console.WriteLine($"{slot.SlotNumber} \t{slot.RegistrationNumber} \t\t{slot.VehicleType} \t\t{slot.Color}");
                        }
                        break;
                    case "type_of_vehicles":
                        string vehicleType = parts[1];
                        int count = alvinParkir.CountVehicle(vehicleType);
                        Console.WriteLine(count);
                        break;
                    case "registration_numbers_for_vehicles_with_ood_plate":
                        var oddPlateNumbers = alvinParkir.GetRegistrationNumbersByPlateType("odd");
                        Console.WriteLine(string.Join(", ", oddPlateNumbers));
                        break;
                    case "registration_numbers_for_vehicles_with_event_plate":
                        var evenPlateNumbers = alvinParkir.GetRegistrationNumbersByPlateType("even");
                        Console.WriteLine(string.Join(", ", evenPlateNumbers));
                        break;
                    case "registration_numbers_for_vehicles_with_colour":
                        string color = parts[1];
                        var regNumber = alvinParkir.GetRegistrationNumbersByColor(color);
                        Console.WriteLine(string.Join(", ", regNumber));
                        break;
                    case "slot_numbers_for_vehicles_with_colour":
                        string color = parts[1];
                        var slot = alvinParkir.GetSlotNumbersByColor(color);
                        Console.WriteLine(string.Join(", ", slot));
                        break;
                    case "slot_number_for_registration_number":
                        string regNumber = parts[1];
                        int slotNumber = alvinParkir.GetSlotNumberByRegistrationNumber(regNumber);
                        if (slotNumber == -1)
                            Console.WriteLine("Not found");
                        else
                            Console.WriteLine(slotNumber);
                        break;
                    case "exit":
                        Console.WriteLine("Exiting program. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid command");
                        break;
                }
            }
        }
    }
}
