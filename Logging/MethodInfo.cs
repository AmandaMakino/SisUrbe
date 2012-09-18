using System;
using System.Collections.Generic;

namespace Common.Logging
{
    public class MethodInfo : IMethodInfo
    {
        #region Implementation of ICommonPerformanceLoggingInfo

        public Guid Keyid { get; set; }
        public int ExecutionTime { get; set; }

        #endregion

        #region Implementation of IMethodInfo

        public string MethodName { get; set; }
        public int CPU { get; set; }
        public float RAM { get; set; }
        public IEnumerable<int> Sites { get; set; }

        #endregion
    }
}
