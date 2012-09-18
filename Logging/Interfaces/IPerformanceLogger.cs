using System.Collections.Generic;

namespace Common.Logging
{
    public interface IPerformanceLogger
    {
        void LogControllerBenchmark(IControllerInfo controllerInfo);
        
        void LogMethodBenchmark(IMethodInfo methodInfo);
    }

    public interface ICommonPerformanceLoggingInfo
    {
        System.Guid Keyid { get; set; }
        int ExecutionTime { get; set; }
    }

    public interface IControllerInfo : ICommonPerformanceLoggingInfo
    {
        string ControllerName { get; set; }
        string ActionName { get; set; }
        int RenderTime { get; set; }
        string Settings { get; set; }
        int? WidgetInstanceId { get; set; }
        int UserGroupUserId { get; set; }
    }

    public interface IMethodInfo : ICommonPerformanceLoggingInfo
    {
        string MethodName { get; set; }
        int CPU { get; set; }
        float RAM { get; set; }
        IEnumerable<int> Sites { get; set; }
    }
}
