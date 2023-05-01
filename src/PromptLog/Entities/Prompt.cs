namespace PromptLog.Entities;

public class Prompt
{
    public Guid Id { get; set; }
    public string Url { get; set; }
    public string Experiment { get; set; }
    public DateTimeOffset Created { get; set; }
    public string Request { get; set; }
    public string Response { get; set; }
    public int Raiting { get; set; }
}