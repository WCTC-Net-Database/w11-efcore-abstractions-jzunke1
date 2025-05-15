using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRpgEntities.Models.Characters;

namespace ConsoleRpgEntities.Models.Rooms
{
    public interface IRoom
    {
        string Name { get; }
        string Description { get; }
        List<ICharacter> Characters { get; }
        IRoom? North { get; set; }
        IRoom? South { get; set; }
        IRoom? East { get; set; }
        IRoom? West { get; set; }
        void Enter();
        void AddCharacter(ICharacter character);
        void RemoveCharacter(ICharacter character);
        void Move(string? direction);
    }

}
