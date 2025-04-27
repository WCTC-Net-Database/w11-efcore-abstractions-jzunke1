using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Characters;

namespace ConsoleRpgEntities.Models.Abilities.PlayerAbilities
{
    public abstract class Ability : IAbility
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AbilityType { get; set; } = string.Empty;

        public virtual IEnumerable<Player> Players { get; set; } = new List<Player>();

        protected Ability()
        {
            Players = new List<Player>();
        }

        public abstract void Activate(IPlayer user, ITargetable target);
    }
}
