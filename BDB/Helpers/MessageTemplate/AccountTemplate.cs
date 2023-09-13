using System;
using System.Collections.Generic;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace BDB.Helpers
{
    public static class AccountTemplate
    {
        static IWebHostEnvironment _hostingEnvironment;
        static string emailTemplate;

        public static void Initialize(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        #region Email 

        public static string SendConatctToCustomerEmailAsync(string userName)
        {

            return ReadFile("./wwwroot/MessageTemplates/Encodable/Email/ContactUsCustomer.mail.html")
                  .Replace("{user}", userName);

        }

















        public static string SendConatctToAdminEmailAsync(string adminName, string userName, string useruserMobile, string userEmail, string userMessage)
        {

            string emailMessage = ReadFile("./wwwroot/MessageTemplates/Encodable/Email/ContactUsAdmin.mail.html")
                .Replace("{admin}", adminName)
                .Replace("{name}", userName)
                  .Replace("{mobile}", useruserMobile)
                     .Replace("{message}", userMessage)
                .Replace("{email}", userEmail).ToString();

            return emailMessage;
        }
    
        public static string SendAvailbilityToAdminEmailAsync(string adminName, string userName, string useruserMobile, string userEmail, string userMessage,string query)
        {
  
            string emailMessage = ReadFile("./wwwroot/MessageTemplates/Encodable/Email/BikeAvailbilityAdmin.mail.html")
                .Replace("{admin}", adminName)
                .Replace("{name}", userName)
                .Replace("{mobile}", useruserMobile)
                .Replace("{message}", userMessage)
                .Replace("{query}", query)
                .Replace("{email}", userEmail).ToString();

            return emailMessage;
        }
        public static string SendBikeAvailbilityToCustomerEmailAsync(string userName)
        {
            string emailMessage = ReadFile("./wwwroot/MessageTemplates/Encodable/Email/BikeAvailbilityCustomer.mail.html")
                .Replace("{user}", userName).ToString();

            return emailMessage;
        }
        public static string SendSignupConfirmationEmailAsync(string recepientName, string email, string password)
        {
            string emailMessage = ReadFile("./wwwroot/MessageTemplates/Encodable/Email/SignupConfirmation.mail.html")
                .Replace("{email}", email)
                .Replace("{password}", password)
                .Replace("{user}", recepientName).ToString();

            return emailMessage;
        }

        public static string SendForgotPasswordEmailAsync(string recepientName, string expireTime, string code)
        {
            string emailMessage = ReadFile("./wwwroot/MessageTemplates/Encodable/Email/ForgotPassword.mail.html")
                .Replace("{expireTime}", expireTime)
                .Replace("{code}", code)
                .Replace("{user}", recepientName).ToString();

            return emailMessage;
        }

        public static string SendOtpEmailAsync(string code, string user)
        {
            var emailTemplate = ReadFile("./wwwroot/MessageTemplates/Encodable/Email/Otp.mail.html");
            string smsMessage = emailTemplate
                 .Replace("{user}", user)
            .Replace("{code}", code).ToString();
            return smsMessage;
        }
        public static string SendExceptionEmailAsync(string message, string stackTrace)
        {
            var emailTemplate = ReadFile("./wwwroot/MessageTemplates/Encodable/Email/Exception.mail.html");
            string smsMessage = emailTemplate
                 .Replace("{message}", message)
                 .Replace("{stacktrace}", stackTrace).ToString();
            return smsMessage;
        }

        #endregion

        #region SMS

        public static string SendOtpSmsAsync(string code, string username)
        {
            var smsTemplate = ReadFile("./wwwroot/MessageTemplates/Encodable/Sms/Otp.sms.txt");
            string smsMessage = smsTemplate
            .Replace("#Custom1#", username).ToString()
            .Replace("#Custom2#", code).ToString();
            return smsMessage;
        }
        #endregion

        #region Common

        private static string ReadPhysicalFile(string path)
        {
            if (_hostingEnvironment == null)
                throw new InvalidOperationException($"{nameof(AccountTemplate)} is not initialized");

            IFileInfo fileInfo = _hostingEnvironment.ContentRootFileProvider.GetFileInfo(path);

            if (!fileInfo.Exists)
                throw new FileNotFoundException($"Template file located at \"{path}\" was not found");

            using (var fs = fileInfo.CreateReadStream())
            {
                using (var sr = new StreamReader(fs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        public static string ReadFile(string FileName)
        {
            try
            {
                using (StreamReader reader = File.OpenText(FileName))
                {
                    string fileContent = reader.ReadToEnd();
                    if (fileContent != null && fileContent != "")
                    {
                        return fileContent;
                    }
                }
            }
            catch (Exception ex)
            {
                //Log
                throw ex;
            }
            return null;
        }
        #endregion


    }
}
