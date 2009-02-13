using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Automation.Peers;

namespace Bindable.Windows.Controls
{
    /// <summary>
    /// Represents a command link.
    /// </summary>
    public class CommandLink : Button
    {
        public static readonly DependencyProperty CommandTextProperty = DependencyProperty.Register("CommandText", typeof(string), typeof(CommandLink), new UIPropertyMetadata(null));
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(CommandLink), new UIPropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLink"/> class.
        /// </summary>
        public CommandLink()
        {
        }

        /// <summary>
        /// Creates an appropriate <see cref="T:System.Windows.Automation.Peers.ButtonAutomationPeer"/> for this control as part of the WPF infrastructure.
        /// </summary>
        /// <returns>A custom automation peer for Command Links.</returns>
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new CommandLinkAutomationPeer(this);
        }

        /// <summary>
        /// Gets or sets the command text.
        /// </summary>
        [Category("Appearance")]
        public string CommandText
        {
            get { return (string)GetValue(CommandTextProperty); }
            set { SetValue(CommandTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        [Category("Appearance")]
        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
    }
}
