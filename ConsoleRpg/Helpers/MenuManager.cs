using ConsoleRpg.Services;
using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Abilities.PlayerAbilities;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Equipment;
using ConsoleRpgEntities.Models.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using W8_assignment_template.Helpers;
using ConsoleRpgShared.Managers;

namespace ConsoleRpg.Helpers;

public class MenuManager
{
    private readonly OutputManager _outputManager;
    private readonly List<string> _characters;
    private readonly GameContext _gameContext;

    public MenuManager(OutputManager outputManager, GameContext gameContext)
    {
        _outputManager = outputManager;
        _characters = new List<string>();
        _gameContext = gameContext;
    }

    public bool ShowMainMenu()
    {
        _outputManager.WriteLine("Welcome to the RPG Game!", ConsoleColor.Yellow);
        _outputManager.WriteLine("1. Start Game", ConsoleColor.Cyan);
        _outputManager.WriteLine("2. Manage Characters", ConsoleColor.Cyan);
        _outputManager.WriteLine("3. Manage Rooms", ConsoleColor.Cyan);
        _outputManager.WriteLine("4. Exit", ConsoleColor.Cyan);
        _outputManager.Display();

        return HandleMainMenuInput();
    }
    private bool HandleMainMenuInput()
    {
        while (true)
        {
            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    _outputManager.WriteLine("Starting game...", ConsoleColor.Green);
                    _outputManager.Display();
                    return true;
                case "2":
                    _outputManager.WriteLine("Opening Character Manager...", ConsoleColor.Green);
                    _outputManager.Display();
                    ShowCharacterManagerMenu();
                    return true;
                case "3":
                    _outputManager.WriteLine("Opening Room Manager...", ConsoleColor.Green);
                    _outputManager.Display();
                    ShowRoomManagerMenu();
                    return true;
                case "4":
                    _outputManager.WriteLine("Exiting game...", ConsoleColor.Red);
                    _outputManager.Display();
                    Environment.Exit(0);
                    return false;
                default:
                    _outputManager.WriteLine("Invalid selection. Please choose 1 or 2.", ConsoleColor.Red);
                    _outputManager.Display();
                    break;
            }
        }
    }


    public bool ShowCharacterManagerMenu()
    {
        _outputManager.WriteLine("Character Manager Menu!", ConsoleColor.Yellow);
        _outputManager.WriteLine("1. Add a Character", ConsoleColor.Cyan);
        _outputManager.WriteLine("2. Edit a Character", ConsoleColor.Cyan);
        _outputManager.WriteLine("3. Display all Characters", ConsoleColor.Cyan);
        _outputManager.WriteLine("4. Search for a Character", ConsoleColor.Cyan);
        _outputManager.WriteLine("5. Add Ability to a Character", ConsoleColor.Cyan);
        _outputManager.WriteLine("6. Display Character Abilities", ConsoleColor.Cyan);
        _outputManager.WriteLine("7. Exit", ConsoleColor.Cyan);
        _outputManager.Display();

        return HandleCharacterManagerMenuInput();
    }
    private bool HandleCharacterManagerMenuInput()
    {
        while (true)
        {
            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    AddCharacter();
                    break;
                case "2":
                    EditCharacter();
                    break;
                case "3":
                    DisplayAllCharacters();
                    break;
                case "4":
                    SearchForCharacter();
                    break;
                case "5":
                    AddAbilityToCharacter();
                    break;
                case "6":
                    DisplayCharacterAbilities();
                    break;
                case "7":
                    _outputManager.WriteLine("Exiting Character Manager...", ConsoleColor.Red);
                    _outputManager.Display();
                    return false;
                default:
                    _outputManager.WriteLine("Invalid selection. Please choose 1, 2, 3, 4, 5, 6, or 7.", ConsoleColor.Red);
                    _outputManager.Display();
                    break;
            }
        }
    }

    public bool ShowRoomManagerMenu()
    {
        _outputManager.WriteLine("Character Room Menu!", ConsoleColor.Yellow);
        _outputManager.WriteLine("1. Add a Room", ConsoleColor.Cyan);
        _outputManager.WriteLine("2. Display Room Details", ConsoleColor.Cyan);
        _outputManager.WriteLine("3. Exit", ConsoleColor.Cyan);
        _outputManager.Display();

        return HandleRoomManagerMenuInput();
    }
    private bool HandleRoomManagerMenuInput()
    {
        while (true)
        {
            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    AddNewRoom();
                    break;
                case "2":
                    DisplayRoomDetails();
                    break;
                case "3":
                    _outputManager.WriteLine("Exiting Room Menu...", ConsoleColor.Red);
                    _outputManager.Display();
                    return false;
                default:
                    _outputManager.WriteLine("Invalid selection. Please choose 1, 2, or 3.", ConsoleColor.Red);
                    _outputManager.Display();
                    break;
            }
        }
    }



    // ========================================== Basic Functionality ==============================================
    public void AddCharacter()
    {
        Console.Write("Enter character name: ");
        string name = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter character health: ");
        if (!int.TryParse(Console.ReadLine(), out int health))
        {
            _outputManager.WriteLine("Invalid health value.", ConsoleColor.Red);
            return;
        }

        Console.Write("Enter character weapon name (leave blank for no weapon): ");
        string? weaponName = Console.ReadLine();
        Weapon? weapon = null;
        if (!string.IsNullOrWhiteSpace(weaponName))
        {
            Console.Write("Enter damage of weapon: ");
            int weaponDamage = 0;
            int.TryParse(Console.ReadLine(), out weaponDamage);

            weapon = _gameContext.Weapons.FirstOrDefault(w => w.WeaponName == weaponName);
            if (weapon == null)
            {
                weapon = new Weapon { WeaponName = weaponName, AttackPower = weaponDamage };
                _gameContext.Weapons.Add(weapon);
            }
            else
            {
                weapon.AttackPower = weaponDamage;
            }
        }

        Console.Write("Enter character armor name (leave blank for no armor): ");
        string? armorName = Console.ReadLine();
        Armor? armor = null;
        if (!string.IsNullOrWhiteSpace(armorName))
        {
            Console.Write("Enter protection of armor: ");
            int armorProtection = 0;
            int.TryParse(Console.ReadLine(), out armorProtection);

            armor = _gameContext.Armors.FirstOrDefault(a => a.ArmorName == armorName);
            if (armor == null)
            {
                armor = new Armor { ArmorName = armorName, DefensePower = armorProtection };
                _gameContext.Armors.Add(armor);
            }
            else
            {
                armor.DefensePower = armorProtection;
            }
        }

        // Equipment
        var equipment = new Equipment { Weapon = weapon, Armor = armor };
        _gameContext.Equipment.Add(equipment);

        // Item (if you have an Item property, set it here)
        Console.Write("Enter characters item (leave blank for no item): ");
        string? item = Console.ReadLine();

        var player = new Player
        {
            Name = name,
            Health = health,
            Equipment = equipment,
            // Item = item // Uncomment if you have an Item property
        };

        _gameContext.Players.Add(player);
        _gameContext.SaveChanges();

        _outputManager.WriteLine("Character added successfully.", ConsoleColor.Green);
        _outputManager.Display();
    }



    public void EditCharacter()
    {
        Console.Write("Enter character name to edit: ");
        string name = Console.ReadLine() ?? string.Empty;

        var player = _gameContext.Players
            .Include(p => p.Equipment)
            .ThenInclude(e => e.Weapon)
            .Include(p => p.Equipment)
            .ThenInclude(e => e.Armor)
            .FirstOrDefault(p => p.Name == name);

        if (player == null)
        {
            _outputManager.WriteLine($"Character '{name}' not found.", ConsoleColor.Red);
            _outputManager.Display();
            return;
        }

        Console.WriteLine($"Editing character: {name}");

        Console.Write("Enter new character health: ");
        if (!int.TryParse(Console.ReadLine(), out int health))
        {
            _outputManager.WriteLine("Invalid health value.", ConsoleColor.Red);
            return;
        }
        player.Health = health;

        // Weapon
        Console.Write("Enter new character weapon name (leave blank for no weapon): ");
        string? weaponName = Console.ReadLine();
        Weapon? weapon = null;
        if (!string.IsNullOrWhiteSpace(weaponName))
        {
            Console.Write("Enter new damage of weapon: ");
            int weaponDamage = 0;
            int.TryParse(Console.ReadLine(), out weaponDamage);

            // Try to find an existing weapon or create a new one
            weapon = _gameContext.Weapons.FirstOrDefault(w => w.WeaponName == weaponName);
            if (weapon == null)
            {
                weapon = new Weapon { WeaponName = weaponName, AttackPower = weaponDamage };
                _gameContext.Weapons.Add(weapon);
            }
            else
            {
                weapon.AttackPower = weaponDamage;
            }
        }

        // Armor
        Console.Write("Enter new character armor name (leave blank for no armor): ");
        string? armorName = Console.ReadLine();
        Armor? armor = null;
        if (!string.IsNullOrWhiteSpace(armorName))
        {
            Console.Write("Enter new protection of armor: ");
            int armorProtection = 0;
            int.TryParse(Console.ReadLine(), out armorProtection);

            // Try to find an existing armor or create a new one
            armor = _gameContext.Armors.FirstOrDefault(a => a.ArmorName == armorName);
            if (armor == null)
            {
                armor = new Armor { ArmorName = armorName, DefensePower = armorProtection };
                _gameContext.Armors.Add(armor);
            }
            else
            {
                armor.DefensePower = armorProtection;
            }
        }

        // Equipment
        if (player.Equipment == null)
        {
            player.Equipment = new Equipment();
            _gameContext.Equipment.Add(player.Equipment);
        }
        player.Equipment.Weapon = weapon;
        player.Equipment.Armor = armor;

        // Item (if you have an Item property or collection, update it here)
        Console.Write("Enter new characters item (leave blank for no item): ");
        string? item = Console.ReadLine();
        // Example: player.Item = item; // Uncomment and adjust if you have an Item property

        _gameContext.SaveChanges();

        _outputManager.WriteLine($"Character '{name}' updated successfully.", ConsoleColor.Green);
        _outputManager.Display();
    }


    public void DisplayAllCharacters()
    {
        var players = _gameContext.Players.ToList();

        Console.WriteLine("Displaying all characters...");
        if (players.Count == 0)
        {
            _outputManager.WriteLine("No characters available to display.", ConsoleColor.Red);
        }
        else
        {
            _outputManager.WriteLine("Displaying all characters:", ConsoleColor.Yellow);
            foreach (var player in players)
            {
                _outputManager.WriteLine($"{player.Name} (Health: {player.Health})", ConsoleColor.White);
            }
        }
        _outputManager.Display();
    }


    public void SearchForCharacter()
    {
        Console.Write("Enter character name to search: ");
        string name = Console.ReadLine() ?? string.Empty;

        var player = _gameContext.Players
            .Include(p => p.Equipment)
            .ThenInclude(e => e.Weapon)
            .Include(p => p.Equipment)
            .ThenInclude(e => e.Armor)
            .FirstOrDefault(p => p.Name.ToLower().Contains(name.ToLower()));

        if (player != null)
        {
            var weapon = player.Equipment?.Weapon?.WeaponName ?? "None";
            var armor = player.Equipment?.Armor?.ArmorName ?? "None";
            _outputManager.WriteLine($"Found character: {player.Name} (Health: {player.Health}, Weapon: {weapon}, Armor: {armor})", ConsoleColor.Green);
        }
        else
        {
            _outputManager.WriteLine($"Character '{name}' not found.", ConsoleColor.Red);
        }
        _outputManager.Display();
    }



    // =========================================== C level Functionality ==================================

    public void AddAbilityToCharacter()
    {
        Console.Write("Enter character name to add ability: ");
        string name = Console.ReadLine() ?? string.Empty;
        var player = _gameContext.Players
            .Include(p => p.Abilities)
            .FirstOrDefault(p => p.Name == name);
        if (player == null)
        {
            _outputManager.WriteLine($"Character '{name}' not found.", ConsoleColor.Red);
            _outputManager.Display();
            return;
        }

        Console.Write("Enter ability name: ");
        string abilityName = Console.ReadLine() ?? string.Empty;
        var ability = _gameContext.Abilities.FirstOrDefault(a => a.Name == abilityName);
        if (ability == null)
        {
            ability = new ShoveAbility { Name = abilityName };
            _gameContext.Abilities.Add(ability);
        }

        player.Abilities = player.Abilities.Append(ability).ToList();
        _gameContext.SaveChanges();

        _outputManager.WriteLine($"Ability '{abilityName}' added to character '{name}'.", ConsoleColor.Green);
        _outputManager.Display();
    }


    public void DisplayCharacterAbilities()
    {
        Console.Write("Enter character name to display abilities: ");
        string name = Console.ReadLine() ?? string.Empty;
        var player = _gameContext.Players
            .Include(p => p.Abilities)
            .FirstOrDefault(p => p.Name == name);
        if (player == null)
        {
            _outputManager.WriteLine($"Character '{name}' not found.", ConsoleColor.Red);
            _outputManager.Display();
            return;
        }

        if (!player.Abilities.Any())
        {
            _outputManager.WriteLine($"Character '{name}' has no abilities.", ConsoleColor.Yellow);
        }
        else
        {
            _outputManager.WriteLine($"Abilities for character '{name}':", ConsoleColor.Yellow);
            foreach (var ability in player.Abilities)
            {
                _outputManager.WriteLine($"- {ability.Name}", ConsoleColor.White);
            }
        }
        _outputManager.Display();
    }

    // ========================================= B level Functionality =========================================================
    public void AddNewRoom()
    {
        Console.WriteLine("Enter room name: ");
        string roomName = Console.ReadLine() ?? string.Empty;

        Console.WriteLine("Enter room description:");
        string roomDescription = Console.ReadLine() ?? string.Empty;

        var room = new Room
        {
            Name = roomName,
            Description = roomDescription
        };

        Console.Write("Do you want to add a monster to this room? (yes/no): ");
        string addMonsterResponse = Console.ReadLine() ?? string.Empty;
        if (addMonsterResponse.Equals("yes", StringComparison.OrdinalIgnoreCase))
        {
            Console.Write("Enter Monster ID to add: ");
            if (int.TryParse(Console.ReadLine(), out int monsterId))
            {
                var monster = _gameContext.Monsters.Find(monsterId);
                if (monster != null)
                {
                    room.Monsters.Add(monster);
                    _outputManager.WriteLine($"Monster '{monster.Name}' added to the room.", ConsoleColor.Green);
                }
                else
                {
                    _outputManager.WriteLine("Monster not found.", ConsoleColor.Red);
                }
            }
            else
            {
                _outputManager.WriteLine("Invalid Monster ID.", ConsoleColor.Red);
            }
        }

        _gameContext.Add(room);
        _gameContext.SaveChanges();

        _outputManager.WriteLine($"Room '{room.Name}' added successfully.", ConsoleColor.Green);
        _outputManager.Display();

    }

    public void DisplayRoomDetails()
    {
        Console.Write("Enter Room ID to display: ");
        if (int.TryParse(Console.ReadLine(), out int roomId))
        {
            var room = _gameContext.Set<Room>()
                .Include(r => r.Players)
                .Include(r => r.Monsters)
                .FirstOrDefault(r => r.Id == roomId);

            if (room != null)
            {
                _outputManager.WriteLine($"Room Name: {room.Name}", ConsoleColor.Yellow);
                _outputManager.WriteLine($"Description: {room.Description}", ConsoleColor.White);

                if (room.Players.Any())
                {
                    _outputManager.WriteLine("Players in the room:", ConsoleColor.Cyan);
                    foreach (var player in room.Players)
                    {
                        _outputManager.WriteLine($"- {player.Name} (Health: {player.Health})", ConsoleColor.White);
                    }
                }
                else
                {
                    _outputManager.WriteLine("No players in the room.", ConsoleColor.Red);
                }

                if (room.Monsters.Any())
                {
                    _outputManager.WriteLine("Monsters in the room:", ConsoleColor.Cyan);
                    foreach (var monster in room.Monsters)
                    {
                        _outputManager.WriteLine($"- {monster.Name} (Health: {monster.Health})", ConsoleColor.White);
                    }
                }
                else
                {
                    _outputManager.WriteLine("No monsters in the room.", ConsoleColor.Red);
                }
            }
            else
            {
                _outputManager.WriteLine("Room not found.", ConsoleColor.Red);
            }
        }
        else
        {
            _outputManager.WriteLine("Invalid Room ID.", ConsoleColor.Red);
        }

        _outputManager.Display();
    }
    // ============================================= A level Functionality ==========================================================



    // ========================================= A+ level Functionality ============================================================

}
