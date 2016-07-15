using System;
using System.Web;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;

public partial class Default2 : System.Web.UI.Page
{

    static String placeholder;

    protected void Page_Load(object sender, EventArgs e)
    {
        //
    }

    protected void InputSubmitButton_Click(object sender, EventArgs e)
    {
        MakeRequest();
        OutputLabel.Text = placeholder;
    }

    static async void MakeRequest()
    {
        // Static sample text taken from API example, "Bill Gatas" should correct to "Bill Gates"
        //byte[] payload = Encoding.UTF8.GetBytes("Bill Gatas");

        // Create a new client
        HttpClient client = new HttpClient();

        // Request headers
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "7fa5b75bedb54314b475ae11788d3756");

        // Request parameters
        var queryString = HttpUtility.ParseQueryString(string.Empty);
        queryString["mode"] = "spell";
        var uri = "https://api.cognitive.microsoft.com/bing/v5.0/spellcheck/?" + queryString;

        // Prepare the response
        HttpResponseMessage response;

        // Request body
        var paramString = HttpUtility.ParseQueryString(string.Empty);

        //Static text example from API reference. Should normally be input text.
        paramString["text"] = "Bill+Gatas";

        byte[] payload = Encoding.UTF8.GetBytes(paramString.ToString());

        using (var content = new ByteArrayContent(payload))
        {
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            response = await client.PostAsync(uri, content);

            // Placeholder will store the value to update the OutputLabel.
            placeholder = response.ToString();
        }

        System.Diagnostics.Debug.WriteLine(response.ToString());
        System.Diagnostics.Debug.WriteLine(response.StatusCode);
    }

}
