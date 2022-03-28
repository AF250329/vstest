// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CommandLine;

using Execution;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Output;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;

using Utilities;

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
        DebuggerBreakpoint.AttachVisualStudioDebugger("VSTEST_RUNNER_DEBUG_ATTACHVS");
        DebuggerBreakpoint.WaitForDebugger("VSTEST_RUNNER_DEBUG");
        UiLanguageOverride.SetCultureSpecifiedByUser();
        return new Executor(ConsoleOutput.Instance).Execute(args);
    }

    public static int RunExecutor(string[] args,  IOutput vsTestLogger, ITestPlatformEventSource testPlatformEventSource, IObjectWriter objectWriter = null, ITestLoggerManager vsTestLogManager = null)
    {
        //DebuggerBreakpoint.AttachVisualStudioDebugger("VSTEST_RUNNER_DEBUG_ATTACHVS");
        //DebuggerBreakpoint.WaitForDebugger("VSTEST_RUNNER_DEBUG");
        UiLanguageOverride.SetCultureSpecifiedByUser();

        // var executor = new Executor(ConsoleOutput.Instance, testPlatformEventSource);
        var executor = new Executor(vsTestLogger, testPlatformEventSource);

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
}
