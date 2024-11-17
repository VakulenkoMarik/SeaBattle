public class Game
{
    int playerBoatsCount = 5;
    int enemyBoatsCount = 5;
    int mapsSize = 5;

    Field playerField = new();
    Field enemyField = new();

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
        playerField.DrawMap();

        Console.WriteLine("");

        enemyField.DrawMap();
    }
}

class Field
{
    Random random = new Random();
    Cell[,] cells = {};

    int boatsCount;
    int mapSize;

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
        int x;
        int y;

        for (int i = 0; i < boatsCount; i++)
        {
            (x, y) = GenerateUniqueXY();

            cells[x, y].isBoat = true;
        }
    }

    (int, int) GenerateUniqueXY()
    {
        int x = random.Next(0, mapSize - 1);
        int y = random.Next(0, mapSize - 1);

        if (cells[x, y].isBoat)
        {
            return GenerateUniqueXY();
        }

        return (x, y);
    }

    public void DrawMap()
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                if (cells[j, i].isBoat)
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write(".");
                }
            } 

            Console.WriteLine();
        }
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