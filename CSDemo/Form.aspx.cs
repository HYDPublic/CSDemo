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

    static String placeholder = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        //sample change
    }

    static async void MakeRequest()
    {
        byte[] payload = Encoding.UTF8.GetBytes("Bill Gatas");

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
        var paramString = HttpUtility.ParseQueryString(string.Empty);
        paramString["text"] = "Bill+Gatas";
        // Do URL Encoding on the input text, not static like above
        byte[] byteData = Encoding.UTF8.GetBytes(paramString.ToString());

        using (var content = new ByteArrayContent(byteData))
        {
            // JSON gets 400 error.
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            System.Diagnostics.Debug.WriteLine("Test");
            response = await client.PostAsync(uri, content);
            placeholder = response.ToString();
            Console.WriteLine("wrote to the placeholder");

            // Double check the reference on the below method
            //response.EnsureSuccessStatusCode();
        }

        System.Diagnostics.Debug.WriteLine(response.ToString());
        System.Diagnostics.Debug.WriteLine(response.StatusCode);
    }

    protected void InputSubmitButton_Click(object sender, EventArgs e)
    {
        MakeRequest();
        OutputLabel.Text = placeholder;
        Console.WriteLine("wrote to the output label");
    }
}
