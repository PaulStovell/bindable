using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Bindable.Windows.Controls;

namespace Bindable.Windows.Controls
{
    /// <summary>
    /// A message list is simply an ItemsControl that has the ability to be closed and automatically reopened when the items change. It 
    /// makes no assumptions about the items it contains or how the IsOpen property (indicating whether it should be shown or hidden) is treated - 
    /// that is left up to the style.  
    /// </summary>
    public class MessageList : ItemsControl
    {
        public static readonly DependencyProperty AllowCloseProperty = DependencyProperty.Register("AllowClose", typeof(bool), typeof(MessageList), new FrameworkPropertyMetadata(true));
        private static readonly DependencyPropertyKey IsOpenPropertyKey = DependencyProperty.RegisterReadOnly("IsOpen", typeof(bool), typeof(MessageList), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsOpenProperty = IsOpenPropertyKey.DependencyProperty;
        public static readonly DependencyProperty ReOpenOnItemsSourceChangedProperty = DependencyProperty.Register("ReOpenOnItemsSourceChanged", typeof(bool), typeof(MessageList), new FrameworkPropertyMetadata(true));
        private bool _closedExplicitly;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageList"/> class.
        /// </summary>
        public MessageList()
        {
            CommandBindings.Add(new CommandBinding(MessageListCommands.Close, CloseCommandExecuted, CloseCommandCanExecute));
            CommandBindings.Add(new CommandBinding(MessageListCommands.Focus, FocusCommandExecuted));
        }

        #region Dependency properties

        /// <summary>
        /// Gets or sets a value indicating whether or not the user is allowed to close this message list.
        /// </summary>
        public bool AllowClose
        {
            get { return (bool)GetValue(AllowCloseProperty); }
            set { SetValue(AllowCloseProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the messages should be displayed.
        /// </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            private set { SetValue(IsOpenPropertyKey, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the IsOpen property should be changed (even if the user closed the panel manually) 
        /// whenever the messages change (except removal). The default is true. 
        /// </summary>
        public bool ReOpenOnItemsSourceChanged
        {
            get { return (bool)GetValue(ReOpenOnItemsSourceChangedProperty); }
            set { SetValue(ReOpenOnItemsSourceChangedProperty, value); }
        }

        /// <summary>
        /// Called when the <see cref="P:System.Windows.Controls.ItemsControl.ItemsSource"/> property changes.
        /// </summary>
        /// <param name="oldValue">Old value of the <see cref="P:System.Windows.Controls.ItemsControl.ItemsSource"/> property.</param>
        /// <param name="newValue">New value of the <see cref="P:System.Windows.Controls.ItemsControl.ItemsSource"/> property.</param>
        protected override void OnItemsSourceChanged(IEnumerable oldValue, System.Collections.IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            IsOpen = HasItems;
        }

        #endregion

        /// <summary>
        /// Invoked when the <see cref="P:System.Windows.Controls.ItemsControl.Items"/> property changes.
        /// </summary>
        /// <param name="e">Information about the change.</param>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                    if ((ReOpenOnItemsSourceChanged || !_closedExplicitly) && IsOpen == false && HasItems)
                    {
                        IsOpen = true;
                    }
                    break;
                default:
                    if (!HasItems)
                    {
                        IsOpen = false;
                    }
                    break;
            }
        }

        /// <summary>
        /// Occurs when an element on the style invokes the PanelCommands.Close command.
        /// </summary>
        private void CloseCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (AllowClose)
            {
                _closedExplicitly = true;
                IsOpen = false;
            }
        }

        /// <summary>
        /// Checks whether the Close command is allowed to be executed.
        /// </summary>
        private void CloseCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = AllowClose;
        }

        /// <summary>
        /// Occurs when a child element invokes the PanelCommands.Focus command.
        /// </summary>
        private void FocusCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var element = e.Parameter as FrameworkElement;
            if (element != null)
            {
                element.Focus();
            }
        }
    }
}