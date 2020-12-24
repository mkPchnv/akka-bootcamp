using System;
﻿using Akka.Actor;

namespace WinTail
{
    #region Program
    class Program
    {
        public static ActorSystem MyActorSystem;

        static void Main(string[] args)
        {
            MyActorSystem = ActorSystem.Create("meekoo-system");

            var consoleWriterProps = Props.Create(() => new ConsoleWriterActor());
            var consoleWriter = MyActorSystem.ActorOf(consoleWriterProps, "console-writer");

            var tailCoordinatorProps = Props.Create(() => new TailCoordinatorActor());
            var tailCoordinator = MyActorSystem.ActorOf(tailCoordinatorProps, "tail-coordinator");

            var fileValidatorActorProps = Props.Create(() => new FileValidatorActor(consoleWriter, tailCoordinator));
            var fileValidatorActor = MyActorSystem.ActorOf(fileValidatorActorProps, "validator");

            var consoleReaderProps = Props.Create<ConsoleReaderActor>(fileValidatorActor);
            var consoleReader = MyActorSystem.ActorOf(consoleReaderProps, "console-reader");

            consoleReader.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.WhenTerminated.Wait();
        }
    }
    #endregion
}
