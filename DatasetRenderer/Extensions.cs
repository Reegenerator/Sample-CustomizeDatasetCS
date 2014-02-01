using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Kodeo.Reegenerator.Language;

namespace DatasetRenderer
{
    static class Extensions
    {
      
        static public string Attr(this XElement ele, string attrName)
        {
            return ele.Attribute(attrName).Value;
        }

        static public string ToIdentifier(this string name)
        {
            return FixName(name);
        }


        /// <summary>
        /// Replaces spaces with '_'.
        /// </summary>
        static public string FixName(string name)
        {
            return name.Replace(' ', '_');
        }
    }
}
