using System;
using System.Web;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public partial class Default2 : System.Web.UI.Page
{

    static String placeholder;
    static String input;

    protected void Page_Load(object sender, EventArgs e)
    {
        //
    }

    protected async void InputSubmitButton_Click(object sender, EventArgs e)
    {
        input = InputTextBox.Text;
        await MakeRequest();
        OutputLabel.Text = placeholder;
    }

    static async Task MakeRequest()
    {
        // Create a new client
        HttpClient client = new HttpClient();

        // Add appropriate request headers (API subscription key)
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "7fa5b75bedb54314b475ae11788d3756");

        // Request query string.
        var queryString = HttpUtility.ParseQueryString(string.Empty);
        queryString["mode"] = "spell";
        var uri = "https://api.cognitive.microsoft.com/bing/v5.0/spellcheck/?" + queryString;

        // Prepare the response
        HttpResponseMessage response;

        // Request body
        var paramString = HttpUtility.ParseQueryString(string.Empty);

        //Static text example from API reference. Should normally be input text.
        paramString["text"] = input; // replace all spaces with + ?

        byte[] payload = Encoding.UTF8.GetBytes(paramString.ToString());

        using (var content = new ByteArrayContent(payload))
        {
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            response = await client.PostAsync(uri, content);

            // Placeholder will store the value to update the OutputLabel.
            var responseBody = response.Content;
            var serializedJson = await responseBody.ReadAsStringAsync();
            JObject json = (JObject)JsonConvert.DeserializeObject(serializedJson);
            JToken flaggedTokens = json.SelectToken("flaggedTokens");

            System.Diagnostics.Debug.WriteLine(flaggedTokens.ToString());
            //placeholder = json.GetType().ToString();
            placeholder = json.ToString();
        }

    }

    //void UpdateLabel(string val)
    //{
    //    OutputLabel.Text = val;
    //}

}
