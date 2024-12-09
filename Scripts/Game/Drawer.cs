using System.Drawing;

public class Drawer(Gamemode gm, Player p1, Player p2)
{
    private readonly Gamemode gamemode = gm;

    private readonly Player player1 = p1;
    private readonly Player player2 = p2;

    public void DrawFields()
    {
        DrawPlayerField(player1);
        
        Console.WriteLine(" ");

        DrawPlayerField(player2);
    }

    private void DrawPlayerField(Player player)
    {
        bool showShips = player.isHuman;

        if (gamemode != Gamemode.PvE)
        {
            showShips = !player.isHuman;
        }

        DrawMap(player, showShips);
        WriteResources(player);
    }

    private void DrawMap(Player player, bool showShips)
    {
        DrawUp(player.field);

        int size = player.field.MapSize;

        for (int i = 0; i < size; i++)
        {
            Console.Write($"\n{i + 1} ");

            for (int j = 0; j < size; j++)
            {
                Cell cell = player.field.GetCell(j, i);

                bool isbackendOfCell = showShips;

                if (player.Threat.isRadarAttack)
                {
                    (int radarX, int radarY, int radarRadius) = player.Threat.GetRadarData();

                    if (IsCellInRadarField(j, i, radarX, radarY, radarRadius))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        isbackendOfCell = true;
                    }
                }

                char cellChar = cell.GetCellSymbol(isbackendOfCell);
                
                Console.Write(cellChar);

                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }

    private void WriteResources(Player player)
    {
        Console.ForegroundColor = ConsoleColor.Blue;

        Console.WriteLine($"\n Boats: {player.shipsCount}");

        if (player.isHuman)
        {
            Console.WriteLine($" Radars: {player.radarsCount}, is currently in use: {player.usesRadar}\n");
        }
    }

    private bool IsCellInRadarField(int currentX, int currentY, int radarX, int radarY, int radius)
    {
        return Math.Abs(currentX - radarX) <= radius && Math.Abs(currentY - radarY) <= radius;
    }

    private void DrawUp(Field field)
    {
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write("  ");

        for (char c = 'a'; c < 'a' + field.MapSize; c++)
        {
            Console.Write(c + "");
        }
    }

    public void DrawOnlyText(ConsoleColor color, string text)
    {
        Console.Clear();
        
        Console.ForegroundColor = color;

        Console.WriteLine(text);
    }
}