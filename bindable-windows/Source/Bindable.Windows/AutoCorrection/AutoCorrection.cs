using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bindable.Windows.AutoCorrection
{
    public class AutoCorrection
    {
        public static readonly DependencyProperty LastCorrectionProperty = DependencyProperty.RegisterAttached("LastCorrection", typeof(Correction), typeof(AutoCorrection), new UIPropertyMetadata(null, CorrectionPropertySet));
        public static readonly DependencyProperty CorrectorProperty = DependencyProperty.RegisterAttached("Corrector", typeof(IAutoCorrector), typeof(AutoCorrection), new UIPropertyMetadata(null, CorrectorPropertySet));
        public static readonly DependencyProperty SuppressCorrectionProperty = DependencyProperty.RegisterAttached("SuppressCorrection", typeof(bool), typeof(AutoCorrection), new UIPropertyMetadata(false));
        public static readonly DependencyProperty HasLastCorrectionProperty = DependencyProperty.RegisterAttached("HasLastCorrection", typeof(bool), typeof(AutoCorrection), new UIPropertyMetadata(null));
        public static readonly DependencyProperty CorrectionRequiredProperty = DependencyProperty.RegisterAttached("CorrectionRequired", typeof(bool), typeof(AutoCorrection), new UIPropertyMetadata(false));

        public AutoCorrection()
        {
            
        }

        public static Correction GetLastCorrection(DependencyObject obj)
        {
            return (Correction)obj.GetValue(LastCorrectionProperty);
        }

        public static void SetLastCorrection(DependencyObject obj, Correction value)
        {
            obj.SetValue(LastCorrectionProperty, value);
        }

        public static IAutoCorrector GetCorrector(DependencyObject obj)
        {
            return (IAutoCorrector)obj.GetValue(CorrectorProperty);
        }

        public static void SetCorrector(DependencyObject obj, IAutoCorrector value)
        {
            obj.SetValue(CorrectorProperty, value);
        }

        public static bool GetSuppressCorrection(DependencyObject obj)
        {
            return (bool)obj.GetValue(SuppressCorrectionProperty);
        }

        public static void SetSuppressCorrection(DependencyObject obj, bool value)
        {
            obj.SetValue(SuppressCorrectionProperty, value);
        }

        public static bool GetHasLastCorrection(DependencyObject obj)
        {
            return (bool)obj.GetValue(HasLastCorrectionProperty);
        }

        public static void SetHasLastCorrection(DependencyObject obj, bool value)
        {
            obj.SetValue(HasLastCorrectionProperty, value);
        }

        public static bool GetCorrectionRequired(DependencyObject obj)
        {
            return (bool)obj.GetValue(CorrectionRequiredProperty);
        }

        public static void SetCorrectionRequired(DependencyObject obj, bool value)
        {
            obj.SetValue(CorrectionRequiredProperty, value);
        }

        private static void CorrectorPropertySet(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var textBox = target as TextBox;
            if (textBox == null) throw new ArgumentException("The Corrector attached dependency property can only be used on TextBoxes");

            if (e.NewValue != null)
            {
                textBox.LostFocus += TextBox_LostFocus;
                textBox.TextChanged += new TextChangedEventHandler(TextBox_TextChanged);
                textBox.CommandBindings.Add(new CommandBinding(AutoCorrectionCommands.UndoLastCorrection, UndoLastCorrection_Executed));
            }
        }

        static void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            if (GetSuppressCorrection(textBox) == false && textBox.IsFocused)
            {
                SetCorrectionRequired(textBox, true);
            }
        }

        private static void UndoLastCorrection_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            var correction = GetLastCorrection(textBox);
            if (correction != null)
            {
                SetSuppressCorrection(textBox, true);
                textBox.Text = (string)correction.OriginalValue;
                SetSuppressCorrection(textBox, false);
                SetLastCorrection(textBox, null);
            }
        }

        private static void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = e.Source as TextBox;
            if (textBox == null) return;

            if (GetSuppressCorrection(textBox) == false && GetCorrectionRequired(textBox))
            {
                var corrector = GetCorrector(textBox);
                if (corrector != null)
                {
                    var correction = corrector.Correct(textBox.Text, typeof (string), CultureInfo.CurrentUICulture);
                    if (correction != null && correction != Correction.DoNothing)
                    {
                        SetLastCorrection(textBox, correction);
                    }
                    else
                    {
                        SetLastCorrection(textBox, null);
                    }
                }
            }
        }

        private static void CorrectionPropertySet(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var textBox = target as TextBox;
            if (textBox == null) return;

            var correction = e.NewValue as Correction;
            if (correction != null)
            {
                SetSuppressCorrection(textBox, true);
                textBox.Text = (string) correction.CorrectedValue;
                SetSuppressCorrection(textBox, false);
            }
            SetHasLastCorrection(textBox, correction != null);
            SetCorrectionRequired(textBox, false);
        }
    }
}