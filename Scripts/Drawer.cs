public static class Drawer
{
    public static void DrawMap(Player player, bool drawTools = true, int radarRadius = -1, int rX = -1, int rY = -1)
    {
        if (radarRadius > 0)
        {
            Console.Clear();

            Console.WriteLine(rX + " " + rY);
        }

        DrawUp(player.field);

        int size = player.field.MapSize;

        for (int i = 0; i < size; i++)
        {
            Console.Write($"\n{i + 1} ");

            for (int j = 0; j < size; j++)
            {
                Cell cell = player.field.GetCell(j, i);
                char cellChar;

                if (radarRadius > 0)
                {
                    cellChar = RadarCell(j, i, rX, rY, radarRadius, cell);
                }
                else
                {
                    cellChar = cell.GetCellSymbol(player.showShips);
                }

                Console.Write(cellChar);

                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        if (drawTools)
        {
            WriteTools(player);
        }
    }

    public static void WriteTools(Player player)
    {
        Console.ForegroundColor = ConsoleColor.Blue;

        Console.WriteLine($"\n Boats: {player.shipsCount}");

        if (player.isHuman)
        {
            Console.WriteLine($" Radars: {player.radarsCount}, is currently in use: {player.usesRadar}\n");
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