using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace patterns_playground
{
    [Serializable]
    public class BusinessRulesViolatedException : Exception
    {
        public BusinessRulesViolatedException(string message)
            : base(message)
        {

        }
    }

    public interface ICommand<TCommandParameters>
    {
        void Execute(TCommandParameters parameters);
    }

    public enum CommandState
    { 
        Executing,
        Completed,
        Failed,
        InvalidParameters
    }

    public abstract class Command<T> : ICommand<T>
        where T: IValidatable
    {
        public CommandState State { get; private set; }
        public Exception Error { get; private set; }

        public void Execute(T parameters)
        {
            if (parameters == null)
            {
                State = CommandState.InvalidParameters;
                throw new ArgumentException("Parameter cannot be null", "parameters");
            }

            State = CommandState.Executing;
            try
            {
                IEnumerable<string> brokenRules;
                if (!parameters.Validate(out brokenRules)) 
                {
                    throw new BusinessRulesViolatedException(string.Join(Environment.NewLine, brokenRules));
                }
                DoExecute(parameters);
            }
            catch (Exception e)
            {
                Error = e;
                State = CommandState.Failed;

                return;
            }

            State = CommandState.Completed;
        }

        protected abstract void DoExecute(T parameters);
    }

    public enum Severity
    { 
        Minor,
        Average,
        Critical
    }

    public static class ValidationExtensions
    {
        public static bool Validate<T>(this T @object, out IEnumerable<string> brokenRules)
            where T : IValidatable
        {
            var validator = ValidatorsRegistry.GetValidatorFor<T>();
            var result = validator.IsValid(@object);

            brokenRules = validator.BrokenRules;

            return !validator.BrokenRules.Any();
        }
    }

    public class Message : IValidatable
    {
        public Message(string text, Severity severity)
        {
            Text = text;
            Severity = severity;
        }

        public string Text { get; private set; }
        public Severity Severity { get; private set; }
    }

    public class PrintMessageCommand : Command<Message>
    {
        protected override void DoExecute(Message message)
        {
            var savedConsoleColor = Console.ForegroundColor;
            switch (message.Severity)
            {   
                case Severity.Minor:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case Severity.Average:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case Severity.Critical:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }
            Console.WriteLine(message.Text);
            Console.ForegroundColor = savedConsoleColor;
        }
    }

    public interface IValidatable { }

    public interface IValidator<T>
        where T: IValidatable
    {
        IEnumerable<string> BrokenRules { get; set; }
        bool IsValid(T @object);
    }

    public class MessageValidator : IValidator<Message>
    {
        public IEnumerable<string> BrokenRules { get; set; }

        public bool IsValid(Message message)
        {
            var result = new List<string>();
            if (string.IsNullOrWhiteSpace(message.Text))
            {
                result.Add("Message text cannot be blank or whitespace string.");
            }

            BrokenRules = result;

            return !BrokenRules.Any();
        }
    }

    public static class ValidatorsRegistry
    {
        private static readonly Dictionary<Type, object> validators;

        static ValidatorsRegistry()
        {
            validators = new Dictionary<Type, object>();
        }

        public static void RegisterValidatorFor<T>(IValidator<T> validator)
            where T : IValidatable
        {
            validators.Add(typeof(T), validator);
        }

        public static IValidator<T> GetValidatorFor<T>()
            where T : IValidatable
        {
            return (IValidator<T>)validators[typeof(T)];
        }
    }
}
