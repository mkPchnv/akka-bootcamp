using Akka.Actor;
using System.IO;
using System.Text;
using WinTail.Messages;

namespace WinTail
{
	public class TailActor : UntypedActor
	{
		private readonly string _path;
		private readonly IActorRef _reportThemActor;
		private readonly FileObserver _observer;
		private readonly Stream _fileStream;
		private readonly StreamReader _fileStreamReader;

		public TailActor(
			IActorRef reporter,
			string path)
		{
			_reportThemActor = reporter;
			_path = path;

			_observer = new FileObserver(Self, Path.GetFullPath(_path));
			_observer.Start();

			// open the file stream with shared read/write permissions
			// (so file can be written to while open)
			_fileStream = new FileStream(Path.GetFullPath(_path),
				FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			_fileStreamReader = new StreamReader(_fileStream, Encoding.UTF8);

			// read the initial contents of the file and send it to console as first msg
			var text = _fileStreamReader.ReadToEnd();
			Self.Tell(new InitRead(_path, text));
		}

		protected override void OnReceive(object message)
		{
			if (message is InitRead startRead && startRead.Text.Length > 0)
				_reportThemActor.Tell(startRead.Text);

			if (message is FileError error)
				_reportThemActor.Tell(string.Format("Tail error: {0}", error.ErrorReason));

			if (message is FileWrite startWrite)
			{
				var text = _fileStreamReader.ReadToEndAsync().GetAwaiter().GetResult();
				if (!string.IsNullOrEmpty(text))
					_reportThemActor.Tell(text);
			}
		}
	}
}
