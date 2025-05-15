using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRpgEntities.Models.Characters.Monsters;
using ConsoleRpgEntities.Models.Characters;

namespace ConsoleRpgEntities.Models.Rooms
{
    public class Room : IRoom
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public virtual List<Player> Players { get; set; } = new();
        public virtual List<Monster> Monsters { get; set; } = new();

        public List<ICharacter> Characters => Players.Cast<ICharacter>().Concat(Monsters.Cast<ICharacter>()).ToList();
        public IRoom? North { get; set; }
        public IRoom? South { get; set; }
        public IRoom? East { get; set; }
        public IRoom? West { get; set; }

        public void Enter()
        {
            Console.WriteLine($"You have entered the {Name}.");
            Console.WriteLine(Description);
            Console.WriteLine("Monsters in the room:");
            foreach (var monster in Monsters)
            {
                Console.WriteLine($"- {monster.Name}");
            }
        }

        public void AddCharacter(ICharacter character)
        {
            if (character is Player player)
            {
                Players.Add(player);
            }
            else if (character is Monster monster)
            {
                Monsters.Add(monster);
            }
        }

        public void RemoveCharacter(ICharacter character)
        {
            if (character is Player player)
            {
                Players.Remove(player);
            }
            else if (character is Monster monster)
            {
                Monsters.Remove(monster);
            }
        }

        public void Move(string? direction)
        {
            IRoom? nextRoom = null;
            switch (direction?.ToLower())
            {
                case "north":
                    nextRoom = North;
                    break;
                case "south":
                    nextRoom = South;
                    break;
                case "east":
                    nextRoom = East;
                    break;
                case "west":
                    nextRoom = West;
                    break;
                default:
                    Console.WriteLine("Invalid direction.");
                    return;
            }
            if (nextRoom != null)
            {
                Console.WriteLine($"You move {direction} to the {nextRoom.Name}.");
                nextRoom.Enter();
            }
            else
            {
                Console.WriteLine("You can't go that way.");
            }
        }
    }
}