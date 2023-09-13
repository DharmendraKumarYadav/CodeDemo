using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitechMedical.Helpers;

namespace BDB
{
    public class AppSettings
    {
        public SmtpConfig SmtpConfig { get; set; }
        public SmsHubConfig SmsHubConfig { get; set; }

    }
   

    public class SmtpConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }

        public string Name { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string AdminEmail { get; set; }
        public string AdminName { get; set; }
    }
    public sealed class SmsHubConfig
    {
        private string _apiKey;
        /// <summary>
        /// Account sid
        /// </summary>
        public string ApiKey
        {
            get => _apiKey;
            set => _apiKey = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }


        private string _senderId;
        /// <summary>
        /// From phone
        /// </summary>
        public string SenderId
        {
            get => _senderId;
            set => _senderId = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private string _smsUrl;
        public string SmsUrl
        {
            get => _smsUrl;
            set => _smsUrl = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}
