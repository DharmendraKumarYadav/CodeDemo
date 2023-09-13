using System.Collections.Generic;
using WhatsappBusiness.CloudApi.Messages.Requests;

namespace BDB.ViewModels
{
    public class WhatsappRequestModel
    {
        public string RecipientPhoneNumber { get; set; }
        public string TemplateName { get; set; }
        public RequestParameter RequestParameter { get; set; }


    }
    public class RequestParameter {
        public List<string> Header { get; set; }
        public List<string> Body { get; set; }

    }
}
