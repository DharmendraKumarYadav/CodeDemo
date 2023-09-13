using BDB;
using IdentityServer4.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace UnitechMedical.Helpers
{
    public interface ISmsService
    {
        Task<(bool success, string errorMsg)> SendAsync(string to, string message);
    }

    public class SmsService : ISmsService
    {
        private SmsHubConfig _smsHubconfig;
        public SmsService(IOptions<AppSettings> config)
        {
            _smsHubconfig = config.Value.SmsHubConfig;
        }
        public async Task<(bool success, string errorMsg)> SendAsync(string to, string message)
        {
            var mobileNumber = VerifyNumber(to);
            string sURL = _smsHubconfig.SmsUrl+"apiKey=" + _smsHubconfig.ApiKey + "&sender=" + _smsHubconfig.SenderId+ "&numbers=" + mobileNumber + "&message=" + message+ "";
            await GetResponse(sURL);
            return (true, null);
        }
        public  async Task<string> GetResponse(string sURL)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sURL);
            request.MaximumAutomaticRedirections = 4;
            request.Credentials = CredentialCache.DefaultCredentials;

            try
            {
                var response = (HttpWebResponse)request.GetResponse();

                Stream receiveStream = response.GetResponseStream();

                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                string sResponse =await readStream.ReadToEndAsync();

                response.Close();

                readStream.Close();

                return sResponse;

            }
            catch(Exception ex)
            {
                return "";
            }
        }

        private string VerifyNumber(string mobileNumber) {
            if (mobileNumber.Length == 10)
            {
                return "91" + mobileNumber;
            }
            else {
                return mobileNumber;
            }
        }
    }
}
