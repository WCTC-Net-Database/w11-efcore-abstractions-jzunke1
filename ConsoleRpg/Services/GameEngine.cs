using ConsoleRpg.Helpers;
using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Characters.Monsters;
using ConsoleRpgEntities.Models.Rooms;
using ConsoleRpgShared.Managers;
using W8_assignment_template.Helpers;

namespace ConsoleRpg.Services;

public class GameEngine
{
    private readonly GameContext _context;
    private readonly MenuManager _menuManager;
    private readonly OutputManager _outputManager;
    private readonly IRoomFactory _roomFactory;
    private readonly MapManager _mapManager;
    private List<IRoom> _rooms;

    private Player? _player;
    private Monster? _goblin;

    public GameEngine(GameContext context, MenuManager menuManager, OutputManager outputManager, IRoomFactory roomFactory)
    {
        _menuManager = menuManager;
        _outputManager = outputManager;
        _context = context;
        _roomFactory = roomFactory;
        _rooms = new List<IRoom>();
        _mapManager = new MapManager(_outputManager);
    }

    public void Run()
    {
        if (_menuManager.ShowMainMenu())
        {
            SetupGame();
        }
    }

    private void GameLoop()
    {
        while (true)
        {
            _mapManager.DisplayMap();
            _outputManager.WriteLine("Choose an action:", ConsoleColor.Cyan);
            _outputManager.WriteLine("1. Move North");
            _outputManager.WriteLine("2. Move South");
            _outputManager.WriteLine("3. Move East");
            _outputManager.WriteLine("4. Move West");

            // Check if there are characters in the current room to attack
            if (_player.CurrentRoom.Characters.Any(c => c != _player))
            {
                _outputManager.WriteLine("5. Attack");
            }

            _outputManager.WriteLine("6. Exit Game");

            _outputManager.Display();

            var input = Console.ReadLine();

            string? direction = null;
            switch (input)
            {
                case "1":
                    direction = "north";
                    break;
                case "2":
                    direction = "south";
                    break;
                case "3":
                    direction = "east";
                    break;
                case "4":
                    direction = "west";
                    break;
                case "5":
                    if (_player.CurrentRoom.Characters.Any(c => c != _player))
                    {
                        _outputManager.WriteLine("Choose attack type:", ConsoleColor.Cyan);
                        _outputManager.WriteLine("1. Attack with weapon");
                        _outputManager.WriteLine("2. Attack with ability");
                        _outputManager.Display();

                        var attackType = Console.ReadLine();
                        if (attackType == "1")
                        {
                            AttackWithWeapon();
                        }
                        else if (attackType == "2")
                        {
                            AttackWithAbility();
                        }
                        else
                        {
                            _outputManager.WriteLine("Invalid attack type selection.", ConsoleColor.Red);
                        }
                    }
                    else
                    {
                        _outputManager.WriteLine("No characters to attack.", ConsoleColor.Red);
                    }
                    break;
                case "6":
                    _outputManager.WriteLine("Exiting game...", ConsoleColor.Red);
                    _outputManager.Display();
                    Environment.Exit(0);
                    break;
                default:
                    _outputManager.WriteLine("Invalid selection. Please choose a valid option.", ConsoleColor.Red);
                    break;
            }

            // Update map manager with the current room after movement
            if (!string.IsNullOrEmpty(direction))
            {
                _outputManager.Clear();
                _player.Move(direction);
                _mapManager.UpdateCurrentRoom(_player.CurrentRoom);
            }
        }
    }

    private void AttackCharacter()
    {
        throw new NotImplementedException();
    }

    private void AttackCharacter(Player player, Monster monster)
    {
        int playerAttackPower = player.Attack();
        monster.Health -= playerAttackPower;
        Console.WriteLine($"{player.Name} attacked {monster.Name} with {player.Equipment?.Weapon?.WeaponName ?? "a fist"} for {playerAttackPower} damage!");

        if (monster.Health <= 0)
        {
            Console.WriteLine($"{monster.Name} has been defeated!");
            return;
        }

        int monsterAttackPower = 15;
        player.TakeDamage(monsterAttackPower);
        Console.WriteLine($"{monster.Name} attacked {player.Name} for {monsterAttackPower} damage!");

        if (player.Health <= 0)
        {
            Console.WriteLine($"{player.Name} has been defeated!");
            return;
        }
    }

    private void AttackWithAbility()
    {
        if (_player == null || _player.CurrentRoom == null)
        {
            _outputManager.WriteLine("Player or current room is not set.", ConsoleColor.Red);
            return;
        }

        var target = _player.CurrentRoom.Characters.FirstOrDefault(c => c != _player) as ITargetable;
        if (target == null)
        {
            _outputManager.WriteLine("No target to attack in this room.", ConsoleColor.Red);
            return;
        }

        var abilities = _player.Abilities.ToList();
        if (!abilities.Any())
        {
            _outputManager.WriteLine("You have no abilities to use.", ConsoleColor.Red);
            return;
        }

        _outputManager.WriteLine("Choose an ability:", ConsoleColor.Cyan);
        for (int i = 0; i < abilities.Count; i++)
        {
            _outputManager.WriteLine($"{i + 1}. {abilities[i].Name}");
        }
        _outputManager.Display();

        if (int.TryParse(Console.ReadLine(), out int abilityIndex) &&
            abilityIndex > 0 && abilityIndex <= abilities.Count)
        {
            var chosenAbility = abilities[abilityIndex - 1];
            _player.UseAbility(chosenAbility, target);
            // Display the name using ICharacter if possible
            string targetName = (target is ICharacter characterTarget) ? characterTarget.Name : "target";
            _outputManager.WriteLine($"{_player.Name} used {chosenAbility.Name} on {targetName}!", ConsoleColor.Green);
        }
        else
        {
            _outputManager.WriteLine("Invalid ability selection.", ConsoleColor.Red);
        }
    }

    private void AttackWithWeapon()
    {
        if (_player == null || _player.CurrentRoom == null)
        {
            _outputManager.WriteLine("Player or current room is not set.", ConsoleColor.Red);
            return;
        }

        var target = _player.CurrentRoom.Characters.FirstOrDefault(c => c != _player) as ITargetable;
        if (target != null)
        {
            int damage = _player.Attack(target);
            string targetName = (target is ICharacter characterTarget) ? characterTarget.Name : "target";
            _outputManager.WriteLine($"{_player.Name} attacked {targetName} with their weapon for {damage} damage!", ConsoleColor.Green);
        }
        else
        {
            _outputManager.WriteLine("No valid target found.", ConsoleColor.Red);
        }
    }

    private void SetupGame()
    {
        _player = _context.Players.OfType<Player>().FirstOrDefault();
        if (_player == null)
        {
            _outputManager.WriteLine("No player found in the database. Please create a player first.", ConsoleColor.Red);
            return;
        }
        _outputManager.WriteLine($"{_player.Name} has entered the game.", ConsoleColor.Green);

        var startingRoom = SetupRooms();
        startingRoom.AddCharacter(_player);
        _player.CurrentRoom = startingRoom;
        _mapManager.UpdateCurrentRoom(startingRoom);

        // Load monsters into random rooms 
        LoadMonsters();

        // Pause before starting the game loop
        Thread.Sleep(500);
        GameLoop();
    }

    private void LoadMonsters()
    {
        _goblin = _context.Monsters.OfType<Goblin>().FirstOrDefault();
        if (_goblin == null)
        {
            _outputManager.WriteLine("No goblin found in the database. Please add a goblin first.", ConsoleColor.Red);
            return;
        }

        var randomRoom = _rooms[new Random().Next(_rooms.Count)];
        randomRoom.AddCharacter(_goblin);
        _outputManager.WriteLine($"{_goblin.Name} has been placed in the {randomRoom.Name}.");
    }

    private IRoom SetupRooms()
    {
        // Create all rooms
        var kitchen = _roomFactory.CreateRoom("kitchen");
        var dungeon = _roomFactory.CreateRoom("dungeon");
        var treasureRoom = _roomFactory.CreateRoom("treasure");
        var bedroom = _roomFactory.CreateRoom("bedroom");
        var library = _roomFactory.CreateRoom("library");
        var entrance = _roomFactory.CreateRoom("entrance");
        var garden = _roomFactory.CreateRoom("garden");
        var armory = _roomFactory.CreateRoom("armory");

        // Map positions (row, col)
        // [0,0] kitchen   [0,1] x        [0,2] x        [0,3] x
        // [1,0] dungeon   [1,1] treasure [1,2] bedroom  [1,3] x
        // [2,0] library   [2,1] entrance [2,2] garden   [2,3] x
        // [3,0] armory    [3,1] x        [3,2] x        [3,3] x

        _mapManager.SetRoomPosition(kitchen, 0, 0);
        _mapManager.SetRoomPosition(dungeon, 1, 0);
        _mapManager.SetRoomPosition(treasureRoom, 1, 1);
        _mapManager.SetRoomPosition(bedroom, 1, 2);
        _mapManager.SetRoomPosition(library, 2, 0);
        _mapManager.SetRoomPosition(entrance, 2, 1);
        _mapManager.SetRoomPosition(garden, 2, 2);
        _mapManager.SetRoomPosition(armory, 3, 0);

        // Set bidirectional links

        // Row 0
        kitchen.South = dungeon;
        dungeon.North = kitchen;

        // Row 1
        dungeon.East = treasureRoom;
        treasureRoom.West = dungeon;

        treasureRoom.East = bedroom;
        bedroom.West = treasureRoom;

        dungeon.South = library;
        library.North = dungeon;

        // Row 2
        library.East = entrance;
        entrance.West = library;

        entrance.East = garden;
        garden.West = entrance;

        bedroom.South = garden;
        garden.North = bedroom;

        // Row 3
        library.South = armory;
        armory.North = library;

        // Store rooms in a list for later use
        _rooms = new List<IRoom>
    {
        kitchen, dungeon, treasureRoom, bedroom, library, entrance, garden, armory
    };

        // Return the entrance as the starting room
        return entrance;
    }


}
