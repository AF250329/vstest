// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.TestHost;

using System;
using System.Collections.Generic;
using System.Text;

internal class Logger
{
    private static Logger _instance = new Logger();

    public static Logger Instance { get { return _instance; } }

    private const string fileName = @"C:\test.hosts.log";

    private Logger()
    {
    }

    public void WriteInfo(string message)
    {
        System.IO.File.AppendAllText(fileName, message);
    }
}
