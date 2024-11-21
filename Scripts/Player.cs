public class Player
{
    public bool isHuman = false;
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

    public bool CanTakeShot(Field fieldOfAttack, int x, int y)
    {
        if (fieldOfAttack.GetCell(x, y).isShoted)
        {
            return false;
        }

        return true;
    }

    public void GetShot(int x, int y)
    {
        if (field.GetCell(x, y).ShotAndFeedback())
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