using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace patterns_playground
{
    class Program
    {
        static void Main(string[] args)
        {
            ValidatorsRegistry.RegisterValidatorFor<Message>(new MessageValidator());

            RunCommand(new Message("Hello world!", Severity.Average));
            RunCommand(new Message("Hello world!", Severity.Minor));
            RunCommand(new Message("Hello world!", Severity.Critical));
            RunCommand(new Message("\t\t\t", Severity.Critical));
        }

        private static void RunCommand(Message message)
        {
            var command = new PrintMessageCommand();
            command.Execute(message);

            if (command.State == CommandState.Completed)
            {
                Console.WriteLine("Command executed successfully");
            }
            else if (command.State == CommandState.Failed || command.State == CommandState.InvalidParameters)
            {
                Console.WriteLine("Command failed to complete. The error code is {0}, the error message is: {1}",
                    command.State, command.Error.Message);
            }
        }
    }
}