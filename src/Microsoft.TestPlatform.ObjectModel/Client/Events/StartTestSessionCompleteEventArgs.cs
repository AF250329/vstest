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
public class StartTestSessionCompleteEventArgs : EventArgs
{
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
}
