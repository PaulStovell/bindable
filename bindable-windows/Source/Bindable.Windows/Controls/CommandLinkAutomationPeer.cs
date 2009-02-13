using System.Windows.Automation.Peers;

namespace Bindable.Windows.Controls
{
    /// <summary>
    /// A custom automation peer for CommandLinks that reads out the title of the command as well as the description.
    /// </summary>
    public class CommandLinkAutomationPeer : ButtonAutomationPeer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLinkAutomationPeer"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public CommandLinkAutomationPeer(CommandLink owner) : base(owner)
        {
        }

        /// <summary>
        /// Gets the name of the class of the element associated with this <see cref="T:System.Windows.Automation.Peers.ButtonBaseAutomationPeer"/>. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetName"/>.
        /// </summary>
        /// <returns>
        /// A string that contains the class name, minus the accelerator key.
        /// </returns>
        protected override string GetNameCore()
        {
            return ((CommandLink) Owner).CommandText + " " + base.GetNameCore();
        }
    }
}
