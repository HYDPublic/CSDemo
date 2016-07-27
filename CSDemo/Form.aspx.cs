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

    static StringLibrary lib;
    TextGenerator generator;

    protected void Page_Load(object sender, EventArgs e)
    {
        lib = new StringLibrary();
        generator = new TextGenerator();

        InputText.Text = generator.generateSentence();

        placeholder = "";
        OutputLabel.Text = lib.values["default"];
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
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", lib.values["sub-key"]);

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


            // no longer making a request after this point. Consider breaking this into multiple smaller functions.

            if (json.Value<string>("_type").Equals("ErrorResponse"))
            {
                // Error
                placeholder = lib.Error(input.Length);
            }
            else
            {
                // Values passed correctly
                JToken flaggedTokens = json.SelectToken("flaggedTokens");

                // Set the placeholder now. If flaggedTokens is empty, it won't be overwritten
                if(flaggedTokens.First == null)
                {
                    placeholder = "You spelled everything correctly!";
                    placeholder = String.Format("{0}{1}{2}", @"<h2 class='success'>", placeholder, @"</h2>");
                }
                else
                {
                    // If the above if statement executes, this foreach never iterates. 
                    // sometimes incorrect words aren't flagged and we get error responses for no reason.... need to test
                    foreach (JToken word in flaggedTokens.Children<JToken>())
                    {
                        Debug.WriteLine("Entered the foreach loop.");
                        var i = 0;
                        // Enumerate the IEnumerable object. Only print values at index 1 & 3.
                        foreach (JToken item in word.Values())
                        {
                            switch (i)
                            {
                                // Index 1 - raw token.
                                case 1:
                                    placeholder += @"<tr><td>";
                                    placeholder += (item.ToString() + "<br>");
                                    placeholder += @"</td>";
                                    Debug.WriteLine(item.ToString());
                                    break;

                                // Index 3 - suggested corrections to the token.
                                case 3:
                                    // iterate the array.
                                    var suggestions = item.Values().Values();

                                    bool flip = true;
                                    foreach (var val in suggestions)
                                    {
                                        if (flip)
                                        {
                                            placeholder += @"<td>";
                                            placeholder += val.ToString();
                                            placeholder += @"</td></tr>";
                                        }
                                        //placeholder += flip ? ("Suggestion: " + val.ToString() + "<br>") : ("Score: " + val.ToString() + "<br><br>");
                                        flip = !flip;
                                    }
                                    Debug.WriteLine(suggestions.ToString());
                                    break;
                            }
                            i++;
                        }
                    }
                    string prefix =
                        @"<div>
                              <table class='table table-striped'>
                                  <thead>
                                      <tr>
                                          <th>Token</th>
                                          <th>Suggestion</th>
                                      </tr>
                                  </thead>
                                  <tbody>
                        ";
                    string suffix = @"</tbody></table></div>";

                    placeholder = String.Format("{0}{1}{2}", prefix, placeholder, suffix);
                    Debug.WriteLine(placeholder);
                }
            }
        }
    }

    protected void ResetButton_Click(object sender, EventArgs e)
    {
        InputTextBox.Text = "";

        // Add Score
        // Maybe also add a confirmation for resetting? or just a label explaining
        // ScoreLabel.Text = "0";
    }

    protected void NewSentenceButton_Click(object sender, EventArgs e)
    {
        InputText.Text = generator.generateSentence();
        InputTextBox.Text = "";
    }
}
