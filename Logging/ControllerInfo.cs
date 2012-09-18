using System;

namespace Common.Logging
{
    public class ControllerInfo : IControllerInfo
    {
        #region Implementation of ICommonPerformanceLoggingInfo

        public Guid Keyid { get; set; }
        public int ExecutionTime { get; set; }
        public int? Sites { get; set; }

        #endregion

        #region Implementation of IControllerInfo

        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public int RenderTime { get; set; }
        public string Settings { get; set; }
        public int? WidgetInstanceId { get; set; }
        public int UserGroupUserId { get; set; }

        #endregion
    }
}
