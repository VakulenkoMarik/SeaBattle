public class User(string name, bool isHuman)
{
    public string Name { get; private set; } = name;
    public bool IsHuman { get; private set; } = isHuman;
    
    public int Wins { get; set; } = 0;
}