using System;
using System.Web;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

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
        paramString["text"] = input; // replace all spaces with +?

        byte[] payload = Encoding.UTF8.GetBytes(paramString.ToString());
        System.Diagnostics.Debug.WriteLine(paramString.ToString());
        using (var content = new ByteArrayContent(payload))
        {
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            response = await client.PostAsync(uri, content);

            // Placeholder will store the value to update the OutputLabel.
            var responseBody = response.Content;
            var serializedJson = await responseBody.ReadAsStringAsync();

            JObject json = (JObject)JsonConvert.DeserializeObject(serializedJson);

            if (json.Value<string>("_type").Equals("ErrorResponse"))
            {
                // Error
                placeholder = input.Length == 0 ? "No words entered!" : "Error response.";
            }
            else
            {
                // Values passed.
                JToken flaggedTokens = json.SelectToken("flaggedTokens");
                var flaggedTokenChildren = flaggedTokens.Values<JToken>();

                foreach (JToken token in flaggedTokenChildren.Values<JToken>("token"))
                {
                    var word = flaggedTokenChildren.Values<JToken>("suggestions");
                    var wordChildren = word.Values<JToken>();

                    foreach (JToken suggestion in wordChildren.Values<JToken>("suggestion"))
                    {
                        System.Diagnostics.Debug.WriteLine(suggestion.Value<JToken>().ToString());
                    }

                    //placeholder = json.ToString();

                    //System.Diagnostics.Debug.WriteLine("---");
                    //System.Diagnostics.Debug.WriteLine(token.Value<JToken>("token").ToString());
                    //System.Diagnostics.Debug.WriteLine(token.Value<JToken>("suggestions").ToString());
                    // this works, just change "token" above to "suggestions" then do the same 
                    // and parse all "suggestion" tags.
                    //System.Diagnostics.Debug.WriteLine("---");
                }
            }
        }
    }
}
