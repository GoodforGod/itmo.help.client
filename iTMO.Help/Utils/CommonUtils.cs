using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

using iTMO.Help.Model;

namespace iTMO.Help.Utils
{
    /// <summary>
    /// Common utils for all classes, use
    /// </summary>
    class CommonUtils
    {
        /// <summary>
        /// Used to retrive all ViewControlls childrens of the parent object
        /// </summary>
        /// <param name="parent">
        /// ViewControll parent XAML object
        /// </param>
        /// <returns></returns>
        public static List<FrameworkElement> GetChildren(DependencyObject parent)
        {
            List<FrameworkElement> controls = new List<FrameworkElement>();

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); ++i)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is FrameworkElement)
                    controls.Add(child as FrameworkElement);
                controls.AddRange(GetChildren(child));
            }
            return controls;
        }

        /// <summary>
        /// Clean HTML from HTML tags, transfer <br/> to \n and etc
        /// </summary>
        /// <param name="html">
        /// Plain html text represantation
        /// </param>
        /// <returns></returns>
        public static string HtmlToPlainText(string html)
        {
            //matches one or more (white space or line breaks) between '>' and '<'
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";

            //match any character between '<' and '>', even when end tag is missing
            const string stripFormatting = @"<[^>]*(>|$)";

            //matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";

            //matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            const string linkBreak = @"<(br|BR)\s{0,1}\/{0,1}>";

            var lineBreakRegex          = new Regex(lineBreak,          RegexOptions.Multiline);
            var stripFormattingRegex    = new Regex(stripFormatting,    RegexOptions.Multiline);
            var tagWhiteSpaceRegex      = new Regex(tagWhiteSpace,      RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }
    }
}
