public class Application
{
    private Player player1;
    private Player player2;

    private Drawer drawer;

    public void Start(Gamemode gamemode, int rounds)
    {
        ApplicationInit(gamemode);

        StartBattle(rounds);

        OutputBattleWiner();
    }

    private void ApplicationInit(Gamemode gamemode)
    {
        SetPlayers(gamemode);

        drawer = new Drawer(
            gamemode,
            player1,
            player2
        );
    }

    private void StartBattle(int rounds)
    {
        for (int i = 0; i < rounds; i++)
        {
            Game round = new();
            round.Start(drawer, player1, player2);
        }
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

    private void OutputBattleWiner()
    {
        string text;

        if (player1.roundWins == player2.roundWins)
        {
            text = "DRAW";
        }
        else
        {
            text = "WINER IS PLAYER " + $"{(player1.roundWins > player2.roundWins ? 1 : 2)}";
        }

        drawer.DrawOnlyText(ConsoleColor.Yellow, text);
    }
}