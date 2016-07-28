using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;

/// <summary>
/// Summary description for StringLibrary
/// </summary>
public class StringLibrary
{

    //TODO expand this further into an HTMLWrapper class?

    public NameValueCollection v;

    public StringLibrary()
    {
        v = new NameValueCollection();

        // Important string associations in Form
        v["default"] = "Your score will appear here.";
        v["sub-key"] = "7fa5b75bedb54314b475ae11788d3756";

        // General HTML
        v["h2e"] = @"<h2 class='error'>";
        v["h2s"] = @"<h2 class='success'>";
        v["h2c"] = @"</h2>";

        // Table tags
        v["tr"] = @"<tr>";
        v["trc"] = @"</tr>";

        v["td"] = @"<td>";
        v["tdc"] = @"</td>";

        // Results table formatting
        v["table"] = 
                @"<div>
                    <table class='table table-striped'>
                        <thead>
                            <tr>
                                <th>Token</th>
                                <th>Suggestion</th>
                            </tr>
                        </thead>
                        <tbody>";
        v["tablec"] = @"</tbody>
                    </table>
                </div>";
    }

    public string Error(int inputLength)
    {
        string result;
        if(inputLength == 0)
        {
            result = "No words entered!";
        }
        else
        {
            result = "Error response.";
        }

        // HTML Wrapping
        return v["h2e"] + result + v["h2-suf"];
    }

    public string Success() 
    {
        return v["h2s"] + "You spelled everything correctly!" + v["h2c"];
    }

    public string WrapTableTags(string item)
    {
        return v["td"] + item + v["tdc"];
    }

    public string WrapTable(string item)
    {
        return v["table"] + item + v["tablec"];
    }
}