public class Game
{
    int playerBoatsCount = 5;
    int enemyBoatsCount = 5;
    int mapsSize = 5;

    Field playerField = new() {isPlayer = true};
    Field enemyField = new();
    Drawer drawer = new();

    public void Start()
    {
        GameCycle();
    }

    void GameCycle()
    {
        GenerateMaps();

        Draw();
    }

    void GenerateMaps()
    {
        playerField.GenerateMap(mapsSize, playerBoatsCount);
        enemyField.GenerateMap(mapsSize, enemyBoatsCount);
    }

    void Draw()
    {
        Console.Clear();

        drawer.DrawMap(playerField);
        Console.WriteLine("\n ");
        drawer.DrawMap(enemyField);
    }
}

class Drawer
{
    public void DrawMap(Field field)
    {
        Console.Write("  ");

        for (char c = 'a'; c < 'a' + field.MapSize; c++)
        {
            Console.Write(c + "");
        }

        for (int i = 0; i < field.MapSize; i++)
        {
            Console.Write($"\n{i + 1} ");

            for (int j = 0; j < field.MapSize; j++)
            {
                Console.Write(GetCell(field, j, i));
            }
        }
    }

    char GetCell(Field field, int x, int y)
    {
        // Enemy
        if (field.cells[x,y].isBoat && field.cells[x,y].isShoted && !field.isPlayer)
        {
            return 'X';
        }
        else if (field.cells[x,y].isShoted && !field.isPlayer)
        {
            return 'O';
        }
        
        // Player
        if (field.cells[x,y].isBoat && field.isPlayer)
        {
            return '#';
        }

        return '.';
    }
}

class Field
{
    Random random = new Random();
    public Cell[,] cells = {};

    int boatsCount;
    int mapSize;
    public bool isPlayer;
    
    public int MapSize { get {return mapSize;} private set {} }

    public void GenerateMap(int mapSize, int boatsCount)
    {
        this.boatsCount = boatsCount;
        this.mapSize = mapSize;

        cells = new Cell[mapSize, mapSize];

        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                cells[i, j] = new Cell();
            }
        }

        PlaceBoats();
    }

    void PlaceBoats()
    {
        for (int i = 0; i < boatsCount; i++)
        {
            (int x, int y) = GenerateUniqueXY();

            cells[x, y].isBoat = true;
        }
    }

    (int, int) GenerateUniqueXY()
    {
        int x = random.Next(0, mapSize);
        int y = random.Next(0, mapSize);

        if (cells[x, y].isBoat)
        {
            return GenerateUniqueXY();
        }

        return (x, y);
    }
}

public class Cell
{
    public Cell()
    {
        isBoat = false;
        isShoted = false;
    }

    public bool isShoted;
    public bool isBoat;
}