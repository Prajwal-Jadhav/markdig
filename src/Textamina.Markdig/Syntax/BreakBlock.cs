﻿using Textamina.Markdig.Helpers;
using Textamina.Markdig.Parsing;

namespace Textamina.Markdig.Syntax
{
    /// <summary>
    /// Repressents a thematic break.
    /// </summary>
    public class BreakBlock : LeafBlock
    {
        public new static readonly BlockParser Parser = new ParserInternal();

        public BreakBlock(BlockParser parser) : base(parser)
        {
            NoInline = true;
        }

        private class ParserInternal : BlockParser
        {
            public override MatchLineResult Match(BlockParserState state)
            {
                var liner = state.Line;
                liner.SkipLeadingSpaces3();

                // 4.1 Thematic breaks 
                // A line consisting of 0-3 spaces of indentation, followed by a sequence of three or more matching -, _, or * characters, each followed optionally by any number of spaces
                var c = liner.Current;

                int count = 0;
                var matchChar = (char)0;
                bool hasSpacesSinceLastMatch = false;
                bool hasInnerSpaces = false;
                while (!liner.IsEol)
                {
                    if (count == 0 && (c == '-' || c == '_' || c == '*'))
                    {
                        matchChar = c;
                        count++;
                    }
                    else if (c == matchChar)
                    {
                        if (hasSpacesSinceLastMatch)
                        {
                            hasInnerSpaces = true;
                        }

                        count++;
                    }
                    else if (!c.IsSpace() || count == 0)
                    {
                        return MatchLineResult.None;
                    }
                    else if (c.IsSpace())
                    {
                        hasSpacesSinceLastMatch = true;
                    }
                    c = liner.NextChar();
                }

                // If it as less than 3 chars or it is a setex heading and we are already in a paragraph, let the paragraph handle it
                var previousParagraphBlock = state.LastBlock as ParagraphBlock;
                // !(previousParagraphBlock.Parent is QuoteBlock || previousParagraphBlock.Parent is ListItemBlock) 
                if (count < 3 || (previousParagraphBlock != null && !(previousParagraphBlock.Parent is QuoteBlock || previousParagraphBlock.Parent is ListItemBlock) && matchChar == '-' && !hasInnerSpaces))
                //if (count < 3 || (previousParagraphBlock != null && matchChar == '-' && !hasInnerSpaces))
                {
                    return MatchLineResult.None;
                }

                state.NewBlocks.Push(new BreakBlock(this));
                return MatchLineResult.Last;
            }
        }
    }
}