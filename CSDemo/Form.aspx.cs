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

    // TODO: keep the same test string on "Submit" click.

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
                Debug.WriteLine(json.ToString());
            }
            else
            {
                placeholder = parser.GetResults();
            }
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
