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
        OutputLabel.Text = lib.v["default"]; // center this text in a div
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
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", lib.v["sub-key"]);

        // Request query string.
        var queryString = HttpUtility.ParseQueryString(string.Empty);
        queryString["mode"] = "spell";
        var uri = lib.v["url"] + queryString;

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

            JsonParser parser = new JsonParser(json, input);

            if(parser.BadResponse())
            {
                placeholder = lib.Error(input.Length);
            }
            else
            {
                placeholder = parser.GetResults();
            }


            // no longer making a request after this point. Consider breaking this into multiple smaller functions.
            // nothing after this is async either, definitely break into a JSONHandler class.

            //if (json.Value<string>("_type").Equals("ErrorResponse"))
            //{
            //    // Error or no input
            //    placeholder = lib.Error(input.Length);
            //}
            //else
            //{
            //    placeholder = parser.GetResults();
            //    //if(parser.EmptyToken())
            //    //{
            //    //    placeholder = lib.Success();
            //    //}
            //    //else
            //    //{
            //    //    placeholder = parser.GetResults();
            //    //}


            //    //// Values passed correctly
            //    //JToken flaggedTokens = json.SelectToken("flaggedTokens");

            //    //if(flaggedTokens.First == null)
            //    //{
            //    //    // No spelling mistakes made.
            //    //    placeholder = lib.Success();
            //    //}
            //    //else
            //    //{
            //    //    // If the above if statement executes, this foreach never iterates. 
            //    //    // sometimes incorrect words aren't flagged and we get error responses for no reason.... need to test
            //    //    foreach (JToken word in flaggedTokens.Children<JToken>())
            //    //    {
            //    //        //Debug.WriteLine("Entered the foreach loop.");
            //    //        var i = 0;
            //    //        // Enumerate the IEnumerable object. Only print values at index 1 & 3.
            //    //        foreach (JToken item in word.Values())
            //    //        {
            //    //            switch (i)
            //    //            {
            //    //                // Index 1 - The raw token
            //    //                case 1:
            //    //                    placeholder += lib.v["tr"] + lib.WrapTableTags(item.ToString());
            //    //                    break;

            //    //                // Index 3 - Suggested corrections to the token.
            //    //                case 3:
            //    //                    // iterate the array.
            //    //                    var suggestions = item.Values().Values();

            //    //                    bool flip = true;
            //    //                    foreach (var val in suggestions)
            //    //                    {
            //    //                        if (flip)
            //    //                        {
            //    //                            placeholder += lib.WrapTableTags(val.ToString()) + lib.v["trc"];
            //    //                        }
            //    //                        flip = !flip;
            //    //                    }
            //    //                    break;
            //    //            }
            //    //            i++;
            //    //        }
            //    //    }
            //    //    // Wrap HTML table tags to format results.
            //    //    placeholder = lib.WrapTable(placeholder);
            //    //}
            //}
        }
    }

    protected void ResetButton_Click(object sender, EventArgs e)
    {
        InputTextBox.Text = "";
    }

    protected void NewSentenceButton_Click(object sender, EventArgs e)
    {
        InputText.Text = generator.generateSentence();
        InputTextBox.Text = "";
    }
}
