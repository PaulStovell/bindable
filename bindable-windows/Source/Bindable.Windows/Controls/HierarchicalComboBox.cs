using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Bindable.Windows.Helpers;

namespace Bindable.Windows.Controls
{
    /// <summary>
    /// Represents a hierarchical combo box. 
    /// </summary>
    [TemplatePart(Name="PART_TreeView", Type=typeof(TreeView))]
    public class HierarchicalComboBox : ComboBox
    {
        public static readonly DependencyProperty NodeTokenPathProperty = DependencyProperty.Register("NodeTokenPath", typeof(string), typeof(HierarchicalComboBox), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty SelectedPathProperty = DependencyProperty.Register("SelectedPath", typeof(string), typeof(HierarchicalComboBox), new UIPropertyMetadata(null, SelectedPathProperty_PropertyChanged));
        public static readonly DependencyProperty SelectedNodeProperty = DependencyProperty.Register("SelectedNode", typeof(object), typeof(HierarchicalComboBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectedNodeProperty_PropertyChanged));
        public static readonly DependencyProperty TreeViewStyleProperty = DependencyProperty.Register("TreeViewStyle", typeof(ControlTemplate), typeof(HierarchicalComboBox), new UIPropertyMetadata(null));
        public static readonly DependencyProperty AlwaysShowDropDownProperty = DependencyProperty.Register("AlwaysShowDropDown", typeof(bool), typeof(HierarchicalComboBox), new UIPropertyMetadata(false));
        public static readonly DependencyPropertyKey IsSelectedPathValidPropertyKey = DependencyProperty.RegisterReadOnly("IsSelectedPathValid", typeof(bool), typeof(HierarchicalComboBox), new UIPropertyMetadata(false));
        private static readonly DependencyProperty IsSelectedPathValidProperty = IsSelectedPathValidPropertyKey.DependencyProperty;
        private TreeView _childTreeView;
        private TextBox _childTextBox;

        /// <summary>
        /// Initializes the <see cref="HierarchicalComboBox"/> class.
        /// </summary>
        static HierarchicalComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HierarchicalComboBox), new FrameworkPropertyMetadata(typeof(HierarchicalComboBox)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchicalComboBox"/> class.
        /// </summary>
        public HierarchicalComboBox()
        {
        }

        /// <summary>
        /// Gets or sets the TextBox which is supplied as part of the WPF control template applied to the control.
        /// </summary>
        protected TextBox ChildTextBox
        {
            get { return _childTextBox; }
            set
            {
                if (_childTextBox != null)
                {
                    _childTextBox.GotFocus -= ChildTextBox_GotFocus;
                    _childTextBox.LostFocus -= ChildTextBox_LostFocus;
                }
                _childTextBox = value;
                if (_childTreeView != null)
                {
                    _childTextBox.GotFocus += ChildTextBox_GotFocus;
                    _childTextBox.LostFocus += ChildTextBox_LostFocus;
                }
            }
        }

        /// <summary>
        /// Gets or sets the TreeView which is supplied as part of the WPF control template applied to the control.
        /// </summary>
        protected TreeView ChildTreeView
        {
            get { return _childTreeView; }
            private set
            {
                if (_childTreeView != null)
                {
                    _childTreeView.SelectedItemChanged -= ChildTreeView_SelectedItemChanged;
                    _childTreeView.Loaded -= ChildTreeView_Loaded;
                }
                _childTreeView = value;
                if (_childTreeView != null)
                {
                    _childTreeView.SelectedItemChanged += ChildTreeView_SelectedItemChanged;
                    _childTreeView.Loaded += ChildTreeView_Loaded;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the drop down should always be shown when the combo box has focus.
        /// </summary>
        public bool AlwaysShowDropDown
        {
            get { return (bool)GetValue(AlwaysShowDropDownProperty); }
            set { SetValue(AlwaysShowDropDownProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style that will be applied to the tree view hosted within the popup.
        /// </summary>
        public ControlTemplate TreeViewStyle
        {
            get { return (ControlTemplate)GetValue(TreeViewStyleProperty); }
            set { SetValue(TreeViewStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected node.
        /// </summary>
        public object SelectedNode
        {
            get { return (object)GetValue(SelectedNodeProperty); }
            set { SetValue(SelectedNodeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the path to the selected node. This path is also shown within the text box.
        /// </summary>
        public string SelectedPath
        {
            get { return (string)GetValue(SelectedPathProperty); }
            set { SetValue(SelectedPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets the node token path.
        /// </summary>
        /// <value>The node token path.</value>
        public string NodeTokenPath
        {
            get { return (string)GetValue(NodeTokenPathProperty); }
            set { SetValue(NodeTokenPathProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether the selected path is valid.
        /// </summary>
        public bool IsSelectedPathValid
        {
            get { return (bool)GetValue(IsSelectedPathValidProperty); }
            private set { SetValue(IsSelectedPathValidPropertyKey, value); }
        }

        /// <summary>
        /// When overridden in a derived class, provides the string path from a given data node. This path is assigned to the SelectedPath
        /// property when the SelectedNode changes.
        /// </summary>
        protected virtual string GetPathToNode(object dataNode)
        {
            return (BindingHelper.EvaluateBindingPath(SelectedValuePath, dataNode) ?? string.Empty).ToString();
        }

        /// <summary>
        /// When overridden in a derived class, allows the class to map the path entered by the user (in the TextBox) to a node. 
        /// Derived classes should make use of the ExpandPathToNodeAndSelectBestMatch method to make selection easier.
        /// </summary>
        /// <param name="enteredPath">The SelectedPath entered by the user.</param>
        protected virtual void SelectAndExpandNodeFromPath(string enteredPath)
        {
            var tokens = enteredPath.Split('\\', '/');
            tokens = tokens.Select(token => token.Trim('\\', '/')).ToArray();

            var dataTemplate = ItemTemplate as HierarchicalDataTemplate;
            if (dataTemplate != null)
            {
                var itemSourceBinding = dataTemplate.ItemsSource;
                if (itemSourceBinding != null)
                {
                    TreeViewHelper.ExpandPathToNodeAndSelectBestMatch(
                        ChildTreeView,
                        tokens,
                        node => BindingHelper.EvaluateBinding(itemSourceBinding, node) as IEnumerable,
                        delegate(object node, string token, int tokenIndex)
                            {
                                var nodeName = (BindingHelper.EvaluateBindingPath(NodeTokenPath, node) ?? string.Empty).ToString();
                                if (tokenIndex < tokens.Length - 1)
                                {
                                    return string.Compare(token, nodeName, StringComparison.CurrentCultureIgnoreCase) == 0;
                                }else
                                {
                                    return nodeName.StartsWith(token, StringComparison.CurrentCultureIgnoreCase);
                                }
                            }
                        );        
                }
            }

            
        }

        /// <summary>
        /// Occurs when a WPF control template is applied to this control.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ChildTreeView = GetTemplateChild("PART_TreeView") as TreeView;
            ChildTextBox = GetTemplateChild("PART_Text") as TextBox;
        }

        /// <summary>
        /// Occurs when the selected node path is changed. This can happen when the user manually types in the text box, or if the path is
        /// set elsewhere.
        /// </summary>
        private static void SelectedPathProperty_PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var comboBox = sender as HierarchicalComboBox;
            if (comboBox == null) return;

            comboBox.ValidateSelectedPath();
            if (!comboBox.IsSelectedPathValid) 
            {
                comboBox.SelectAndExpandNodeFromPath(comboBox.SelectedPath);
            }
        }

        /// <summary>
        /// Handles the SelectedItemChanged event of the ChildTreeView control. We use this to synchronize the SelectedPath and SelectedNode 
        /// properties when an item in the tree view is clicked.
        /// </summary>
        private void ChildTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var treeView = (TreeView)sender;
            SelectedNode = treeView.SelectedItem;
        }

        /// <summary>
        /// Occurs when the selected node is changed. We use this to ensure the IsSelectedPathValid property is correct.
        /// </summary>
        private static void SelectedNodeProperty_PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var comboBox = sender as HierarchicalComboBox;
            if (comboBox == null) return;
            
            if (comboBox.ChildTextBox == null || (comboBox.ChildTextBox != null && !comboBox.ChildTextBox.IsFocused))
            {
                // Since the TextBox may be bound to the SelectedPath, don't push the value if it has focus. The TextBox won't update
                // if the user still has focus, which means our SelectedPath and the TextBox's Text property will be out of sync.
                comboBox.SelectedPath = comboBox.GetPathToNode(comboBox.SelectedNode);
            }
            comboBox.ValidateSelectedPath();
        }

        /// <summary>
        /// Occurs when the ChildTextBox control is selected. We use this to automatically open the drop down if required.
        /// </summary>
        private void ChildTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (AlwaysShowDropDown) IsDropDownOpen = true;
        }

        /// <summary>
        /// Occurs when the ChildTextBox control loses focus. We use this to correct any paths the user has entered manually. 
        /// </summary>
        private void ChildTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Correct the path
            SelectedPath = GetPathToNode(SelectedNode);

            // Since the binding on the TextBox should be set to UpdateSourceTrigger=PropertyChanged, WPF's optimisations will stop the 
            // text from being re-read when the user tabs away from the control, even though we have set the SelectedPath and it is bound
            // to it. The code below will find any bindings on the TextBox in the template and force them to re-read and discard what the 
            // use typed (assuming it is wrong). 
            if (ChildTextBox != null)
            {
                var binding = BindingOperations.GetBindingExpression(ChildTextBox, TextBox.TextProperty);
                if (binding != null)
                {
                    binding.UpdateTarget();
                }
            }
        }

        /// <summary>
        /// Occurs when the ItemsSource property changes. We use this to ensure that the SelectedPath is in sync with the SelectedNode. 
        /// This is required because data binding to the SelectedNode or ItemsSource could occur in either order; if the ItemsSource \
        /// is empty when the SelectedNode is assigned, we can't resolve the selected path. 
        /// </summary>
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            SelectedPath = GetPathToNode(SelectedNode);
        }

        /// <summary>
        /// Occurs when the child TreeView is loaded. This is required because when the ComboBox is loaded, the TreeView control will be instantiated,
        /// but the TreeViewItems used to show each item won't. This means that although the SelectedNode has been selected, the TreeView 
        /// won't reflect this selection. The Loaded event occurs when the TreeView is added to the control tree (i.e., shown), so we 
        /// use it to ensure that the SelectedNode is in sync. with the SelectedPath and that the items are actually selected and expanded.  
        /// </summary>
        private void ChildTreeView_Loaded(object sender, RoutedEventArgs e)
        {
            SelectAndExpandNodeFromPath(SelectedPath);
        }

        private void ValidateSelectedPath()
        {
            if (SelectedPath == null && SelectedNode == null)
            {
                IsSelectedPathValid = true;
            }
            else if (SelectedNode != null)
            {
                IsSelectedPathValid = SelectedPath == GetPathToNode(SelectedNode);
            }
            else
            {
                IsSelectedPathValid = false;
            }
        }
    }
}