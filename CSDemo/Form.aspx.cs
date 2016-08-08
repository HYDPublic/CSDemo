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
    static String prompt;

    static StringLibrary lib;
    TextGenerator generator;

    protected void Page_Load(object sender, EventArgs e)
    {
        lib = new StringLibrary();
        generator = new TextGenerator();
        prompt = PromptText.Text;
        PromptText.Text = generator.generateSentence();

        placeholder = "";
        OutputLabel.Text = lib.v["default"]; // center this text in a div
    }

    protected async void InputSubmitButton_Click(object sender, EventArgs e)
    {
        // Cannot access Length property in async method?
        input = InputTextBox.Text;
        await MakeRequest();

        // For Edit Distance, lower is better. Subtract from length of string for easier interpretation
        var score = Math.Max(input.Length, prompt.Length) - CalculateEditDistance(prompt, input);

        // Format output string
        var output = $"Phrase: {prompt}<br> Input: {input}<br> Your Score: {score}!<br>";

        OutputLabel.Text = output + placeholder;
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
        PromptText.Text = generator.generateSentence();
        InputTextBox.Text = "";
    }

    protected int min(int a, int b, int c)
    {
        if (a < b && a < c) return a;
        if (b < a && b < c) return b;
        return c;
    }

    public int CalculateEditDistance(string a, string b)
    {
        if (String.IsNullOrEmpty(a) || String.IsNullOrEmpty(b)) return 0;

        int lengthA = a.Length;
        int lengthB = b.Length;
        var distances = new int[lengthA + 1, lengthB + 1];
        for (int i = 0; i <= lengthA; distances[i, 0] = i++) ;
        for (int j = 0; j <= lengthB; distances[0, j] = j++) ;

        for (int i = 1; i <= lengthA; i++)
            for (int j = 1; j <= lengthB; j++)
            {
                int cost = b[j - 1] == a[i - 1] ? 0 : 1;
                distances[i, j] = Math.Min
                    (
                    Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                    distances[i - 1, j - 1] + cost
                    );
            }
        return distances[lengthA, lengthB];
    }

    // Score is calculated based on String Length - Edit Distance
    // (Larger edit distance is bad)
    protected int CalculateScore(string PromptText, string InputText, int p_length, int i_length)
    {
        // Code modified from below source.
        // http://www.geeksforgeeks.org/dynamic-programming-set-5-edit-distance/

        // We will need to index each string
        char[] p_text = PromptText.ToCharArray();
        char[] i_text = PromptText.ToCharArray();

        // Table to store scores in for Dynamic Programming
        int[,] scores = new int[p_length + 1, i_length + 1];

        for(int i=0;i<p_length;i++)
        {
            for(int j=0;j<i_length;j++)
            {
                // When first string is empty, all ADD of string 2
                if(i == 0)
                {
                    scores[i, j] = j;
                }
                // When second string is empty, all ADD of string 1
                else if(j == 0)
                {
                    scores[i, j] = i;
                }
                // If characters are the same, ignore
                else if(p_text[i - 1] == i_text[j - 1])
                {
                    scores[i, j] = scores[i - 1, j - 1];
                }
                // If different, consider insert, delete, or replace operations
                else
                {
                    scores[i, j] = 1 + min
                        (
                            scores[i, j - 1],    // Insert
                            scores[i - 1, j],    // Delete
                            scores[i - 1, j - 1] // Replace
                        );
                }
            }
        }

        return scores[p_length, i_length];
    }
}
