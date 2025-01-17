﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

using System;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Output;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;

using CommandLineResources = Resources.Resources;

/// <summary>
/// Argument Processor for the "--testSessionCorrelationId|/TestSessionCorrelationId" command line argument.
/// </summary>
internal class TestSessionCorrelationIdProcessor : IArgumentProcessor
{
    /// <summary>
    /// The name of the command line argument that the TestSessionCorrelationIdProcessor handles.
    /// </summary>
    public const string CommandName = "/TestSessionCorrelationId";

    private Lazy<IArgumentProcessorCapabilities> _metadata;

    private Lazy<IArgumentExecutor> _executor;

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    public Lazy<IArgumentProcessorCapabilities> Metadata
    {
        get
        {
            if (_metadata == null)
            {
                _metadata = new Lazy<IArgumentProcessorCapabilities>(() => new TestSessionCorrelationIdProcessorCapabilities());
            }

            return _metadata;
        }
    }

    /// <summary>
    /// Gets or sets the executor.
    /// </summary>
    public Lazy<IArgumentExecutor> Executor
    {
        get
        {
            if (_executor == null)
            {
                _executor = new Lazy<IArgumentExecutor>(() => new TestSessionCorrelationIdProcessorModeProcessorExecutor(CommandLineOptions.Instance));
            }

            return _executor;
        }

        set
        {
            _executor = value;
        }
    }
}

internal class TestSessionCorrelationIdProcessorCapabilities : BaseArgumentProcessorCapabilities
{
    public override string CommandName => TestSessionCorrelationIdProcessor.CommandName;

    public override bool AllowMultiple => false;

    // We put priority at the same level of the argument processor for runsettings passed as argument through cli.
    // We'll be sure to run before test run or artifact post processing.
    public override ArgumentProcessorPriority Priority => ArgumentProcessorPriority.CliRunSettings;

    public override HelpContentPriority HelpPriority => HelpContentPriority.None;

    // We want to be sure that this command won't show in user help
    public override string HelpContentResourceName => null;
}

/// <summary>
/// Argument Executor for the "/TestSessionCorrelationId" command line argument.
/// </summary>
internal class TestSessionCorrelationIdProcessorModeProcessorExecutor : IArgumentExecutor
{
    private readonly CommandLineOptions _commandLineOptions;

    public TestSessionCorrelationIdProcessorModeProcessorExecutor(CommandLineOptions options)
    {
        _commandLineOptions = options ?? throw new ArgumentNullException(nameof(options));
    }

    public void Initialize(string argument)
    {
        if (string.IsNullOrEmpty(argument))
        {
            throw new CommandLineException(CommandLineResources.InvalidTestSessionCorrelationId);
        }

        _commandLineOptions.TestSessionCorrelationId = argument;
        EqtTrace.Verbose($"TestSessionCorrelationIdProcessorModeProcessorExecutor.Initialize: TestSessionCorrelationId '{argument}'");
    }

    public ArgumentProcessorResult Execute(IObjectWriter objectWriter = null, ITestPlatformEventSource testPlatformEventSource = null, ITestLoggerManager vsTestLogManager = null) => ArgumentProcessorResult.Success;
}
