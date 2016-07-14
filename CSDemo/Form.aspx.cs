using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default2 : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void InputSubmitButton_Click(object sender, EventArgs e)
    {
        // Make the API request call, then return the result.
        InputLabel.Text = "Submitted";

        //https://api.cognitive.microsoft.com/bing/v5.0/spellcheck/[?mode] 

        //HttpRequest request = new HttpRequest("filename", "https://api.cognitive.microsoft.com/bing/v5.0/spellcheck/", "Bill Gatas");
        
        
    }
}


/*
 * using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Web;

namespace CSHttpClientSample
{
    static class Program
    {
        static void Main()
        {
            MakeRequest();
            Console.WriteLine("Hit ENTER to exit...");
            Console.ReadLine();
        }
        
        static async void MakeRequest()
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "{subscription key}");

            // Request parameters
            queryString["mode"] = "{string}";
            var uri = "https://api.cognitive.microsoft.com/bing/v5.0/spellcheck/?" + queryString;

            HttpResponseMessage response;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes("{body}");

            using (var content = new ByteArrayContent(byteData))
            {
               content.Headers.ContentType = new MediaTypeHeaderValue("< your content type, i.e. application/json >");
               response = await client.PostAsync(uri, content);
            }

        }
    }
}
 */
