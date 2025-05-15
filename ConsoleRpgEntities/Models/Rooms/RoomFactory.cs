using ConsoleRpgEntities.Models.Rooms;

namespace ConsoleRpgEntities.Models.Rooms;

public class RoomFactory : IRoomFactory
{
    public IRoom CreateRoom(string roomType)
    {
        // You can expand this switch as you add more room types
        return roomType.ToLower() switch
        {
            "bedroom" => new Bedroom("Bedroom", "A cozy bedroom with a soft bed."),
            "kitchen" => new Kitchen("Kitchen", "A kitchen filled with the aroma of food."),
            "dungeon" => new Room { Name = "Dungeon", Description = "A dark, damp dungeon." },
            "library" => new Room { Name = "Library", Description = "A quiet library full of books." },
            "armory" => new Room { Name = "Armory", Description = "A room lined with weapons and armor." },
            "garden" => new Room { Name = "Garden", Description = "A lush garden with blooming flowers." },
            "entrance" => new Room { Name = "Entrance", Description = "The main entrance to the building." },
            "treasure" => new Room { Name = "Treasure Room", Description = "A room glittering with treasure." },
            _ => new Room { Name = "Unknown", Description = "An unremarkable room." }
        };
    }
}
