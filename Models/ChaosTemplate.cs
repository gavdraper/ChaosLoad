namespace ChaosLoad.Models
{
    public class ChaosTemplate
    {
        public string ScriptPath { get; set; }
        public int RunCount { get; set; }
        public int Sleep { get; set; }
        public int Threads { get; set; }
        public string MongoDatabase { get; set; }
    }

}
