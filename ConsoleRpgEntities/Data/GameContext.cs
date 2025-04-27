using ConsoleRpgEntities.Models.Abilities.PlayerAbilities;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Characters.Monsters;
using ConsoleRpgEntities.Models.Equipment;
using Microsoft.EntityFrameworkCore;

namespace ConsoleRpgEntities.Data
{
    public class GameContext : DbContext
    {
        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<Monster> Monsters { get; set; } = null!;
        public DbSet<Ability> Abilities { get; set; } = null!;
        public DbSet<Equipment> Equipment { get; set; } = null!;
        public DbSet<Weapon> Weapons { get; set; } = null!;
        public DbSet<Armor> Armors { get; set; } = null!;

        public GameContext(DbContextOptions<GameContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure one-to-one relationship between Player and Equipment
            modelBuilder.Entity<Player>()
                .HasOne(p => p.Equipment)
                .WithOne()
                .HasForeignKey<Player>(p => p.EquipmentId);

            // Configure one-to-one relationships for Equipment with Weapon and Armor
            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Weapon)
                .WithMany()
                .HasForeignKey("WeaponId");

            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Armor)
                .WithMany()
                .HasForeignKey("ArmorId");

            modelBuilder.Entity<Player>()
                .HasOne(p => p.Equipment)
                .WithOne()
                .HasForeignKey<Player>(p => p.EquipmentId);

            // Configure TPH for Character hierarchy
            modelBuilder.Entity<Monster>()
                .HasDiscriminator<string>(m => m.MonsterType)
                .HasValue<Goblin>("Goblin");

            // Configure TPH for Ability hierarchy
            modelBuilder.Entity<Ability>()
                .HasDiscriminator<string>(pa => pa.AbilityType)
                .HasValue<ShoveAbility>("ShoveAbility");

            // Configure many-to-many relationship between Player and Abilities
            modelBuilder.Entity<Player>()
                .HasMany(p => p.Abilities)
                .WithMany(a => a.Players)
                .UsingEntity(j => j.ToTable("PlayerAbilities"));

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }


    }
}


