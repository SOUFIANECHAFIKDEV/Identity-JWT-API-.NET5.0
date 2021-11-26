using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace IdentityAPI.Domain
{
    public class EmailBuilder
    {
        public string MailTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> Cc { get; set; }
        public List<string> Bcc { get; set; }
        public IList<IFormFile> Attachments { get; set; }
    }
}