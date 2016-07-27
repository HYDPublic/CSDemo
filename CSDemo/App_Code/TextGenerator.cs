using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


/// <summary>
/// Summary description for TextGenerator
/// </summary>
public class TextGenerator
{

    public string Text { get; set; }
    private string[] easy, medium, hard;

    public TextGenerator()
    {
        easy = new string[]
        {
            "Hello world!",
            "My name is Freddy",
            "I like to walk in the park",
            "I have a pet dog"
        };

        medium = new string[] 
        {
            "The quick brown fox jumps over the lazy dog",
            "Pack my box with five dozen liquor jugs.",
            "Jackdaws love my big sphinx of quartz.",
            "The five boxing wizards jump quickly.",
            "How vexingly quick daft zebras jump!",
            "Bright vixens jump; dozy fowl quack."
        };

        // TODO fillout this array with long sentences.
        hard = new string[] { };

    }

}