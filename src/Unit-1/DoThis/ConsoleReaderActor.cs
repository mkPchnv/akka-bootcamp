using System;
using Akka.Actor;

namespace WinTail
{
    /// <summary>
    /// Actor responsible for reading FROM the console. 
    /// Also responsible for calling <see cref="ActorSystem.Terminate"/>.
    /// </summary>
    class ConsoleReaderActor : UntypedActor
    {
        public const string ExitCommand = "exit";
        public const string StartCommand = "start";
        private IActorRef _consoleWriterActor;

        public ConsoleReaderActor(IActorRef consoleWriterActor)
        {
            _consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
		{
			if (message.Equals(StartCommand))
			{
				DoPrintInstructions();
			}

			GetAndValidate();
		}

		private void GetAndValidate()
		{
            var msg = Console.ReadLine();

			if (msg is string received && !string.IsNullOrEmpty(received) && String.Equals(received, ExitCommand, StringComparison.OrdinalIgnoreCase))
			{
				// shut down the system (acquire handle to system via this actors context)
				Context.System.Terminate();
				return;
			}
            
            _consoleWriterActor.Tell(msg, ActorRefs.NoSender);
		}

		private static void DoPrintInstructions()
        {
            Console.WriteLine("Write whatever you want into the console!");
            Console.Write("Some lines will appear as");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" red ");
            Console.ResetColor();
            Console.Write(" and others will appear as");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(" green! ");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Type 'exit' to quit this application at any time.\n");
            Console.WriteLine("Please provide the URI of a log file on disk.\n");
        }
    }
}