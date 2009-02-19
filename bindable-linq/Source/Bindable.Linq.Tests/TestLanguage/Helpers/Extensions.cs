using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Bindable.Linq.Tests.TestLanguage.Helpers
{
    internal static class Extensions
    {
        public static void AddRange<TElement>(this ObservableCollection<TElement> collection, params TElement[] itemsToAdd)
        {
            foreach (var itemToAdd in itemsToAdd)
                collection.Add(itemToAdd);
        }
        
        public static void MoveRange<TElement>(this ObservableCollection<TElement> collection, int index, params TElement[] itemsToMove)
        {
            foreach (var itemToMove in itemsToMove)
            {
                var oldIndex = collection.IndexOf(itemToMove);
                collection.Move(oldIndex, index);
                index++;
            }
        }

        public static void InsertRange<TElement>(this ObservableCollection<TElement> collection, int index, params TElement[] itemsToInsert)
        {
            foreach (var itemToInsert in itemsToInsert)
            {
                collection.Insert(index, itemToInsert);
                index++;
            }
        }

        public static void ReplaceRange<TElement>(this ObservableCollection<TElement> collection, TElement[] originals, TElement[] replacements)
        {
            var getItem = new Func<TElement[], int, TElement>( 
                (items, index) => index >= items.Length ? default(TElement) : items[index]
            );

            for (int i = 0; i < Math.Max(originals.Length, replacements.Length); i++)
            {
                var oldItem = getItem(originals, i);
                if (oldItem != null) collection.Remove(oldItem);
                var newItem = getItem(replacements, i);
                if (newItem != null) collection.Add(newItem);
            }
        }

        public static void RemoveRange<TElement>(this ObservableCollection<TElement> collection, params TElement[] itemsToRemove)
        {
            foreach (var itemToRemove in itemsToRemove)
                collection.Remove(itemToRemove);
        }


    }
}
