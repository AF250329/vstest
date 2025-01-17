﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

/// <summary>
/// Event arguments used to notify the caller about the status of the test session.
/// </summary>
[DataContract]
public class StopTestSessionCompleteEventArgs : EventArgs
{
    /// <summary>
    /// Creates an instance of the current class.
    /// </summary>
    public StopTestSessionCompleteEventArgs()
    { }

    /// <summary>
    /// Creates an instance of the current class.
    /// </summary>
    /// 
    /// <param name="testSessionInfo">The test session info.</param>
    public StopTestSessionCompleteEventArgs(TestSessionInfo testSessionInfo)
    {
        TestSessionInfo = testSessionInfo;
    }

    /// <summary>
    /// Gets or sets the test session info.
    /// </summary>
    [DataMember]
    public TestSessionInfo TestSessionInfo { get; set; } = null;

    /// <summary>
    /// Gets or sets the metrics.
    /// </summary>
    [DataMember]
    public IDictionary<string, object> Metrics { get; set; } = null;

    /// <summary>
    /// Gets or sets a value indicating if the session was successfully stopped or not.
    /// </summary>
    [DataMember]
    public bool IsStopped { get; set; } = false;
}
