using ConsoleRpgEntities.Models.Abilities.PlayerAbilities;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Characters.Monsters;
using ConsoleRpgEntities.Models.Equipment;
using ConsoleRpgEntities.Models.Rooms;
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
        public DbSet<Room> Rooms { get; set; } = null!;

        public GameContext(DbContextOptions<GameContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().Ignore(p => p.CurrentRoom);
            modelBuilder.Entity<Monster>().Ignore(m => m.CurrentRoom);

            modelBuilder.Entity<Room>().Ignore(r => r.North);
            modelBuilder.Entity<Room>().Ignore(r => r.South);
            modelBuilder.Entity<Room>().Ignore(r => r.East);
            modelBuilder.Entity<Room>().Ignore(r => r.West);


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

        public void SeedDatabase()
        {
            if (Weapons.Any() || Armors.Any() || Abilities.Any() || Players.Any())
            {
                // Database already seeded
                return;
            }

            // Add Weapons
            var sword = new Weapon { WeaponName = "Sword", AttackPower = 15 };
            var bow = new Weapon { WeaponName = "Bow", AttackPower = 10 };
            var axe = new Weapon { WeaponName = "Axe", AttackPower = 20 };
            Weapons.AddRange(sword, bow, axe);

            // Add Armors
            var leatherArmor = new Armor { ArmorName = "Leather Armor", DefensePower = 5 };
            var chainmail = new Armor { ArmorName = "Chainmail", DefensePower = 10 };
            Armors.AddRange(leatherArmor, chainmail);

            // Add Players
            var player1 = new Player
            {
                Name = "Archer",
                Health = 100,
                Experience = 0,
                Equipment = new Equipment { Weapon = bow, Armor = leatherArmor },
            };

            var player2 = new Player
            {
                Name = "Warrior",
                Health = 150,
                Experience = 0,
                Equipment = new Equipment { Weapon = sword, Armor = chainmail },
            };

            Players.AddRange(player1, player2);

            // Save changes to the database
            SaveChanges();
        }


    }
}


