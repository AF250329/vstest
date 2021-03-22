// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using Microsoft.VisualStudio.TestPlatform.CommandLine;
    using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
    using Microsoft.VisualStudio.TestPlatform.Utilities;

    using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

    /// <summary>
    /// Argument Executor for the "/TestCaseFilter" command line argument.
    /// </summary>
    internal class TestCaseFilterArgumentProcessor : IArgumentProcessor
    {
        #region Constants

        /// <summary>
        /// The name of the command line argument that the TestCaseFilterArgumentExecutor handles.
        /// </summary>
        public const string CommandName = "/TestCaseFilter";

        #endregion

        private Lazy<IArgumentProcessorCapabilities> metadata;

        private Lazy<IArgumentExecutor> executor;

        /// <summary>
        /// Gets the metadata.
        /// </summary>
        public Lazy<IArgumentProcessorCapabilities> Metadata
        {
            get
            {
                if (this.metadata == null)
                {
                    this.metadata = new Lazy<IArgumentProcessorCapabilities>(() => new TestCaseFilterArgumentProcessorCapabilities());
                }

                return this.metadata;
            }
        }

        /// <summary>
        /// Gets or sets the executor.
        /// </summary>
        public Lazy<IArgumentExecutor> Executor
        {
            get
            {
                if (this.executor == null)
                {
                    this.executor = new Lazy<IArgumentExecutor>(() => new TestCaseFilterArgumentExecutor(CommandLineOptions.Instance));
                }

                return this.executor;
            }

            set
            {
                this.executor = value;
            }
        }
    }

    internal class TestCaseFilterArgumentProcessorCapabilities : BaseArgumentProcessorCapabilities
    {
        public override string CommandName => TestCaseFilterArgumentProcessor.CommandName;

        public override bool AllowMultiple => false;

        public override bool IsAction => false;

        public override ArgumentProcessorPriority Priority => ArgumentProcessorPriority.Normal;

        public override string HelpContentResourceName => CommandLineResources.TestCaseFilterArgumentHelp;

        public override HelpContentPriority HelpPriority => HelpContentPriority.TestCaseFilterArgumentProcessorHelpPriority;
    }

    /// <summary>
    /// Argument Executor for the "/TestCaseFilter" command line argument.
    /// </summary>
    internal class TestCaseFilterArgumentExecutor : IArgumentExecutor
    {
        #region Fields

        /// <summary>
        /// Used for getting sources.
        /// </summary>
        private CommandLineOptions commandLineOptions;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public TestCaseFilterArgumentExecutor(CommandLineOptions options)
        {
            Contract.Requires(options != null);
            this.commandLineOptions = options;
        }
        #endregion

        #region IArgumentExecutor

        /// <summary>
        /// Initializes with the argument that was provided with the command.
        /// </summary>
        /// <param name="argument">Argument that was provided with the command.</param>
        public void Initialize(string argument)
        {
            var defaultFilter = this.commandLineOptions.TestCaseFilterValue;
            var hasDefaultFilter = !string.IsNullOrWhiteSpace(defaultFilter);
            
            if (!hasDefaultFilter && string.IsNullOrWhiteSpace(argument))
            {
                throw new CommandLineException(string.Format(CultureInfo.CurrentUICulture, CommandLineResources.TestCaseFilterValueRequired));
            }

            if (!hasDefaultFilter)
            {
                this.commandLineOptions.TestCaseFilterValue = argument;
            }
            else
            {
                // Merge default filter an provided filter by AND operator to have both the default filter and custom filter applied.
                this.commandLineOptions.TestCaseFilterValue = $"({defaultFilter})&({argument})";
            }
        }

        /// <summary>
        /// The TestCaseFilter is already set, return success.
        /// </summary>
        /// <returns> The <see cref="ArgumentProcessorResult"/> Success </returns>
        public ArgumentProcessorResult Execute(IObjectWriter objectWriter = null, ITestPlatformEventSource testPlatformEventSource = null, ITestLoggerManager vsTestLogManager = null)
        {
            return ArgumentProcessorResult.Success;
        }
        #endregion
    }
}
