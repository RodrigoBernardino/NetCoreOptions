namespace NetCoreOptions.TestWorkerApplication
{
    public class TestWorkerApplicationOptions : IAppSettingsOptions
    {
        public string ApplicationName { get; set; }
        public TextConfiguration TextConfiguration { get; set; }
    }

    public class TextConfiguration
    {
        public int PeriodValue { get; set; }
        public string PeriodUnit { get; set; }
    }
}
