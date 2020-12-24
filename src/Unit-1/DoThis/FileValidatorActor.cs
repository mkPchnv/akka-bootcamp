using Akka.Actor;
using System.IO;
using WinTail.Messages;

namespace WinTail
{
	public class FileValidatorActor : UntypedActor
	{
		private readonly IActorRef _consoleWriter;
		private readonly IActorRef _watcher;

		public FileValidatorActor(
			IActorRef writerActor,
			IActorRef watcher)
		{
			_consoleWriter = writerActor;
			_watcher = watcher;
		}

		protected override void OnReceive(object message)
		{
			if (message is string emptyMessage && string.IsNullOrWhiteSpace(emptyMessage))
			{
				_consoleWriter.Tell(new EmptyInput("Input was blank. Please try again.\n"));

				// tell sender to continue doing its thing (whatever that may be,
				// this actor doesn't care)
				Sender.Tell(new Continue());
			}

			if (message is string receiveMessage && !string.IsNullOrWhiteSpace(receiveMessage))
			{
				var valid = IsFileUri(receiveMessage);
				if (valid)
				{
					_consoleWriter.Tell(new InputSuccess(
						string.Format("Starting processing for {0}", receiveMessage)));

					_watcher.Tell(new StartTail(receiveMessage,
						_consoleWriter));
				}
				else
				{
					_consoleWriter.Tell(new ValidationError(
						string.Format("{0} is not an existing URI on disk.", receiveMessage)));

					// tell sender to continue doing its thing (whatever that
					// may be, this actor doesn't care)
					Sender.Tell(new Continue());
				}
			}
		}

		private bool IsFileUri(string path)
			=> File.Exists(path);
	}
}
