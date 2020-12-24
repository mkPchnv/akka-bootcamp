using Akka.Actor;
using WinTail.Messages;

namespace WinTail
{
	public class TailCoordinatorActor : UntypedActor
	{
		protected override void OnReceive(object message)
		{
			if (message is StartTail startCommand && startCommand.FilePath.Length > 0)
				Context.ActorOf(Props.Create(() => new TailActor(startCommand.Reporter, startCommand.FilePath)));
		}
	}
}
