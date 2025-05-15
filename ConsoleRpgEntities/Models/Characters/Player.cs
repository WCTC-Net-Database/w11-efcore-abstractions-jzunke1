using System.Numerics;
using ConsoleRpgEntities.Models.Abilities.PlayerAbilities;
using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Equipment;
using ConsoleRpgEntities.Models.Rooms;

namespace ConsoleRpgEntities.Models.Characters
{
    public class Player : ITargetable, IPlayer, ICharacter
    {
        public int Experience { get; set; }

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Health { get; set; }
        public int HP { get => Health; set => Health = value; }
        public int Level { get; set; }
        public string Type { get; set; } = "Player";
        public IRoom CurrentRoom { get; set; } = null!;
        public virtual IEnumerable<Ability> Abilities { get; set; } = new List<Ability>();
        public virtual ConsoleRpgEntities.Models.Equipment.Equipment? Equipment { get; set; }
        public int EquipmentId { get; set; }

        public void Move(IRoom room)
        {
            CurrentRoom = room;
        }

        public void Move(string? direction)
        {
            if (CurrentRoom != null)
            {
                CurrentRoom.Move(direction);
            }
            else
            {
                throw new InvalidOperationException("Current room is not set.");
            }
        }

        public int Attack(ITargetable target)
        {
            if (target is Player player)
            {
                int damage = Equipment?.Weapon?.AttackPower ??10;
                player.TakeDamage(damage);
                return damage;
            }
            else
            {
                throw new InvalidOperationException("Target is not a valid player.");
            }
        }

        public int Attack()
        {
            if (Equipment?.Weapon != null)
            {
                return Equipment.Weapon.AttackPower;
            }

            return 5;
        }

        public void Attack(ICharacter target)
        {
            if (target is Player player)
            {
                int damage = Equipment?.Weapon?.AttackPower ?? 10;
                player.TakeDamage(damage);
                return;
            }
            else
            {
                throw new InvalidOperationException("Target is not a valid player.");
            }
        }


        public int TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0)
            {
                Health = 0;
            }
            return Health;
        }

        public void UseAbility(IAbility ability, ITargetable target)
        {
            if (Abilities.Contains(ability))
            {
                ability.Activate(this, target);
            }
            else
            {
                Console.WriteLine($"{Name} does not have the ability {ability.Name}!");
            }
        }

        void IPlayer.Attack(ITargetable target)
        {
            Attack(target);
        }
    }
}
