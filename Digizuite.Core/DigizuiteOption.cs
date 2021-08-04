using System;

namespace Digizuite
{
    [Flags]
    public enum DigizuiteOption
    {
        Nothing = 1 << 0,
        SkipCache = 1 << 1,
        UseNewUploadService = 1 << 2
    }
}