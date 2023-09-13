using Microsoft.AspNetCore.Hosting;

namespace BDB.Helpers.Template
{
    public static class EmailTemplate
    {
        static IWebHostEnvironment _hostingEnvironment;
        public static void Initialize(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
    }
}
