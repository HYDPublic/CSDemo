using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


/// <summary>
/// Summary description for TextGenerator
/// </summary>
public class TextGenerator
{

    //public string Text { get; set; }
    private string[] sentences;

    public TextGenerator()
    {
        sentences = new string[] 
        {
            "The quick brown fox jumps over the lazy dog",
            "Pack my box with five dozen liquor jugs.",
            "Jackdaws love my big sphinx of quartz.",
            "The five boxing wizards jump quickly.",
            "How vexingly quick daft zebras jump!",
            "Bright vixens jump; dozy fowl quack."
        };
    }

    public string generateSentence()
    {
        Random r = new Random();

        int index = (int)Math.Floor((r.NextDouble() * sentences.Length));

        return sentences[index];
    }

}