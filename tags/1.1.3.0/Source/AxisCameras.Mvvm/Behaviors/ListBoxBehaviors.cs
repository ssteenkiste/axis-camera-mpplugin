#region Copyright (C) 2005-2015 Team MediaPortal

// Copyright (C) 2005-2015 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

#endregion

using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace AxisCameras.Mvvm.Behaviors
{
    /// <summary>
    /// Attached behavior giving ListBox some expanded features.
    /// </summary>
    public static class ListBoxBehaviors
    {
        #region SelectedItems attached dependency property

        /// <summary>
        /// Describes the selected items in the ListBox. This attached property is used when e.g. a
        /// ViewModel wishes to bind the selected items in a ListBox (ListView inherit from ListBox) to
        /// a property on the ViewModel.
        /// It was designed when virtualization flaws were found during normal binding from a
        /// ListViewItems' IsSelected property to the corresponding property on a ViewModel. Note that
        /// this binding goes one way, changes from the View are propagated to the ViewModel, however
        /// changes on the ViewModel are not propagated to the View.
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached(
                "SelectedItems",
                typeof(ObservableCollection<object>),
                typeof(ListBoxBehaviors),
                new PropertyMetadata(OnSelectedItemsChanged));

        /// <summary>
        /// Gets the selected items.
        /// </summary>
        /// <param name="target">The target ListBox.</param>
        public static ObservableCollection<object> GetSelectedItems(DependencyObject target)
        {
            return (ObservableCollection<object>)target.GetValue(SelectedItemsProperty);
        }

        /// <summary>
        /// Sets the selected items.
        /// </summary>
        /// <param name="target">The target ListBox.</param>
        /// <param name="value">The value to set.</param>
        public static void SetSelectedItems(
            DependencyObject target,
            ObservableCollection<object> value)
        {
            target.SetValue(SelectedItemsProperty, value);
        }

        /// <summary>
        /// Called when property SelectedItems has changed.
        /// </summary>
        /// <param name="target">The target ListBox.</param>
        /// <param name="e">The event args.</param>
        private static void OnSelectedItemsChanged(
            DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            var listBox = target as ListBox;
            if (listBox == null)
            {
                return;
            }

            if (e.NewValue != null)
            {
                Add(listBox, listBox.SelectedItems);
                listBox.SelectionChanged += ListBox_SelectionChanged;
            }
            else
            {
                listBox.SelectionChanged -= ListBox_SelectionChanged;
            }
        }

        /// <summary>
        /// Handles the SelectionChanged event of the ListBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event args.</param>
        private static void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var target = sender as DependencyObject;
            if (target == null)
            {
                return;
            }

            // The default value for properties e.AddedItems and e.RemovedItems is an empty array, thus
            // there is no need to check for null
            Add(target, e.AddedItems);
            Remove(target, e.RemovedItems);
        }

        #endregion

        /// <summary>
        /// Add items to the list of selected items.
        /// </summary>
        /// <param name="target">The object with the attached property.</param>
        /// <param name="addedItems">The items to add.</param>
        private static void Add(DependencyObject target, IList addedItems)
        {
            ObservableCollection<object> selectedItems = GetSelectedItems(target);
            if (selectedItems != null)
            {
                foreach (object addedItem in addedItems)
                {
                    selectedItems.Add(addedItem);
                }
            }
        }

        /// <summary>
        /// Remove items from the list of selected items.
        /// </summary>
        /// <param name="target">The object with the attached property.</param>
        /// <param name="removedItems">The items to remove.</param>
        private static void Remove(DependencyObject target, IList removedItems)
        {
            ObservableCollection<object> selectedItems = GetSelectedItems(target);
            if (selectedItems != null)
            {
                foreach (object removedItem in removedItems)
                {
                    selectedItems.Remove(removedItem);
                }
            }
        }
    }
}