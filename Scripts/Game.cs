using System.Text;

public class Game
{
    public int player1RoundsWin = 0;
    public int player2RoundsWin = 0;

    private User user1;
    private User user2;

    private Gamemode gamemode;

    private int rounds = 3;

    public void Start(User user1, User user2)
    {
        GameInit(user1, user2);

        StartBattle();

        BattleWinerProcessing();
    }

    private void StartBattle()
    {
        for (int i = 0; i < rounds; i++)
        {
            Round round = new();

            Player player1 = user1.CreateNewPlayer();
            Player player2 = user2.CreateNewPlayer();

            round.Start(gamemode, player1, player2);

            WinnerProcessing(round.Winer);
        }
    }

    private void GameInit(User u1, User u2)
    {
        DefinitionOfGamemode(u1, u2);

        user1 = u1;
        user2 = u2;
    }

    private void WinnerProcessing(Player? winer)
    {
        if (winer != null)
        {
            if (winer == user1.player)
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

    private void DefinitionOfGamemode(User u1, User u2)
    {
        gamemode = (u1.IsHuman, u2.IsHuman) switch
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
            builder.Append($"Player {(winer == user1.player ? 1 : 2)} is winer");
        }

        builder.Append($" ({player1RoundsWin}/{player2RoundsWin})");

        Drawer.DrawOnlyText(ConsoleColor.DarkRed, $"\n{builder}");

        Thread.Sleep(2000);
    }

    private void BattleWinerProcessing()
    {
        if (player1RoundsWin != player2RoundsWin)
        {
            _ = player1RoundsWin > player2RoundsWin ? user1.Wins++ : user2.Wins++;
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