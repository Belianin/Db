using System;
using System.Collections;
using System.Collections.Generic;
using Db.Metrics.Counter;
using Db.Metrics.Counter.Integer;

namespace Db.Metrics
{
    public interface IMetricsContext
    {
        IIntegerCounter CreateIntegerCounter(string name, TimeSpan aggregationPeriod);

        IEnumerable<CounterEvent<int>> GetIntegerCountMetrics(string name);
    }
}