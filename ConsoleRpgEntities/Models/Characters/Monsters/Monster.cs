using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Rooms;

namespace ConsoleRpgEntities.Models.Characters.Monsters
{
    public abstract class Monster : IMonster, ITargetable, ICharacter
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Health { get; set; }
        public int AggressionLevel { get; set; }
        public string MonsterType { get; set; } = string.Empty;
        public IRoom CurrentRoom { get; set; } = null!;
        public int HP { get; set; }
        public int Level { get; set; }
        public string Type { get; set; } = string.Empty;


        protected Monster()
        {

        }

        public abstract void Attack(ITargetable target);

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

        public void Attack(ICharacter target)
        {
            if (target is Player player)
            {
                int damage = 10;
                player.TakeDamage(damage);
            }
            else
            {
                throw new InvalidOperationException("Target is not a valid player.");
            }
        }
    }
}
