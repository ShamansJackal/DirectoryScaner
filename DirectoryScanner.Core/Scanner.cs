using DirectoryScanner.Core.Struct;
using System.Collections.Concurrent;

namespace DirectoryScanner.Core
{
    public class Scanner
    {
        private uint _threadCount;
        private Semaphore _semaphore;
        private ConcurrentDictionary<Thread, int> _threads = new();

        public readonly DirectoryNode Root;

        private ConcurrentQueue<DirectoryNode> _queqe = new();

        public Scanner(uint threadCount, string path)
        {
            _threadCount = threadCount;
            _semaphore = new Semaphore((int)threadCount, (int)threadCount);
            Root = new DirectoryNode(path, null);
            _queqe.Enqueue(Root);
        }

        public void StartProcess()
        {
            while (_queqe.Any() || _threads.Any())
            {
                _semaphore.WaitOne();
                if (_queqe.TryDequeue(out DirectoryNode directory))
                {
                    Thread thread = new(obj => ScanNode((DirectoryNode)obj));
                    _threads[thread] = thread.ManagedThreadId;
                    thread.Start(directory);
                }
                _semaphore.Release();
            }

            Thread.Sleep(5000);
        }

        private void ScanNode(DirectoryNode node)
        {
            _semaphore.WaitOne();
            var dir = new DirectoryInfo(node.Fullpath);
            foreach (var subDir in dir.EnumerateDirectories())
            {
                var subNode = new DirectoryNode(subDir.FullName, node);
                _queqe.Enqueue(subNode);

                node.Add(subNode);
                Thread.Sleep(5000);
            }

            foreach (var file in dir.EnumerateFiles())
            {
                Thread.Sleep(5000);
                node.Add(new FileNode(file.FullName, file.Length, node));
            }

            node.IsComplited = true;
            _threads.TryRemove(new (Thread.CurrentThread, Environment.CurrentManagedThreadId));
            _semaphore.Release();
        } 
    }
}