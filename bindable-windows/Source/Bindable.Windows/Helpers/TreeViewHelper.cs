
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Generic;

namespace Bindable.Windows.Helpers
{
    /// <summary>
    /// Contains helper classes for dealing with Tree Views and other hierarchical item controls.
    /// </summary>
    public static class TreeViewHelper
    {
        /// <summary>
        /// Expands the tree view item.
        /// </summary>
        /// <param name="owningItemGenerator">The owning item generator.</param>
        /// <param name="dataItem">The data item.</param>
        /// <returns></returns>
        public static TreeViewItem ExpandTreeViewItem(ItemContainerGenerator owningItemGenerator, object dataItem)
        {
            if (owningItemGenerator == null) return null;

            var treeViewItem = owningItemGenerator.ContainerFromItem(dataItem) as TreeViewItem;
            if (treeViewItem != null)
            {
                treeViewItem.IsExpanded = true;
            }
            return treeViewItem;
        }

        /// <summary>
        /// Selects the tree view item.
        /// </summary>
        /// <param name="owningItemGenerator">The owning item generator.</param>
        /// <param name="dataItem">The data item.</param>
        public static void SelectTreeViewItem(ItemContainerGenerator owningItemGenerator, object dataItem)
        {
            if (owningItemGenerator == null) return;

            var treeViewItem = owningItemGenerator.ContainerFromItem(dataItem) as TreeViewItem;
            if (treeViewItem != null)
            {
                treeViewItem.IsSelected = true;
            }
        }

        /// <summary>
        /// Expands each tree view item along the path to a node, and selects the final node.
        /// </summary>
        /// <param name="treeView">The tree view control containing the items to be traversed.</param>
        /// <param name="pathTokens">The individual tokens that make up the path.</param>
        /// <param name="childNodeDataItemSelector">A lambda that selects the child nodes of a given node.</param>
        /// <param name="matchNodePredicate">A lambda that indicates whether a token correctly matches a node data item.</param>
        /// <returns>The data item of the found node, or null if no match was found.</returns>
        public static object ExpandPathToNodeAndSelectBestMatch(ItemsControl treeView, string[] pathTokens, Func<object, IEnumerable> childNodeDataItemSelector, Func<object, string, int, bool> matchNodePredicate)
        {
            var foundNode = default(object);
            if (treeView != null)
            {
                var itemsSource = treeView.ItemsSource;
                if (itemsSource != null)
                {
                    foreach (var currentNodeDataItem in itemsSource)
                    {
                        foundNode = ExpandPathToNodeAndSelectBestMatchRecursively(treeView, treeView.ItemContainerGenerator, currentNodeDataItem, childNodeDataItemSelector, matchNodePredicate, pathTokens, 0);
                        if (foundNode != null)
                        {
                            break;
                        }
                    }
                }
            } 
            return foundNode;
        }

        private static object ExpandPathToNodeAndSelectBestMatchRecursively(UIElement treeView, ItemContainerGenerator owningItemGenerator, object currentNodeDataItem, Func<object, IEnumerable> childNodeDataItemSelector, Func<object, string, int, bool> matchPathToNode, string[] pathTokens, int tokenIndex)
        {
            var foundNode = default(object);
            var currentToken = pathTokens[tokenIndex];

            // Check whether this is the node we are looking for.
            if ((matchPathToNode(currentNodeDataItem, currentToken, tokenIndex)))
            {
                // Expand this item before looking at the children. The update layout call is needed because although the 
                // item has been expanded, none of the expanded items will have been created, and so calls to the 
                // treeViewItem.ItemGenerator will return null until the UI has had a chance to create the child items.
                var currentTreeViewItem = ExpandTreeViewItem(owningItemGenerator, currentNodeDataItem);
                if (treeView != null) 
                {
                    treeView.UpdateLayout();
                }

                if (tokenIndex < pathTokens.Length - 1)
                {
                    // Now see if we can continue to match any of the children.
                    var currentChildItemCenerator = currentTreeViewItem == null ? null : currentTreeViewItem.ItemContainerGenerator;
                    var childNodes = childNodeDataItemSelector(currentNodeDataItem);
                    if (childNodes != null)
                    {
                        foreach (var childNode in childNodes)
                        {
                            foundNode = ExpandPathToNodeAndSelectBestMatchRecursively(treeView, currentChildItemCenerator, childNode, childNodeDataItemSelector, matchPathToNode, pathTokens, tokenIndex + 1);
                            if (foundNode != null)
                            {
                                break;
                            }
                        }
                    }
                }

                // This node is a match, but none of the children are (or there are no children). Select this node instead.
                if (foundNode == null)
                {
                    foundNode = currentNodeDataItem;
                    SelectTreeViewItem(owningItemGenerator, currentNodeDataItem);
                }
            }
            return foundNode;
        }
    }
}
