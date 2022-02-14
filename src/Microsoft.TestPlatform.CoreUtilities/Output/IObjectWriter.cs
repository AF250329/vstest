// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.CoreUtilities.Output;

#pragma warning disable RS0016 // Add public types and members to the declared API
public interface IObjectWriter : Microsoft.VisualStudio.TestPlatform.Utilities.IOutput
#pragma warning restore RS0016 // Add public types and members to the declared API
{
    /// <summary>
    /// Function will send actual object
    /// </summary>
    /// <param name="obj">The actual object.</param>
    void SendObject(object obj);
}

