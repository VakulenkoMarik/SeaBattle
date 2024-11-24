public class Player
{
    public bool isHuman = false;
    public int shotX = -1, shotY = -1;
    public int boatsCount = 5;

    public Field field = new();

    public void GenerateField(int mapSize)
    {
        field.GenerateMap(mapSize, boatsCount);
    }

    public bool IsDefeat()
    {
        return boatsCount <= 0;
    }

    public bool CanTakeShot(Field fieldOfAttack)
    {
        if (shotX < 0 || shotY < 0)
        {
            return false;
        }

        return !fieldOfAttack.GetCell(shotX, shotY).isShot;
    }

    public void GetShot(int x, int y)
    {
        if (field.GetCell(x, y).isShip)
        {
            boatsCount--;
        }
    }

    public void DrawMap()
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
                Cell cell = field.GetCell(j, i);
                char cellChar = cell.GetCellSymbol(isHuman);

                Console.Write(cellChar);
            }
        }

        Console.WriteLine($"\n Boats: {boatsCount}\n");
    }
}