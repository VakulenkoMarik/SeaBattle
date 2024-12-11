public class User(string name)
{
    public string Name { get; private set; } = name;
    public int Wins { get; set; } = 0;
    public bool IsHuman { get; set; } = false;
}