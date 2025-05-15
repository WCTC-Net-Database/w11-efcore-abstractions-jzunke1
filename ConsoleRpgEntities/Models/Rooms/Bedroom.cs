using System.Collections.Generic;
using ConsoleRpgEntities.Models.Rooms;
using ConsoleRpgEntities.Models.Characters;

namespace ConsoleRpgEntities.Models.Rooms
{
    public class Bedroom : IRoom
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IRoom? West { get; set; }
        public IRoom? East { get; set; }
        public IRoom? North { get; set; }
        public IRoom? South { get; set; }
        public List<ICharacter> Characters { get; set; }

        public Bedroom(string name, string description)
        {
            Name = name;
            Description = description;
            Characters = new List<ICharacter>();
        }

        public void Enter()
        {
            // Output logic should be handled in the UI/service layer
        }

        public void AddCharacter(ICharacter character)
        {
            Characters.Add(character);
        }

        public void RemoveCharacter(ICharacter character)
        {
            Characters.Remove(character);
        }

        public void Move(string? direction)
        {
            // Implement movement logic if needed
        }
    }
}
