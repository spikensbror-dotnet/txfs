using System;
using System.Collections.Generic;

namespace Txfs.Impl
{
    interface IFileSystemTransactionLog
    {
        Stack<Action> CommitActions { get; }
        Stack<Action> RollbackActions { get; }
    }
}
