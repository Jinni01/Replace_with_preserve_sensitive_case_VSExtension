/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using System.Text.RegularExpressions;
using PromptGenerator;

namespace ReplaceCommandImplementation
{
    public static class Replacer
    {
        public static void ReplaceSelectedLines(ITextView textView, IEditorOperations editorOperations)
        {
            if (textView.Selection.IsEmpty)
            {
                return;
            }

            var selectedSpan = textView.Selection.SelectedSpans[0];

            string origin, change;
            string title = "Replace With Preserve Sensitive Case";

            origin = Prompt.ShowDialog("Replace From : ", title);
            if (origin.Length == 0) return;

            change = Prompt.ShowDialog("Replace To : ", title);
            if (origin.Length != change.Length) return;

            string trans = Regex.Replace(selectedSpan.GetText(), origin,
                new MatchEvaluator((Match match) =>
                {
                    string val = match.Value;
                    char[] arrayVal = val.ToCharArray();

                    int len = val.Length;
                    for (int i = 0; i < len; ++i)
                    {
                        if (char.IsUpper(arrayVal[i])) arrayVal[i] = char.ToUpper(change[i]);
                        else arrayVal[i] = char.ToLower(change[i]);
                    }

                    return new string(arrayVal);
                })
                , RegexOptions.IgnoreCase);

            textView.TextBuffer.Replace(selectedSpan, trans);
            //editorOperations.MoveToEndOfLine(extendSelection: false);
            //editorOperations.Delete();
            editorOperations.DeleteHorizontalWhiteSpace();
        }
    }
}
