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

        // No spelling mistakes were made in the query so "flaggedTokens" is empty.
        if (flaggedTokens == null)
        {
            return lib.Success();
        }
        else
        {
            // sometimes incorrect words aren't flagged and we get error responses for no reason.... need to test
            foreach (JToken word in flaggedTokens.Children<JToken>())
            {
                result += lib.v["tr"] + GetIndex(word.Values(), 1);
                var suggestions = GetIndexValues(word.Values(), 3);
                result += lib.WrapTableTags(GetIndex(suggestions, 0)) + lib.v["trc"];
            }

            // Wrap HTML table tags to format results.
            return lib.WrapTable(result);
        }
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

        //JToken flaggedTokens = json.SelectToken("flaggedTokens");

        //if (flaggedTokens.First == null)
        //{
        //    // No spelling mistakes made.
        //    placeholder = lib.Success();
        //}
        //else
        //{
        //    // If the above if statement executes, this foreach never iterates. 
        //    // sometimes incorrect words aren't flagged and we get error responses for no reason.... need to test
        //    foreach (JToken word in flaggedTokens.Children<JToken>())
        //    {
        //        //Debug.WriteLine("Entered the foreach loop.");
        //        var i = 0;
        //        // Enumerate the IEnumerable object. Only print values at index 1 & 3.
        //        foreach (JToken item in word.Values())
        //        {
        //            switch (i)
        //            {
        //                // Index 1 - The raw token
        //                case 1:
        //                    placeholder += lib.v["tr"] + lib.WrapTableTags(item.ToString());
        //                    break;

        //                // Index 3 - Suggested corrections to the token.
        //                case 3:
        //                    // iterate the array.
        //                    var suggestions = item.Values().Values();

        //                    bool flip = true;
        //                    foreach (var val in suggestions)
        //                    {
        //                        if (flip)
        //                        {
        //                            placeholder += lib.WrapTableTags(val.ToString()) + lib.v["trc"];
        //                        }
        //                        flip = !flip;
        //                    }
        //                    break;
        //            }
        //            i++;
        //        }
        //    }
        //    // Wrap HTML table tags to format results.
        //    placeholder = lib.WrapTable(placeholder);
        //}

        //return json.SelectToken("flaggedTokens").First == null;
    }

}