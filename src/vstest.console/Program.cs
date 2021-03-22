// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CommandLine
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading;

    using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
    using Microsoft.VisualStudio.TestPlatform.Utilities; 

    /// <summary>
    /// Main entry point for the command line runner.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main entry point. Hands off execution to the executor class.
        /// </summary>
        /// <param name="args">Arguments provided on the command line.</param>
        /// <returns>0 if everything was successful and 1 otherwise.</returns>
        public static int Main(string[] args)
        {            
            var debugEnabled = Environment.GetEnvironmentVariable("VSTEST_RUNNER_DEBUG");
            if (!string.IsNullOrEmpty(debugEnabled) && debugEnabled.Equals("1", StringComparison.Ordinal))
            {
                ConsoleOutput.Instance.WriteLine("Waiting for debugger attach...", OutputLevel.Information);

                var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                ConsoleOutput.Instance.WriteLine(
                    string.Format("Process Id: {0}, Name: {1}", currentProcess.Id, currentProcess.ProcessName),
                    OutputLevel.Information);

                while (!Debugger.IsAttached)
                {
                    Thread.Sleep(1000);
                }

                Debugger.Break();
            }

            SetCultureSpecifiedByUser();

            return new Executor(ConsoleOutput.Instance).Execute(args);
        }

        public static int RunExecutor(string[] args, ITestPlatformEventSource testPlatformEventSource, IObjectWriter objectWriter = null, ITestLoggerManager vsTestLogManager = null)
        {
            var debugEnabled = Environment.GetEnvironmentVariable("VSTEST_RUNNER_DEBUG");
            if (!string.IsNullOrEmpty(debugEnabled) && debugEnabled.Equals("1", StringComparison.Ordinal))
            {
                ConsoleOutput.Instance.WriteLine("Waiting for debugger attach...", OutputLevel.Information);

                var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                ConsoleOutput.Instance.WriteLine(
                    string.Format("Process Id: {0}, Name: {1}", currentProcess.Id, currentProcess.ProcessName),
                    OutputLevel.Information);

                while (!Debugger.IsAttached)
                {
                    Thread.Sleep(1000);
                }

                Debugger.Break();
            }

            SetCultureSpecifiedByUser();

            var executor = new Executor(ConsoleOutput.Instance, testPlatformEventSource);

            if (objectWriter != null)
            {
                executor.ObjectWriter = objectWriter;
            }

            if (vsTestLogManager != null)
            {
                executor.VStestLogManager = vsTestLogManager;
            }

            return executor.Execute(args);
        }

        private static void SetCultureSpecifiedByUser()
        {
            var userCultureSpecified = Environment.GetEnvironmentVariable(CoreUtilities.Constants.DotNetUserSpecifiedCulture);
            if(!string.IsNullOrWhiteSpace(userCultureSpecified))
            {
                try
                {
                    CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CreateSpecificCulture(userCultureSpecified);
                }
                catch(Exception)
                {
                    ConsoleOutput.Instance.WriteLine(string.Format("Invalid Culture Info: {0}", userCultureSpecified), OutputLevel.Information);
                }
            }
        }
    }
}
