using System;
using Autodesk.Revit.UI;

namespace RevitQAQC.Core.Services
{
    public class ElementSelectionService : IDisposable
    {
        private readonly SelectElementHandler _handler;
        private readonly ExternalEvent _externalEvent;

        public ElementSelectionService()
        {
            _handler = new SelectElementHandler();

            _externalEvent =
                ExternalEvent.Create(_handler);
        }

        public void SelectElement(long elementId)
        {
            _handler.ElementId = elementId;

            _externalEvent.Raise();
        }

        public void Dispose()
        {
            _externalEvent?.Dispose();
        }
    }
}