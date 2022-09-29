using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryScanner.Core.Struct
{
    public sealed class DirectoryNode : Node
    {
        public bool IsComplited = false;

        public IReadOnlyCollection<Node> Childs => _childs;

        private List<Node> _childs = new();

        public DirectoryNode(string path) : base(path)
        {
        }

        public override long? Size => IsComplited && Childs.All(x => x.Size.HasValue) ? Childs.Sum(x => x.Size) : null;

        public void Add(Node node)
        {
            _childs.Add(node);
            OnPropertyChanged(nameof(Size));
            OnPropertyChanged(nameof(Childs));
            OnPropertyChanged(nameof(Percent));
        }
    }
}
