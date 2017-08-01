/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The part represent the part in the assembly line. It contains all the meta data 
/// about this part, and will be attached to the actual part gameObject
/// </summary>

namespace WyzLink.Assemble
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    public class AssembleFlowParser
    {
        public static void ParseAssembleFlowFile(string text, Action<int, int> connect)
        {
            var sr = new StringReader(text);
            int lineNumber = 0;
            while (true)
            {
                var line = sr.ReadLine();
                if (line == null)
                {
                    break;
                }

                line.Trim();
                if (line.StartsWith("#"))
                {
                    // Comment, ignore
                }
                else
                {
                    var index = line.IndexOf("->");
                    if (index == -1)
                    {
                        Debug.LogError("The text parsing failed on line: " + line + "@" + lineNumber);
                    }
                    var t0 = line.Substring(0, index);
                    var t1 = line.Substring(index + 2, line.Length - index - 2);
                    int a0;
                    int a1;
                    if (Int32.TryParse(t0, out a0) && Int32.TryParse(t1, out a1))
                    {
                        connect(a0, a1);
                    }
                    else
                    {
                        Debug.LogError("Failed to parse the format of line: " + line + "@" + lineNumber);
                    }
                }
                lineNumber++;
            }
        }
    }
}