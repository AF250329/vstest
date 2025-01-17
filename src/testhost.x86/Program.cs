// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.TestHost;

using System;
using System.Collections.Generic;
using System.Diagnostics;

using CoreUtilities.Helpers;
using CoreUtilities.Tracing;

using Execution;

using ObjectModel;

/// <summary>
/// The program.
/// </summary>
public class Program
{
    private const string TestSourceArgumentString = "--testsourcepath";

    /// <summary>
    /// The main.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    public static void Main(string[] args)
    {
        try
        {
            //while (!Debugger.IsAttached)
            //{
            //    System.Threading.Thread.Sleep(1000);
            //}
            //
            //Debugger.Break();

            Logger.Instance.WriteInfo($"[Main] Started\nBefore running VSTests\n---------------------------------------------------------------------------------" +
                                                    $"Id = {System.Diagnostics.Process.GetCurrentProcess().Id}\n" +
                                                    // $"HandleCount = {System.Diagnostics.Process.GetCurrentProcess().HandleCount}\n" +
                                                    $"MaxWorkingSet = {System.Diagnostics.Process.GetCurrentProcess().MaxWorkingSet} bytes\n" +
                                                    $"MinWorkingSet = {System.Diagnostics.Process.GetCurrentProcess().MinWorkingSet} bytes\n" +
                                                    $"Modules count = {System.Diagnostics.Process.GetCurrentProcess().Modules.Count}\n" +
                                                    $"NonpagedSystemMemorySize64 = {System.Diagnostics.Process.GetCurrentProcess().NonpagedSystemMemorySize64} bytes\n" +
                                                    $"PagedMemorySize64 = {System.Diagnostics.Process.GetCurrentProcess().PagedMemorySize64} bytes\n" +
                                                    $"PeakPagedMemorySize64 = {System.Diagnostics.Process.GetCurrentProcess().PeakPagedMemorySize64} bytes\n" +
                                                    $"PeakVirtualMemorySize64 = {System.Diagnostics.Process.GetCurrentProcess().PeakVirtualMemorySize64} bytes\n" +
                                                    $"PeakWorkingSet64 = {System.Diagnostics.Process.GetCurrentProcess().PeakWorkingSet64} bytes\n" +
                                                    $"PrivateMemorySize64 = {System.Diagnostics.Process.GetCurrentProcess().PrivateMemorySize64} bytes\n" +
                                                    $"VirtualMemorySize64 = {System.Diagnostics.Process.GetCurrentProcess().VirtualMemorySize64} bytes\n" +
                                                    $"WorkingSet64 = {System.Diagnostics.Process.GetCurrentProcess().WorkingSet64} bytes");

            TestPlatformEventSource.Instance.TestHostStart();

            Run(args);
        }
        catch (Exception ex)
        {
            EqtTrace.Error("TestHost: Error occurred during initialization of TestHost : {0}", ex);

            // Throw exception so that vstest.console get the exception message.
            throw;
        }
        finally
        {
            TestPlatformEventSource.Instance.TestHostStop();
            EqtTrace.Info("Testhost process exiting.");
        }
    }

    // In UWP(App models) Run will act as entry point from Application end, so making this method public
    public static void Run(string[] args)
    {
        // DebuggerBreakpoint.AttachVisualStudioDebugger("VSTEST_HOST_DEBUG_ATTACHVS");
        // DebuggerBreakpoint.WaitForNativeDebugger("VSTEST_HOST_NATIVE_DEBUG");
        // DebuggerBreakpoint.WaitForDebugger("VSTEST_HOST_DEBUG");
        UiLanguageOverride.SetCultureSpecifiedByUser();
        var argsDictionary = CommandLineArgumentsHelper.GetArgumentsDictionary(args);

        Logger.Instance.WriteInfo("[Run] Received: {argsDictionary.Count} arguments");

        if (argsDictionary.Keys.Contains("--diag") == false)
        {
            argsDictionary.Add("--diag", "C:\\test.host.diagnostic.log");
        }
        else
        {
            // Already contains
        }

        // Invoke the engine with arguments
        GetEngineInvoker(argsDictionary).Invoke(argsDictionary);
    }

    private static IEngineInvoker GetEngineInvoker(IDictionary<string, string> argsDictionary)
    {
        IEngineInvoker invoker = null;
#if NETFRAMEWORK
        // If Args contains test source argument, invoker Engine in new appdomain
        if (argsDictionary.TryGetValue(TestSourceArgumentString, out var testSourcePath) && !string.IsNullOrWhiteSpace(testSourcePath))
        {
            // remove the test source arg from dictionary
            argsDictionary.Remove(TestSourceArgumentString);

            // Only DLLs and EXEs can have app.configs or ".exe.config" or ".dll.config"
            if (System.IO.File.Exists(testSourcePath) &&
                (testSourcePath.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)
                 || testSourcePath.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)))
            {
                invoker = new AppDomainEngineInvoker<DefaultEngineInvoker>(testSourcePath);
            }
        }
#endif
        return invoker ?? new DefaultEngineInvoker();
    }
}
