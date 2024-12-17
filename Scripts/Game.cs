using System.Text;

public class Game
{
    private GamePlayer gamePlayer1;
    private GamePlayer gamePlayer2;

    public User? BattleWiner { get; private set; }

    private Gamemode gamemode;

    private int rounds = 1;

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
            round.Start(gamemode, gamePlayer1, gamePlayer2);

            WinnerProcessing(round.Winer);
        }
    }

    private void GameInit(User user1, User user2)
    {
        user1.Games++;
        user2.Games++;

        DefinitionOfGamemode(user1, user2);

        gamePlayer1 = new(user1);
        gamePlayer2 = new(user2);
    }

    private void WinnerProcessing(Player? winer)
    {
        if (winer != null)
        {
            if (winer == gamePlayer1.player)
            {
                gamePlayer1.roundsWins++;
            }
            else
            {
                gamePlayer2.roundsWins++;
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
            builder.Append($"Player {(winer == gamePlayer1.player ? 1 : 2)} is winer");
        }

        builder.Append($" ({gamePlayer1.roundsWins}/{gamePlayer2.roundsWins})");

        Drawer.DrawOnlyText(ConsoleColor.DarkRed, $"\n{builder}");

        Thread.Sleep(2000);
    }

    private void BattleWinerProcessing()
    {
        int player1Wins = gamePlayer1.roundsWins;
        int player2Wins = gamePlayer2.roundsWins;

        if (player1Wins != player2Wins)
        {
            User winer = player1Wins > player2Wins ? gamePlayer1.user : gamePlayer2.user;

            BattleWiner = winer;
        }

        OutputBattleWiner();
    }

    private void OutputBattleWiner()
    {
        int player1RoundsWins = gamePlayer1.roundsWins;
        int player2RoundsWins = gamePlayer2.roundsWins;

        string text;

        if (player1RoundsWins == player2RoundsWins)
        {
            text = "DRAW";
        }
        else
        {
            text = "WINER IS PLAYER " + $"{(player1RoundsWins > player2RoundsWins ? 1 : 2)}";
        }

        Drawer.DrawOnlyText(ConsoleColor.Yellow, text);
    }
}