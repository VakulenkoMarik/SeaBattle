public class User(string name, bool isHuman)
{
    public string Name { get; private set; } = name;
    public int Wins { get; set; } = 0;
    public bool IsHuman { get; private set; } = isHuman;

    public Player player { get; private set; } = new(name, isHuman);
}