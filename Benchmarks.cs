using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using LibLogBenchmarks.Logging;
using LibLogBenchmarks.Logging.LogProviders;
using NLog;
using NLog.Config;
using NLog.Targets;
using Logger = NLog.Logger;
using LogLevel = NLog.LogLevel;

namespace LibLogBenchmarks
{
    public class Benchmarks
    {
        static readonly Logger NLogLogger = LogManager.GetCurrentClassLogger();
        static readonly ILog LibLogLogger = LogProvider.For<Benchmarks>();

        [Params(true, false)]
        public bool IsDisabled;

        [Setup]
        public void Setup()
        {
            var config = new LoggingConfiguration();
            var nullTarget = new NullTarget();
            config.AddTarget("console", nullTarget);
            var rule = new LoggingRule("*", IsDisabled ? LogLevel.Info : LogLevel.Debug, nullTarget);
            config.LoggingRules.Add(rule);
            LogManager.Configuration = config;

            LogProvider.SetCurrentLogProvider(new NLogLogProvider());
        }

        [Benchmark(Baseline = true)]
        public void NLog() => NLogLogger.Debug("Sample debug message");

        [Benchmark]
        public void LibLog() => LibLogLogger.Debug("Sample debug message");
    }
}
