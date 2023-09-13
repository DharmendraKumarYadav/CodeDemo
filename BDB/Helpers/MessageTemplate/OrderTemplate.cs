using DAL.Models.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;



namespace BDB.Helpers
{
    public class EmailProductList
    {
        public string Name { get; set; }
    }
    public static class OrderTemplate
    {
        static IWebHostEnvironment _hostingEnvironment;

        #region Email
        public static void Initialize(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public static string SendOrderCreateCustomerEmailAsync( string bookingId, string bookingDate, string bookingAmount,
            string name, string address, string city, string country, string pincode, string bikeName)
        {

            var emailTemplate = ReadFile("./wwwroot/MessageTemplates/Encodable/Email/NewBookingCustomer.mail.html");
            string emailMessage = emailTemplate
             .Replace("{bookingId}", bookingId)
             .Replace("{bookingDate}", bookingDate)
             .Replace("{bookingAmount}", bookingAmount)
                  .Replace("{bikeName}", bikeName)
              .Replace("{name}", name)
               .Replace("{address}", address)
               .Replace("{city}", city)
                .Replace("{country}", country)
              .Replace("{pincode}", pincode).ToString();
            return emailMessage;
        }

        public static string SendOrderCreateAdminEmailAsync(string adminName, string customer, string bookingId, string bookingDate, string bookingAmount,
            string name, string address, string city, string country, string pincode,string bikeName)
        {

            var emailTemplate = ReadFile("./wwwroot/MessageTemplates/Encodable/Email/NewBookingAdmin.mail.html");
            string emailMessage = emailTemplate
                .Replace("{admin}", adminName)
                  .Replace("{customer}", customer)
                .Replace("{bookingId}", bookingId)
                .Replace("{bookingDate}", bookingDate)
                .Replace("{bookingAmount}", bookingAmount)
                     .Replace("{bikeName}", bikeName)
                 .Replace("{name}", name)
                  .Replace("{address}", address)
                  .Replace("{city}", city)
                   .Replace("{country}", country)
                 .Replace("{pincode}", pincode).ToString();

            return emailMessage;
        }

        public static string SendOrderCancelCustomerEmailAsync(string recepientName, string orderId, string totalAmount, EmailProductList productList)
        {
            var item = productList;
            string emailMessage = ReadPhysicalFile("./wwwroot/MessageTemplates/Encodable/Email/BookingCancelCustomer.mail.html")
                .Replace("{user}", recepientName)
                .Replace("{order}", orderId)
                 .Replace("{ordertotal}", totalAmount)
                  .Replace("{productlist}", "").ToString();

            return emailMessage;
        }

        public static string SendOrderCancelAdminEmailAsync(string recepientName, string customer, string orderId, string totalAmount, EmailProductList productList)
        {
            // if (emailTemplate == null)
            var emailTemplate = ReadPhysicalFile("./wwwroot/MessageTemplates/Encodable/BookingCancelAdmin.mail.html");
            StringBuilder htmlStr = new StringBuilder("");
           
            string emailMessage = emailTemplate
                .Replace("{user}", recepientName)
                .Replace("{order}", orderId)
                .Replace("{customer}", customer)
                 .Replace("{ordertotal}", totalAmount)
                  .Replace("{productlist}", htmlStr.ToString()).ToString();
            return emailMessage;
        }

      

        #endregion

        #region Phone

        //public static string SendOrderFailedSmsAsync(string orderId,string name)
        //{
        //    var smsTemplate = ReadFile("./wwwroot/MessageTemplates/Encodable/Sms/OrderFailedCustomer.sms.txt");
        //    string smsMessage = smsTemplate
        //          .Replace("{name}", name)
        //    .Replace("{orderid}", orderId);
        //    return smsMessage;
        //}

        public static string SendOrderCreateCustomerSmsAsync(string orderId, string product, string amount)
        {
            var smsTemplate = ReadFile("./wwwroot/MessageTemplates/Encodable/Sms/NewBookingCustomer.sms.txt");
            string smsMessage = smsTemplate
            .Replace("{orderid}", orderId)
            .Replace("{product}", product)
            .Replace("{amount}", amount).ToString();
            return smsMessage;
        }

        public static string SendOrderCancelCustomerSmsAsync(string orderId, string product,string name)
        {
            var smsTemplate = ReadFile("./wwwroot/MessageTemplates/Encodable/BookingCancel.sms.txt");
            string smsMessage = smsTemplate
            .Replace("{name}", name)
            .Replace("{orderid}", orderId)
            .Replace("{product}", product).ToString();
            return smsMessage;
        }
        #endregion

        private static string ReadPhysicalFile(string path)
        {
            if (_hostingEnvironment == null)
                throw new InvalidOperationException($"{nameof(OrderTemplate)} is not initialized");

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

    }
}
