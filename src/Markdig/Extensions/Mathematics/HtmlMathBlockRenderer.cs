// Copyright (c) Alexandre Mutel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license.
// See the license.txt file in the project root for more information.

using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Markdig.Extensions.Mathematics
{
    /// <summary>
    /// A HTML renderer for a <see cref="MathBlock"/>.
    /// </summary>
    /// <seealso cref="HtmlObjectRenderer{T}" />
    public class HtmlMathBlockRenderer : HtmlObjectRenderer<MathBlock>
    {
        protected override void Write(HtmlRenderer renderer, MathBlock obj)
        {
            renderer.EnsureLine();
            if (renderer.EnableHtmlForBlock)
            {
                renderer.Write("<div").WriteAttributes(obj).WriteLine(">");
                renderer.WriteLine(" $$ ");
            }

            renderer.WriteLeafRawLines(obj, true, renderer.EnableHtmlEscape);

            if (renderer.EnableHtmlForBlock)
            {
                renderer.Write(" $$ ");
                renderer.WriteLine("</div>");
            }
        }
    }
}