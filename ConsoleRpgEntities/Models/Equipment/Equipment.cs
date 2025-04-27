using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRpgEntities.Models.Equipment
{
    public class Equipment
    {
        public int Id { get; set; }
        public virtual Weapon? Weapon { get; set; }
        public virtual Armor? Armor { get; set; }
    }
    public class Weapon
    {
        public int WeaponId { get; set; }
        public string WeaponName { get; set; } = string.Empty;
        public int AttackPower { get; set; }
    }
    public class Armor
    {
        public int ArmorId { get; set; }
        public string ArmorName { get; set; } = string.Empty;
        public int DefensePower { get; set; }
    }

}
