public class Field
{
    private Random random = new Random();
    private Cell[,] cells = {};

    private int mapSize;
    public int MapSize { get {return mapSize;} private set {} }

    public void GenerateMap(int mapSize, int boatsCount)
    {
        if (mapSize * mapSize < boatsCount)
        {
            boatsCount = mapSize * mapSize - 1;
        }

        this.mapSize = mapSize;

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
        int x = random.Next(0, mapSize);
        int y = random.Next(0, mapSize);

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

public class Cell
{
    public Cell()
    {
        isShip = false;
        isShot = false;
    }

    public bool isShot;
    public bool isShip;

    public bool ShotHitShip()
    {
        if (!isShot)
        {
            isShot = true;

            if (isShip)
            {
                return true;
            }
        }

        return false;
    }

    public char GetCellSymbol(bool isHuman)
    {
        if (isShip && isShot)
        {
            return 'X';
        }
        
        if (isShot)
        {
            return 'O';
        }
        
        // Player
        if (isShip && isHuman)
        {
            return '#';
        }

        return '.';
    }
}