using ConsoleRpg.Helpers;
using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Characters.Monsters;

namespace ConsoleRpg.Services;

public class GameEngine
{
    private readonly GameContext _context;
    private readonly MenuManager _menuManager;
    private readonly OutputManager _outputManager;

    private Player? _player;
    private Monster? _goblin;

    public GameEngine(GameContext context, MenuManager menuManager, OutputManager outputManager)
    {
        _menuManager = menuManager;
        _outputManager = outputManager;
        _context = context;
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
        _outputManager.Clear();

        while (true)
        {
            _outputManager.WriteLine("Choose an action:", ConsoleColor.Cyan);
            _outputManager.WriteLine("1. Attack");
            _outputManager.WriteLine("2. Quit");

            _outputManager.Display();

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    if (_player != null && _goblin != null)
                    {
                        AttackCharacter(_player, _goblin);
                    }
                    else
                    {
                        _outputManager.WriteLine("Player or Goblin is not initialized.", ConsoleColor.Red);
                    }
                    break;
                case "2":
                    _outputManager.WriteLine("Exiting game...", ConsoleColor.Red);
                    _outputManager.Display();
                    Environment.Exit(0);
                    break;
                default:
                    _outputManager.WriteLine("Invalid selection. Please choose 1.", ConsoleColor.Red);
                    break;
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


    private void SetupGame()
    {
        _player = _context.Players.OfType<Player>().FirstOrDefault();
        if (_player == null)
        {
            _outputManager.WriteLine("No player found in the database. Please create a player first.", ConsoleColor.Red);
            return;
        }
        _outputManager.WriteLine($"{_player.Name} has entered the game.", ConsoleColor.Green);

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
            throw new InvalidOperationException("No goblin found in the database.");
        }
    }

}
