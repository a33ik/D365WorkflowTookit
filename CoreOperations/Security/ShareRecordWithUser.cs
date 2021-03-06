﻿using System.Activities;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using UltimateWorkflowToolkit.Common;

namespace UltimateWorkflowToolkit.CoreOperations.Security
{
    public class ShareRecordWithUser : CrmWorkflowBase
    {
        #region Input/Output Parameters

        [Input("Record Reference")]
        [RequiredArgument]
        public InArgument<string> Record { get; set; }

        [Input("User")]
        [RequiredArgument]
        [ReferenceTarget("systemuser")]
        public InArgument<EntityReference> User { get; set; }

        [Input("Read Access")]
        [RequiredArgument]
        [Default("False")]
        public InArgument<bool> ReadAccess { get; set; }

        [Input("Write Access")]
        [RequiredArgument]
        [Default("False")]
        public InArgument<bool> WriteAccess { get; set; }

        [Input("Delete Access")]
        [RequiredArgument]
        [Default("False")]
        public InArgument<bool> DeleteAccess { get; set; }

        [Input("Append Access")]
        [RequiredArgument]
        [Default("False")]
        public InArgument<bool> AppendAccess { get; set; }

        [Input("Append To Access")]
        [RequiredArgument]
        [Default("False")]
        public InArgument<bool> AppendToAccess { get; set; }

        [Input("Assign Access")]
        [RequiredArgument]
        [Default("False")]
        public InArgument<bool> AssignAccess { get; set; }

        [Input("Share Access")]
        [RequiredArgument]
        [Default("False")]
        public InArgument<bool> ShareAccess { get; set; }

        #endregion Input/Output Parameters

        protected override void ExecuteWorkflowLogic()
        {
            var target = ConvertToEntityReference(Record.Get(Context.ExecutionContext));

            #region Build Sharing Mask

            var rights = AccessRights.None;

            if (ReadAccess.Get(Context.ExecutionContext))
            {
                rights |= AccessRights.ReadAccess;
            }

            if (WriteAccess.Get(Context.ExecutionContext))
            {
                rights |= AccessRights.WriteAccess;
            }

            if (DeleteAccess.Get(Context.ExecutionContext))
            {
                rights |= AccessRights.DeleteAccess;
            }

            if (AppendAccess.Get(Context.ExecutionContext))
            {
                rights |= AccessRights.AppendAccess ;
            }

            if (AppendToAccess.Get(Context.ExecutionContext))
            {
                rights |= AccessRights.AppendToAccess;
            }

            if (AssignAccess.Get(Context.ExecutionContext))
            {
                rights |= AccessRights.AssignAccess;
            }

            if (ShareAccess.Get(Context.ExecutionContext))
            {
                rights |= AccessRights.ShareAccess;
            }

            #endregion Build Sharing Mask

            Context.SystemService.Execute(new ModifyAccessRequest()
            {
                PrincipalAccess = new PrincipalAccess()
                {
                    AccessMask = rights,
                    Principal = User.Get(Context.ExecutionContext)
                },
                Target = target
            });
        }
    }
}
