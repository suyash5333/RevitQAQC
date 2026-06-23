using System;
using System.Reflection;
using Autodesk.Revit.UI;

namespace RevitQAQC.Addin.Application
{
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            string tabName = "QAQC";

            try
            {
                application.CreateRibbonTab(tabName);
            }
            catch
            {
                // Tab already exists
            }

            RibbonPanel panel =
                application.CreateRibbonPanel(
                    tabName,
                    "Model Quality");

            string assemblyPath =
                Assembly.GetExecutingAssembly().Location;

            PushButtonData buttonData =
                new PushButtonData(
                    "RunQAQC",
                    "Run QAQC",
                    assemblyPath,
                    "RevitQAQC.Addin.Commands.RunQAQCCommand");

            panel.AddItem(buttonData);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}