namespace E_CommerceWeb.ViewModels.Email
{
    public class UserEmailOptions
    {
        public List<string> RecieveEmails { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IFormFile Attachment { get; set; }
        public List<KeyValuePair<string, string>> PlaceHolders { get; set; }
    }
}
