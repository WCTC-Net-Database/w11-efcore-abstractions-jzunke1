using ConsoleRpgEntities;
using ConsoleRpgEntities.Models.Rooms;
using ConsoleRpgEntities.Helpers;
using ConsoleRpgShared.Managers;

namespace W8_assignment_template.Helpers;

public class MapManager
{
    private const int RoomNameLength = 5;
    private const int gridRows = 4;
    private const int gridCols = 4;
    private readonly OutputManager _outputManager;
    private readonly string[,] mapGrid;
    private readonly IRoom?[,] roomGrid;
    private IRoom _currentRoom;

    private readonly Dictionary<IRoom, (int row, int col)> _roomPositions = new();

    public void SetRoomPosition(IRoom room, int row, int col)
    {
        _roomPositions[room] = (row, col);
    }


    public MapManager(OutputManager outputManager)
    {
        _currentRoom = null!;
        _outputManager = outputManager;
        mapGrid = new string[gridRows, gridCols];
        roomGrid = new IRoom?[gridRows, gridCols];
    }

    public void DisplayMap()
    {
        Console.WriteLine("Map:");
        var grid = new string[gridRows, gridCols];

        // Fill with empty
        for (int i = 0; i < gridRows; i++)
            for (int j = 0; j < gridCols; j++)
                grid[i, j] = "       ";

        // Place rooms at their fixed positions
        foreach (var kvp in _roomPositions)
        {
            var room = kvp.Key;
            var (row, col) = kvp.Value;
            if (row >= 0 && row < gridRows && col >= 0 && col < gridCols)
            {
                var roomName = room.Name.Length > RoomNameLength
                    ? room.Name.Substring(0, RoomNameLength)
                    : room.Name.PadRight(RoomNameLength);
                grid[row, col] = $"[{roomName}]";
            }
        }

        // Print the grid, highlighting the current room
        for (int i = 0; i < gridRows; i++)
        {
            for (int j = 0; j < gridCols; j++)
            {
                var room = _roomPositions.FirstOrDefault(x => x.Value == (i, j)).Key;
                if (room != null && room == _currentRoom)
                    WriteWithColor($"{grid[i, j],-7}", ConsoleColor.White);
                else
                    Console.Write($"{grid[i, j],-7}");
            }
            Console.WriteLine();
        }
    }


    private void WriteWithColor(string text, ConsoleColor color)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = originalColor;
    }

    public void UpdateCurrentRoom(IRoom currentRoom)
    {
        _currentRoom = currentRoom;
    }

    private void PlaceRoom(IRoom room, int row, int col)
    {
        if (row < 0 || row >= gridRows || col < 0 || col >= gridCols)
            return;

        if (mapGrid[row, col] != "       ")
        {
            return;
        }

        var roomName = room.Name.Length > RoomNameLength
            ? room.Name.Substring(0, RoomNameLength)
            : room.Name.PadRight(RoomNameLength);

        mapGrid[row, col] = $"[{roomName}]";
        roomGrid[row, col] = room;

        if (room.North != null && row > 1)
        {
            mapGrid[row - 1, col] = "   |   ";
            PlaceRoom(room.North, row - 2, col);
        }

        if (room.South != null && row < gridRows - 2)
        {
            mapGrid[row + 1, col] = "   |   ";
            PlaceRoom(room.South, row + 2, col);
        }

        if (room.East != null && col < gridCols - 2)
        {
            mapGrid[row, col + 1] = "  ---  ";
            PlaceRoom(room.East, row, col + 2);
        }

        if (room.West != null && col > 1)
        {
            mapGrid[row, col - 1] = "  ---  ";
            PlaceRoom(room.West, row, col - 2);
        }
    }
}
