using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleRpgEntities.Models.Rooms;

namespace ConsoleRpgEntities.Models.Characters
{
    public interface ICharacter
    {
        IRoom CurrentRoom { get; }
        int HP { get; set; }
        int Level { get; set; }
        string Name { get; set; }
        string Type { get; set; }

        void Attack(ICharacter target);
        void Move(IRoom room);
        void Move(string? direction);
    }
}
