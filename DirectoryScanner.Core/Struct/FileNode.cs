using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryScanner.Core.Struct
{
    public sealed class FileNode : Node
    {
        private readonly long _size;
        public FileNode(string path, long size) : base(path)
        {
            _size = size;
        }

        public override long? Size => _size;
    }
}
