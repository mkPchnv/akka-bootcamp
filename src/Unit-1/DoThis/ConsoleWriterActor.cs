using System;
using Akka.Actor;
using WinTail.Messages;

namespace WinTail
{
    /// <summary>
    /// Actor responsible for serializing message writes to the console.
    /// (write one message at a time, champ :)
    /// </summary>
    class ConsoleWriterActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            if (message is EmptyInput empty)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(empty.Text);
            }

            else if (message is InputSuccess success)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(success.Text);
            }

            else
                Console.WriteLine(message);

            Console.ResetColor();
        }
    }
}
