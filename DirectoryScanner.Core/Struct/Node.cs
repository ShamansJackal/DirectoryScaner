using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace DirectoryScanner.Core.Struct
{
    public abstract class Node : INotifyPropertyChanged
    {
        public abstract long? Size { get; }

        public bool HasChilds => this is DirectoryNode;

        public string Name => Path.GetFileName(Fullpath);

        public string Fullpath;

        public DirectoryNode? Parent;

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public float? Percent => Parent != null && Parent.Size.HasValue ? Size / Parent.Size : null;

        public Node(string path)
        {
            Fullpath = path;
            PropertyChanged += Parent?.PropertyChanged;
        }
    }
}
