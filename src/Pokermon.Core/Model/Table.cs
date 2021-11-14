namespace Pokermon.Core.Model
{
    public class Table
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Player[] Players { get; set; } = new Player[8];

        public Table(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
