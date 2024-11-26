public static class Drawer
{
    public static void DrawMap(Field field, bool isHuman, int x = -1, int y = -1, int radius = -1, bool highlightArea = false)
    {
        DrawUp(field);

        for (int i = 0; i < field.MapSize; i++)
        {
            Console.Write($"\n{i + 1} ");

            for (int j = 0; j < field.MapSize; j++)
            {
                Cell cell = field.GetCell(j, i);
                char cellChar;

                if (highlightArea)
                {
                    cellChar = RadarCell(j, i, x, y, radius, cell);
                }
                else
                {
                    cellChar = cell.GetCellSymbol(isHuman);
                }

                Console.Write(cellChar);

                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }

    public static void WriteTools(int boatsCount, int radarsCount, bool usesRadar, bool isHuman)
    {
        Console.ForegroundColor = ConsoleColor.Blue;

        Console.WriteLine($"\n Boats: {boatsCount}");

        if (isHuman)
        {
            Console.WriteLine($" Radars: {radarsCount}, is currently in use: {usesRadar}\n");
        }
    }

    private static void DrawUp(Field field)
    {
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write("  ");

        for (char c = 'a'; c < 'a' + field.MapSize; c++)
        {
            Console.Write(c + "");
        }
    }

    private static char RadarCell(int currentX, int currentY, int radarX, int radarY, int radius, Cell cell)
    {
        if (Math.Abs(currentX - radarX) <= radius && Math.Abs(currentY - radarY) <= radius)
        {
            Thread.Sleep(200);
        
            Console.ForegroundColor = ConsoleColor.Green;

            return cell.GetCellSymbol(true);
        }
        else
        {
            Thread.Sleep(50);

            return '~';
        }
    }
}