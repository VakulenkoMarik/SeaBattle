using System.Text;

public class Game
{
    public int player1RoundsWin = 0;
    public int player2RoundsWin = 0;

    private Player player1;
    private Player player2;

    private int rounds = 3;

    public void Start(Gamemode gamemode)
    {
        StartBattle(gamemode);

        OutputBattleWiner();
    }

    private void StartBattle(Gamemode gamemode)
    {
        SetPlayers(gamemode);

        for (int i = 0; i < rounds; i++)
        {
            Round round = new();
            round.Start(gamemode, player1, player2);

            WinnerProcessing(round.Winer);
        }
    }

    private void WinnerProcessing(Player? winer)
    {
        if (winer != null)
        {
            if (winer == player1)
            {
                player1RoundsWin++;
            }
            else
            {
                player2RoundsWin++;
            }
        }

        OutputWiner(winer);
    }

    private void SetPlayers(Gamemode gamemode)
    {
        (player1, player2) = gamemode switch
        {
            Gamemode.PvP => (MakeHumanPlayer(), MakeHumanPlayer()),
            Gamemode.PvE => (MakeHumanPlayer(), MakeBotPlayer()),
            Gamemode.EvE => (MakeBotPlayer(), MakeBotPlayer()),
            _ => (player1, player2)
        };
    }

    private Player MakeHumanPlayer()
    {
        return new() { isHuman = true };
    }

    private Player MakeBotPlayer()
    {
        return new();
    }

    private void OutputWiner(Player? winer)
    {
        StringBuilder builder = new StringBuilder();

        if (winer == null)
        {
            builder.Append("Draw");
        }
        else
        {
            builder.Append($"Player {(winer == player1 ? 1 : 2)} is winer");
        }

        builder.Append($" ({player1RoundsWin}/{player2RoundsWin})");

        Drawer.DrawOnlyText(ConsoleColor.DarkRed, $"\n{builder}");

        Thread.Sleep(2000);
    }

    private void OutputBattleWiner()
    {
        string text;

        if (player1RoundsWin == player2RoundsWin)
        {
            text = "DRAW";
        }
        else
        {
            text = "WINER IS PLAYER " + $"{(player1RoundsWin > player2RoundsWin ? 1 : 2)}";
        }

        Drawer.DrawOnlyText(ConsoleColor.Yellow, text);
    }
}