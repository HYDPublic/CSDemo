using System;
using System.Web;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;

public partial class Default2 : System.Web.UI.Page
{

    static String placeholder;
    static String input;

    string Title { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.Title = "Form";
        placeholder = "";
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
        Debug.WriteLine(paramString.ToString());
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
                // Values passed correctly
                JToken flaggedTokens = json.SelectToken("flaggedTokens");

                // Set the placeholder now. If flaggedTokens is empty, it won't be overwritten
                if(flaggedTokens.First == null)
                {
                    placeholder = "You spelled everything correctly!";
                }

                foreach (JToken word in flaggedTokens.Children<JToken>())
                {

                    var i = 0;
                    // Enumerate the IEnumerable object. Only print values at index 1 & 3.
                    foreach (JToken item in word.Values())
                    {
                        switch(i)
                        {
                            // Index 1 - raw token.
                            case 1:
                                placeholder += ("Token: " + item.ToString() + "<br>");
                                Debug.WriteLine(item.ToString());
                                break;

                            // Index 3 - suggested corrections to the token.
                            case 3:
                                // iterate the array.
                                var suggestions = item.Values().Values();

                                bool flip = true;
                                foreach (var val in suggestions)
                                {
                                    placeholder += flip ? ("Suggestion: " + val.ToString() + "<br>") : ("Score: " + val.ToString() + "<br><br>");
                                    flip = !flip;
                                }
                                Debug.WriteLine(suggestions.ToString());
                                break;
                        }
                        i++;
                    }
                }
            }
        }

        string prefix = @"<div class='test-class'>";
        string suffix = @"</div>";

        placeholder = prefix + placeholder + suffix;

    }
}
