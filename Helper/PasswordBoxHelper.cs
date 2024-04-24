using System.Windows;
using System.Windows.Controls;


    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty BoundPasswordProperty =
        DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordBoxHelper), new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static string GetBoundPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(BoundPasswordProperty);
        }

        public static void SetBoundPassword(DependencyObject obj, string value)
        {
            obj.SetValue(BoundPasswordProperty, value);
        }

        private static void OnBoundPasswordChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = obj as PasswordBox;
            if (passwordBox == null)
                return;

            // Unregister the event to prevent recursive updating
            passwordBox.PasswordChanged -= PasswordChanged;

            // Update the Password property of the PasswordBox
            passwordBox.Password = e.NewValue?.ToString();

            // Register the event again
            passwordBox.PasswordChanged += PasswordChanged;
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox == null)
                return;

            // Update the BoundPassword attached property
            SetBoundPassword(passwordBox, passwordBox.Password);
        }
    }

