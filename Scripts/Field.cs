public class Field
{
    private Random random = new Random();
    private Cell[,] cells = {};

    private int mapSize;
    public int boatsCount;
    
    public int MapSize { get {return mapSize;} private set {} }

    public void GenerateMap(int mapSize, int boatsCount)
    {
        if (mapSize * mapSize < boatsCount)
        {
            boatsCount = mapSize * mapSize - 1;
        }

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

    private void PlaceBoats()
    {
        for (int i = 0; i < boatsCount; i++)
        {
            (int x, int y) = GenerateUniqueXY();

            cells[x, y].isBoat = true;
        }
    }

    private (int, int) GenerateUniqueXY()
    {
        int x = random.Next(0, mapSize);
        int y = random.Next(0, mapSize);

        if (cells[x, y].isBoat)
        {
            return GenerateUniqueXY();
        }

        return (x, y);
    }

    public void TakeShot(Field fieldOfAttack, int x, int y)
    {
        if (!fieldOfAttack.cells[x, y].isShoted)
        {
            if (fieldOfAttack.cells[x, y].isBoat)
            {
                fieldOfAttack.boatsCount--;
            }

            fieldOfAttack.cells[x, y].isShoted = true;
        }
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
        isBoat = false;
        isShoted = false;
    }

    public bool isShoted;
    public bool isBoat;

    public bool ShotAndFeedback()
    {
        if (!isShoted)
        {
            isShoted = true;

            if (isBoat)
            {
                return true;
            }
        }

        return false;
    }

    public char GetCellSymbol(bool isHuman)
    {
        if (isBoat && isShoted)
        {
            return 'X';
        }
        
        if (isShoted)
        {
            return 'O';
        }
        
        // Player
        if (isBoat && isHuman)
        {
            return '#';
        }

        return '.';
    }
}