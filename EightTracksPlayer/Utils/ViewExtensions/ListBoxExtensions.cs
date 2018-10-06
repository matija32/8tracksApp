using System;
using System.Windows;
using System.Windows.Controls;

namespace EightTracksPlayer.Utils.ViewExtensions
{
    public static class ListBoxExtensions
    {
        public static readonly DependencyProperty FocusedItemProperty =
            DependencyProperty.RegisterAttached("FocusedItem", typeof (int), typeof (ListBoxExtensions),
                                                new PropertyMetadata(-1, OnFocusedItemChanged));

        private static void OnFocusedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListBox listBox = d as ListBox;
            int i = 0;
            foreach (var item in listBox.ItemsSource)
            {
                if (i++ == (int) e.NewValue)
                {
                    listBox.ScrollIntoView(item);
                    break;
                }
            }
        }


        public static int GetFocusedItem(DependencyObject dp)
        {
            return (int) dp.GetValue(FocusedItemProperty);
        }

        public static void SetFocusedItem(DependencyObject dependencyObject, int value)
        {
            dependencyObject.SetValue(FocusedItemProperty, value);
        }
    }
}