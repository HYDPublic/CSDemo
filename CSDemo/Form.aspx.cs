using System;
using System.Web;
using System.Text;
using static System.Net.WebRequest;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public partial class Default2 : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    static async void MakeRequest()
    {
        // Create a new web request.
        //HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://api.cognitive.microsoft.com/bing/v5.0/spellcheck/?mode=spell");

        //// Add appropriate details
        ////byte[] payload = Encoding.UTF8.GetBytes(InputTextBox.Text);
        byte[] payload = Encoding.UTF8.GetBytes("Bill Gatas");

        //req.ContentLength = payload.Length;
        //req.ContentType = "application/x-www-form-urlencoded";
        //req.Method = "POST";
        //req.Headers.Add("Ocp-Apim-Subscription-Key", "7fa5b75bedb54314b475ae11788d3756");

        // their stuff
        HttpClient client = new HttpClient();
        var queryString = HttpUtility.ParseQueryString(string.Empty);

        // Request headers
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "7fa5b75bedb54314b475ae11788d3756");

        // Request parameters
        queryString["mode"] = "spell";
        var uri = "https://api.cognitive.microsoft.com/bing/v5.0/spellcheck/?" + queryString;
        System.Diagnostics.Debug.WriteLine(uri);

        HttpResponseMessage response;

        // Request body
        byte[] byteData = Encoding.UTF8.GetBytes("Bill Gatas");

        using (var content = new ByteArrayContent(byteData))
        {
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            //content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            System.Diagnostics.Debug.WriteLine("Test");
            response = await client.PostAsync(uri, content);
            response.EnsureSuccessStatusCode();
        }
    }

    static void UpdateOutputLabel(string s)
    {

    }

    protected void InputSubmitButton_Click(object sender, EventArgs e)
    {
        MakeRequest();
    }
}
