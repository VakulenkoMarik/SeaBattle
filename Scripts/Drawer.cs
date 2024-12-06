public class Drawer
{
    private Gamemode gamemode;

    private Player player1;
    private Player player2;

    public Drawer (Gamemode gm, Player p1, Player p2)
    {
        gamemode = gm;
        player1 = p1;
        player2 = p2;
    }

    public void DrawFields()
    {
        DrawPlayerField(player1);
        
        Console.WriteLine(" ");

        DrawPlayerField(player2);
    }

    private void DrawPlayerField(Player player)
    {
        DrawMap(player);
        WriteResources(player);
    }

    private void DrawMap(Player player)
    {
        DrawUp(player.field);

        int size = player.field.MapSize;

        for (int i = 0; i < size; i++)
        {
            Console.Write($"\n{i + 1} ");

            for (int j = 0; j < size; j++)
            {
                bool isHumanForModes = player.isHuman;

                if (gamemode != Gamemode.PvE)
                {
                    isHumanForModes = !player.isHuman;
                }

                Cell cell = player.field.GetCell(j, i);
                char cellChar = cell.GetCellSymbol(isHumanForModes);
                
                Console.Write(cellChar);

                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }

    private void WriteResources(Player player)
    {
        Console.ForegroundColor = ConsoleColor.Blue;

        Console.WriteLine($"\n Boats: {player.shipsCount}");
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
}