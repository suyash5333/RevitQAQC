using System;
using RevitQAQC.Shared.Models;

namespace RevitQAQC.Interfaces.Services
{
    public interface IQAScanService
    {
        void RunScan(Action<ReportModel> completionCallback);
    }
}