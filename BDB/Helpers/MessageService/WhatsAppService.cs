using System.Threading.Tasks;
using WhatsappBusiness.CloudApi.Interfaces;
using WhatsappBusiness.CloudApi.Messages.Requests;
using WhatsappBusiness.CloudApi.Response;
using WhatsappBusiness.CloudApi.Exceptions;
using BDB.ViewModels;
using System.Collections.Generic;
using WhatsappBusiness.CloudApi.Messages.ReplyRequests;

namespace BDB.Helpers.MessageService
{
    public interface IWhatsAppService
    {
        Task<MarkMessageResponse> MarkMessageAsReadAsync(MarkMessageRequest markMessage);

        Task<WhatsAppResponse> SendTextMessageAsync(TextMessageReplyRequest textMessage);

        Task<WhatsAppResponse> SendWhatsAppTemplateMessage(WhatsappRequestModel textMessage);

    }

    public class WhatsAppService : IWhatsAppService
    {
        private readonly IWhatsAppBusinessClient _whatsAppBusinessClient;
        public WhatsAppService(IWhatsAppBusinessClient whatsAppBusinessClient)
        {
            _whatsAppBusinessClient = whatsAppBusinessClient;
        }
      
        public async Task<MarkMessageResponse> MarkMessageAsReadAsync(MarkMessageRequest markMessage)
        {
            return await _whatsAppBusinessClient.MarkMessageAsReadAsync(markMessage);
        }

        public async Task<WhatsAppResponse> SendTextMessageAsync(TextMessageReplyRequest textMessage)
        {
            return await _whatsAppBusinessClient.SendTextMessageAsync(textMessage);
        }

        public async Task<WhatsAppResponse> SendWhatsAppTemplateMessage(WhatsappRequestModel textMessage)
        {

            try
            {
                TextTemplateMessageRequest textTemplateMessage = new TextTemplateMessageRequest();
                textTemplateMessage.To = textMessage.RecipientPhoneNumber;
                textTemplateMessage.Template = new TextMessageTemplate();
                textTemplateMessage.Template.Name = textMessage.TemplateName;
                textTemplateMessage.Template.Language = new TextMessageLanguage();
                textTemplateMessage.Template.Language.Code = "en_US";

                if (textMessage.RequestParameter != null)
                {
                    textTemplateMessage.Template.Components = new List<TextMessageComponent>();
                    if (textMessage.RequestParameter.Header != null)
                        textTemplateMessage.Template.Components.Add(GetParameterComponent(textMessage.RequestParameter.Header, "header"));

                    if (textMessage.RequestParameter.Body != null)
                        textTemplateMessage.Template.Components.Add(GetParameterComponent(textMessage.RequestParameter.Header, "body"));
                }
                var results = await _whatsAppBusinessClient.SendTextMessageTemplateAsync(textTemplateMessage);
                return results;
            }
            catch (WhatsappBusinessCloudAPIException ex)
            {
                return null;

            }
        }
        private TextMessageComponent GetParameterComponent(List<string> parameter, string name)
        {
            var parameterList = new List<TextMessageParameter>();
            foreach (var item in parameter)
            {
                parameterList.Add(new TextMessageParameter
                {
                    Type = "text",
                    Text = item
                });
            }
            return new TextMessageComponent()
            {
                Type = name,
                Parameters = parameterList
            };
        }
    }


}
