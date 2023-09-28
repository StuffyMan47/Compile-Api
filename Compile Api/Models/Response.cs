namespace Compile_Api.Models
{
    public class Response
    {
        public int userId { get; set; }
        public string compilationResult { get; set; }
        public string compilationErrors { get; set; }
        public string syntaxErrors { get; set; }
        public bool itDone { get; set; }

    }
}
