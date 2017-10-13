using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SoundBoard
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Input.ICommand" />
    /// Edit XML Comment Template for CommandBase
    public class CommandBase : ICommand
    {
        private Action<object> _action;

        public CommandBase(EventHandler executed, Action<object> action)
        {
            this.IsExecutionPossible = true;
            this.Executed = executed;
            _action = action;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBase"/> class.
        /// </summary>
        /// <param name="executed">The executed.</param>
        /// Edit XML Comment Template for #ctor
        public CommandBase(EventHandler executed)
        {
            this.IsExecutionPossible = true;
            this.Executed = executed;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBase"/> class.
        /// </summary>
        public CommandBase()
        {
        }

        /// <summary>Private member of the 'IsExecutionPossible' property</summary>
        private bool myIsExecutionPossible;

        /// <summary>Get or set the IsExecutionPossible</summary>
        /// <value>
        /// <c>true</c> if this instance is execution possible; otherwise, <c>false</c>.
        public bool IsExecutionPossible
        {
            get
            {
                return myIsExecutionPossible;
            }

            set
            {
                if (myIsExecutionPossible == value)
                {
                    return;
                }

                myIsExecutionPossible = value;

                if (null != IsExecutionPossibleChanged)
                {
                    IsExecutionPossibleChanged(this, new EventArgs());
                }

                if (null != CanExecuteChanged)
                {
                    CanExecuteChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>Implementation of the interface members.</summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            return myIsExecutionPossible;
        }

        /// <summary>Implementation of the interface members.</summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public virtual void Execute(object parameter)
        {
            if (null != Executed)
            {
                Executed(this, new EventArgs());
            }
        }

        /// <summary>Implementation of the interface members.</summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>Implementation of the interface members.</summary>
        public event EventHandler Executed;

        /// <summary>
        /// Public event of the 'IsExecutionPossible' property, which signalizes that the property has changed.
        /// </summary>
        public event EventHandler IsExecutionPossibleChanged;
    }
}
