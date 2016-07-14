using System;
using System.Web;
using System.Text;
using static System.Net.WebRequest;
using System.IO;
using System.Net;

public partial class Default2 : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void InputSubmitButton_Click(object sender, EventArgs e)
    {
        // Make the API request call, then return the result.
        //InputLabel.Text = InputTextBox.Text;
        //InputTextBox.Text = string.Empty;

        //https://api.cognitive.microsoft.com/bing/v5.0/spellcheck/[?mode] 
        //HttpRequest request = new HttpRequest("filename", "https://api.cognitive.microsoft.com/bing/v5.0/spellcheck/", "Bill Gatas");


        // Create a new web request.
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://api.cognitive.microsoft.com/bing/v5.0/spellcheck/");



        // Add appropriate details
        byte[] payload = Encoding.UTF8.GetBytes(InputTextBox.Text);

        req.ContentLength = payload.Length;
        req.ContentType = "application/json";
        req.Method = "POST";
        req.Headers.Add("Ocp-Apim-Subscription-Key", "7fa5b75bedb54314b475ae11788d3756");

        Stream stream = req.GetRequestStream();
        stream.Write(payload, 0, payload.Length);
        stream.Close();
        try
        {
            if(req.HaveResponse)
            {
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                res.Close();

                stream = res.GetResponseStream();

                StreamReader reader = new StreamReader(stream);

                string responseFromServer = reader.ReadToEnd();

                OutputLabel.Text = responseFromServer;
                System.Diagnostics.Debug.WriteLine(responseFromServer);

                reader.Close();
                stream.Close();
                res.Close();
            } else
            {
                // Debugging.
                System.Diagnostics.Debug.WriteLine("No response.");
                System.Diagnostics.Debug.WriteLine(req.ToString());
            }
        }
        catch (System.Net.WebException exception)
        {
            System.Diagnostics.Debug.WriteLine(exception.Status);
        }

        // Manipulate to view response in browser.
        //Console.WriteLine(req.Headers);

        //var data = HttpServerUtility.UrlTokenEncode(Encoding.UTF8.GetBytes(InputTextBox.Text));
        //var data = Encoding.UTF8.GetBytes(InputTextBox.Text);
        //req.ContentLength = data.Length;

        //var reqStream = req.GetRequestStream();
        //reqStream.Write(data, 0, data.Length);
        //reqStream.Close();

        //var res = req.GetResponse();

        //OutputLabel.Text = "begin << " + req.GetResponse().ToString() + " >> end";

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
