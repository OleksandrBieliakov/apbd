using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace tuto1
{
    public class Program
    {

        public int MyProperty { get; set; }

        public static async Task Main(string[] args)
        {
            var websiteUrl = args[0];
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(websiteUrl);

            if (response.IsSuccessStatusCode)
            {
                var htmlContent = await response.Content.ReadAsStringAsync();

                var regex = new Regex("[a-z]+[0-9]*@[a-z]+\\.[a-z]*", RegexOptions.IgnoreCase);

                var emailAdresses = regex.Matches(htmlContent);
                
                foreach (var item in emailAdresses)
                {
                    Console.WriteLine(item);
                }


            }

          
        }

    }
}
