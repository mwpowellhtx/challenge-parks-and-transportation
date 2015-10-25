namespace Challenge.Core
{
    using System;
    using System.IO;

    public class Disposable : IDisposable
    {
        protected Disposable()
        {
        }

        ~Disposable()
        {
            Dispose(false);
        }

        private bool _disposed;

        protected bool IsDisposed
        {
            get { return _disposed; }
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public void Dispose()
        {
            if (IsDisposed) return;
            Dispose(true);
            _disposed = true;
        }
    }

    public abstract class ChallengeBase : Disposable
    {
        private readonly TextWriter _writer;

        protected ChallengeBase(TextReader reader, TextWriter writer)
        {
            _writer = writer;
            Initialize(reader);
        }

        private void Initialize(TextReader reader)
        {
            Read(reader);
        }

        protected abstract void Read(TextReader reader);

        protected virtual string[] ReadLines(TextReader reader)
        {
            return reader.ReadToEndAsync().Result.Replace("\r\n", "\n").Split('\n');
        }

        protected abstract void Report(TextWriter writer);

        protected abstract void Run();

        protected override void Dispose(bool disposing)
        {
            if (!disposing || IsDisposed) return;

            Run();

            Report(_writer);

            base.Dispose(true);
        }
    }
}
