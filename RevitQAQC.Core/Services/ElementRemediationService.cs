using System;
using Autodesk.Revit.UI;

namespace RevitQAQC.Core.Services
{
    public class ElementRemediationService
    {
        private readonly RemediateNamingIssueHandler _handler;
        private readonly ExternalEvent _externalEvent;

        public ElementRemediationService()
        {
            _handler = new RemediateNamingIssueHandler();

            _externalEvent =
                ExternalEvent.Create(_handler);
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
    }
}