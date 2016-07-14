using System;
using System.Web;
using System.Text;
using static System.Net.WebRequest;

public partial class Default2 : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void InputSubmitButton_Click(object sender, EventArgs e)
    {
        // Make the API request call, then return the result.
        InputLabel.Text = InputTextBox.Text;
        InputTextBox.Text = string.Empty;

        //https://api.cognitive.microsoft.com/bing/v5.0/spellcheck/[?mode] 
        //HttpRequest request = new HttpRequest("filename", "https://api.cognitive.microsoft.com/bing/v5.0/spellcheck/", "Bill Gatas");

        var req = System.Net.WebRequest.Create("https://api.cognitive.microsoft.com/bing/v5.0/spellcheck/");
        req.ContentType = "application/json";
        req.Method = "POST";
        //var data = HttpServerUtility.UrlTokenEncode(Encoding.UTF8.GetBytes(InputTextBox.Text));
        var data = Encoding.UTF8.GetBytes(InputTextBox.Text);
        req.ContentLength = data.Length;

        var reqStream = req.GetRequestStream();
        reqStream.Write(data, 0, data.Length);
        reqStream.Close();

        var res = req.GetResponse();

        InputTextBox.Text = req.GetResponse().ToString() + ", end";

        //var req = new HttpRequest("some-name", "https://api.cognitive.microsoft.com/bing/v5.0/spellcheck/", "Bill Gatas");
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
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "7fa5b75bedb54314b475ae11788d3756");

            // Request parameters
            queryString["mode"] = "spell";
            var uri = "https://api.cognitive.microsoft.com/bing/v5.0/spellcheck/?" + queryString;

            HttpResponseMessage response;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes("Bill Gatas");

            using (var content = new ByteArrayContent(byteData))
            {
               content.Headers.ContentType = new MediaTypeHeaderValue("< your content type, i.e. application/json >");
               response = await client.PostAsync(uri, content);
            }

        }
    }
}
 */
