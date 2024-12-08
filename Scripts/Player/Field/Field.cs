public class Field
{
    private Random random = new Random();
    private Cell[,] cells = {};

    public int MapSize { get; private set; }

    public void GenerateMap(int mapSize, int boatsCount)
    {
        if (mapSize * mapSize < boatsCount)
        {
            boatsCount = mapSize * mapSize - 1;
        }

        MapSize = mapSize;

        cells = new Cell[mapSize, mapSize];

        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                cells[i, j] = new Cell();
            }
        }

        PlaceBoats(boatsCount);
    }

    private void PlaceBoats(int boatsCount)
    {
        for (int i = 0; i < boatsCount; i++)
        {
            (int x, int y) = GenerateUniqueXY();

            cells[x, y].isShip = true;
        }
    }

    private (int, int) GenerateUniqueXY()
    {
        int x = random.Next(0, MapSize);
        int y = random.Next(0, MapSize);

        if (cells[x, y].isShip)
        {
            return GenerateUniqueXY();
        }

        return (x, y);
    }

    public Cell GetCell(int x, int y)
    {
        return cells[x, y];
    }
}