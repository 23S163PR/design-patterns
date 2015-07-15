using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace patterns_playground
{
    public interface ICommand
    {
        void Execute();
    }

    public class Switch
    {
        ICommand _closedCommand;
        ICommand _openedCommand;

        public Switch(ICommand closedCommand, ICommand openedCommand)
        {
            _closedCommand = closedCommand;
            _openedCommand = openedCommand;
        }

        public void Close()
        {
            _closedCommand.Execute();
        }

        public void Open()
        {
            _openedCommand.Execute();
        }
    }

    public interface ISwitchable
    {
        void PowerOn();
        void PowerOff();
    }

    public class Light : ISwitchable
    {
        public void PowerOn()
        {
            Console.WriteLine("The light is on");
        }

        public void PowerOff()
        {
            Console.WriteLine("The light is off");
        }
    }

    public class CloseSwitchCommand : ICommand
    {
        private ISwitchable _switchable;

        public CloseSwitchCommand(ISwitchable switchable)
        {
            _switchable = switchable;
        }

        public void Execute()
        {
            _switchable.PowerOn();
        }
    }

    public class OpenSwitchCommand : ICommand
    {
        private ISwitchable _switchable;

        public OpenSwitchCommand(ISwitchable switchable)
        {
            _switchable = switchable;
        }

        public void Execute()
        {
            _switchable.PowerOff();
        }
    }
}