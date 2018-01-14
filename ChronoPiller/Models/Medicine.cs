namespace ChronoPiller.Models
{
    public class Medicine
    {
        public string Name { get; }
        public int Interval { get; }

        public Medicine(string name, int interval)
        {
            Name = name;
            Interval = interval;
        }
    }
}