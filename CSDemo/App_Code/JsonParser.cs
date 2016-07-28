using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class JsonParser
{

    JObject json;
    String input;

    StringLibrary lib;

    public JsonParser(JObject json, String input)
    {
        this.json = json;
        this.input = input;

        lib = new StringLibrary();
    }

    public String GetIndex(IJEnumerable<JToken> collection, int index)
    {
        var i = 0;
        String result = "";

        foreach(JToken item in collection)
        {
            if(i == index)
            {
                result = item.ToString();
                return result;
            }
            i++;
        }

        // Not found or bad index
        return null;
    }

    public IJEnumerable<JToken> GetIndexValues(IJEnumerable<JToken> collection, int index)
    {
        var i = 0;
        foreach (JToken item in collection)
        {
            if(i == index)
            {
                return item.Values().Values();
            }
            i++;
        }

        // Not found or bad index.
        return null;
    }

    public String GetResults()
    {
        String result = "";
        JToken flaggedTokens = GetToken("flaggedTokens");

        if (flaggedTokens == null)
        {
            // No spelling mistakes were made in the query so "flaggedTokens" is empty.
            return lib.Success();
        }
        else
        {
            // sometimes incorrect words aren't flagged and we get error responses for no reason.... need to test
            foreach (JToken word in flaggedTokens.Children<JToken>())
            {
                result += GetTokenAndSuggestion(word);
            }

            // Wrap HTML table tags to format results.
            return lib.WrapTable(result);
        }
    }

    public String GetTokenAndSuggestion(JToken token)
    {
        String result = "";

        result += lib.v["tr"] + lib.WrapTableTags(GetIndex(token.Values(), 1));
        var suggestions = GetIndexValues(token.Values(), 3);
        result += lib.WrapTableTags(GetIndex(suggestions, 0)) + lib.v["trc"];

        return result;
    }

    public bool BadResponse()
    {
        return json.Value<string>("_type").Equals("ErrorResponse");
    }

    public JToken GetToken(String id)
    {
        JToken token = json.SelectToken(id);

        if(token.First == null)
        {
            // Empty token
            return null;
        }
        else
        {
            return token;
        }
    }
}