namespace Digizuite.Models
{
    public class AuthenticateResponse
    {
        public string AccessKey { get; set; }
        public string CsrfToken { get; set; }
        public string MemberId { get; set; }
        public string Itemid { get; set; }
        public string LanguageId { get; set; }
    }
}