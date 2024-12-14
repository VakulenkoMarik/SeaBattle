public class GamePlayer(User user)
{
    public User user = user;
    public Player player = new(user.Name, user.IsHuman);

    public int roundsWins = 0;
}