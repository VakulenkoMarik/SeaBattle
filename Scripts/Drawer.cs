public static class Drawer
{
    private static Gamemode gamemode;

    public static void SetDrawerMode(Gamemode gm)
    {
        gamemode = gm;
    }

    public static void DrawFullMap(Player player)
    {
        DrawMap(player);
        WriteResources(player);
    }

    public static void DrawMap(Player player, int radarRadius = -1, int radiusX = -1, int radiusY = -1)
    {
        if (radarRadius > 0)
        {
            Console.Clear();
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
                    cellChar = RadarCell(j, i, radiusX, radiusY, radarRadius, cell);
                }
                else
                {
                    cellChar = DetermineShowingCell(player, cell);
                }

                Console.Write(cellChar);

                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }

    public static void WriteResources(Player player)
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
    
    private static char DetermineShowingCell(Player player, Cell cell)
    {
        bool isHumanForModes = player.isHuman;

        if (gamemode != Gamemode.PvE)
        {
            isHumanForModes = !player.isHuman;
        }
            
        return cell.GetCellSymbol(isHumanForModes);
    }
    
    public static void DrawOnlyText(string text, int time = -1)
    {
        Console.Clear();

        Console.WriteLine($"\n{text}");

        if (time > 0)
        {
            Thread.Sleep(time);
        }
    }
}