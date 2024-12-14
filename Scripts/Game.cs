using System.Text;

public class Game
{
    public int player1RoundsWin = 0;
    public int player2RoundsWin = 0;

    private Player player1;
    private Player player2;

    public Player? BattleWiner { get; private set; }

    private Gamemode gamemode;

    private int rounds = 3;

    public void Start(Player player1, Player player2)
    {
        GameInit(player1, player2);

        StartBattle();

        BattleWinerProcessing();
    }

    private void StartBattle()
    {
        for (int i = 0; i < rounds; i++)
        {
            Round round = new();

            player1.ResetValues();
            player2.ResetValues();

            round.Start(gamemode, player1, player2);

            WinnerProcessing(round.Winer);
        }
    }

    private void GameInit(Player p1, Player p2)
    {
        DefinitionOfGamemode(p1, p2);

        player1 = p1;
        player2 = p2;
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

    private void DefinitionOfGamemode(Player p1, Player p2)
    {
        gamemode = (p1.isHuman, p2.isHuman) switch
        {
            (true, true) => Gamemode.PvP,
            (true, false) or (false, true) => Gamemode.PvE,
            (false, false) => Gamemode.EvE
        };
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

    private void BattleWinerProcessing()
    {
        if (player1RoundsWin != player2RoundsWin)
        {
            Player winer = player1RoundsWin > player2RoundsWin ? player1 : player2;

            BattleWiner = winer;
        }

        OutputBattleWiner();
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