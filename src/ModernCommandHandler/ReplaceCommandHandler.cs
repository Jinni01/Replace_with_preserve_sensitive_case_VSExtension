/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using ReplaceCommandImplementation;
using Microsoft.VisualStudio.Commanding;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace ModernCommandHandler
{
    [Export(typeof(ICommandHandler))]
    [ContentType("text")]
    [Name(nameof(ReplaceCommandHandler))]
    public class ReplaceCommandHandler : ICommandHandler<ReplaceCommandArgs>
    {
        public string DisplayName => "Replace With Preserve Sensitive Case in Selected Range";

        [Import]
        private IEditorOperationsFactoryService EditorOperations = null;

        public CommandState GetCommandState(ReplaceCommandArgs args)
        {
            return args.TextView.Selection.IsEmpty ? CommandState.Unavailable : CommandState.Available;
        }

        public bool ExecuteCommand(ReplaceCommandArgs args, CommandExecutionContext context)
        {
            using (context.OperationContext.AddScope(allowCancellation: false, description: "Replace With Preserve Sensitive Case in Selected Range"))
            {
                //args.TextView.TextBuffer.Insert(0, "// Invoked from modern command handler\r\n");
                Replacer.ReplaceSelectedLines(args.TextView, EditorOperations.GetEditorOperations(args.TextView));
            }

            return true;
        }
    }
}
