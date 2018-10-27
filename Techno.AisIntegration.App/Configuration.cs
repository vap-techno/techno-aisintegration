namespace TechnoAisIntegration.App
{
    public class Configuration : IConfig
    {
        public string DbName { get; set; }
        public string Provider { get; set; }
        public string LogFile { get; set; }
    }
}