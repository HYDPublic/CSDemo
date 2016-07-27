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

    public NameValueCollection values;

    public StringLibrary()
    {
        values = new NameValueCollection();

        // Important string associations in Form.aspx.cs
        values["default"] = "Your score will appear here.";
        values["sub-key"] = "7fa5b75bedb54314b475ae11788d3756";
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
        return @"<h2 class='error'>" + result + @"</h2>";

    }
}