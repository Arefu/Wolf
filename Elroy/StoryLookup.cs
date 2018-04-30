namespace Elroy
{
    internal class StoryLookup
    {
        public StoryLookup(long Start, long End, int Missions)
        {
            this.Start = Start;
            this.End = End;
            this.Missions = Missions;
        }

        public long Start { get; set; }
        public long End { get; set; }
        public int Missions { get; set; }
    }
}