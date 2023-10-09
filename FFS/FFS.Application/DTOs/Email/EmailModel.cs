namespace FFS.Application.DTOs.Email
{
    public class EmailModel
    {
        public List<string> To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
