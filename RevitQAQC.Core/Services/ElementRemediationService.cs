using System;
using System.Collections.Generic;
using Autodesk.Revit.UI;

namespace RevitQAQC.Core.Services
{
    public class ElementRemediationService
    {
        private readonly RemediateNamingIssueHandler _handler;
        private readonly ExternalEvent _externalEvent;

        private readonly RemediateAllNamingIssuesHandler _allHandler;
        private readonly ExternalEvent _allExternalEvent;

        public ElementRemediationService()
        {
            _handler =
                new RemediateNamingIssueHandler();

            _externalEvent =
                ExternalEvent.Create(_handler);

            _allHandler =
                new RemediateAllNamingIssuesHandler();

            _allExternalEvent =
                ExternalEvent.Create(_allHandler);
        }

        public void RemediateNamingIssue(
            long elementId,
            string suggestedValue,
            Action<bool> completionCallback)
        {
            _handler.ElementId = elementId;
            _handler.SuggestedValue = suggestedValue;
            _handler.CompletionCallback = completionCallback;

            _externalEvent.Raise();
        }

        public void RemediateAllNamingIssues(
            List<long> elementIds,
            List<string> suggestedValues,
            Action<int> completionCallback)
        {
            _allHandler.ElementIds = elementIds;
            _allHandler.SuggestedValues = suggestedValues;
            _allHandler.CompletionCallback = completionCallback;

            _allExternalEvent.Raise();
        }
    }
}