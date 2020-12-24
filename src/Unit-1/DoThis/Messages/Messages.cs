using Akka.Actor;

namespace WinTail.Messages
{
	#region Validator
	public class EmptyInput
	{
		public EmptyInput(string error)
		{
			Text = error;
		}

		public string Text { get; private set; }
	}

	public class Continue
	{
		public Continue() {}
	}

	public class InputSuccess
	{
		public InputSuccess(string message)
		{
			Text = message;
		}

		public string Text { get; private set; }
	}

	public class ValidationError
	{
		public ValidationError(string path)
		{
			FilePath = path;
		}

		public string FilePath { get; private set; }
	}
	#endregion

	#region TailCoordinator
	public class StartTail
	{
		public StartTail(
			string path,
			IActorRef reporter)
		{
			FilePath = path;
			Reporter = reporter;
		}

		public string FilePath { get; private set; }

		public IActorRef Reporter { get; private set; }
	}

	public class StopTail
	{
		public StopTail(string path)
		{
			FilePath = path;
		}

		public string FilePath { get; private set; }
	}
	#endregion

	#region Tail
	public class FileWrite
	{
		public FileWrite(string path)
		{
			FilePath = path;
		}

		public string FilePath { get; private set; }
	}

	public class FileError
	{
		public FileError(
			string path,
			string reason)
		{
			FilePath = path;
			ErrorReason = reason;
		}

		public string FilePath { get; private set; }

		public string ErrorReason { get; private set; }
	}

	public class InitRead
	{
		public InitRead(
			string path,
			string text)
		{
			FilePath = path;
			Text = text;
		}

		public string FilePath { get; private set; }

		public string Text { get; private set; }
	}
	#endregion
}
