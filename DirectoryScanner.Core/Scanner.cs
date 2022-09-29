using DirectoryScanner.Core.Struct;
using System.Collections.Concurrent;

namespace DirectoryScanner.Core
{
    public class Scanner
    {
        private uint _threadCount;
        private Semaphore _semaphore;

        public readonly DirectoryNode Root;

        private ConcurrentQueue<DirectoryNode> _queqe = new();

        public Scanner(uint threadCount, string path)
        {
            _threadCount = threadCount;
            _semaphore = new Semaphore((int)threadCount, (int)threadCount);
            Root = new DirectoryNode(path);
            _queqe.Enqueue(Root);
        }

        public void StartProcess()
        {
            while (_queqe.Any())
            {
                try
                {
                    _semaphore.WaitOne();
                    if (_queqe.TryDequeue(out DirectoryNode directory))
                    {
                        Thread thread = new(obj => ScanNode((DirectoryNode)obj));
                        thread.Start(directory);
                    }
                    _semaphore.Release();
                }catch(Exception ex)
                {
                    int i = 5;
                }
            }
        }

        private void ScanNode(DirectoryNode node)
        {
            _semaphore.WaitOne();
            var dir = new DirectoryInfo(node.Fullpath);
            foreach (var subDir in dir.EnumerateDirectories())
            {
                var subNode = new DirectoryNode(subDir.FullName);
                _queqe.Enqueue(subNode);

                node.Add(subNode);
            }

            foreach (var file in dir.EnumerateFiles())
            {
                Thread.Sleep(5000);
                node.Add(new FileNode(file.FullName, file.Length));
            }

            node.IsComplited = true;
            _semaphore.Release();
        } 
    }
}