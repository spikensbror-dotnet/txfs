using System;
using System.Collections.Generic;

namespace Txfs.Impl
{
    class FileSystemTransactionLog : IFileSystemTransactionLog
    {
        public Stack<Action> CommitActions { get; } = new Stack<Action>();
        public Stack<Action> RollbackActions { get; } = new Stack<Action>();
    }
}
