using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Mikunigaoka.Controls
{
    [TemplatePart(Name = "PART_NumberBox", Type = typeof(NumberBox))]
    [TemplatePart(Name = "PART_InfoBar", Type = typeof(InfoBar))]
    public sealed partial class ValidationNumberBox : Control
    {
        // Element fields

        private NumberBox? numberBox;
        private InfoBar? infoBar;
        private INotifyDataErrorInfo? oldDataContext;

        // Constructor and other functions.

        public ValidationNumberBox()
        {
            DefaultStyleKey = typeof(ValidationNumberBox);

            DataContextChanged += ValidationNumberBox_DataContextChanged;
        }

        private void ValidationNumberBox_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (oldDataContext is INotifyDataErrorInfo)
                oldDataContext.ErrorsChanged -= ErrorInfo_ErrorsChanged;

            if (args.NewValue is INotifyDataErrorInfo errorInfo1)
            {
                oldDataContext = errorInfo1;

                oldDataContext.ErrorsChanged += ErrorInfo_ErrorsChanged;
            }

            RefreshErrors();
        }

        private void ErrorInfo_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            RefreshErrors();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            numberBox = (NumberBox)GetTemplateChild("PART_NumberBox");
            infoBar = (InfoBar)GetTemplateChild("PART_InfoBar");

            numberBox.ValueChanged += NumberBox_ValueChanged;
        }

        private void NumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            Value = sender.Value;
        }

        private static void OnPropertyNamePropertyChanged(object? sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is not string { Length: > 0 } propertyName ||
                sender is null)
                return;

            ((ValidationNumberBox)sender).RefreshErrors();
        }

        private void RefreshErrors()
        {
            if (numberBox is not NumberBox box ||
                infoBar is not InfoBar bar ||
                PropertyName is not string propertyName ||
                DataContext is not INotifyDataErrorInfo dataErrorInfo)
                return;

            ValidationResult? result = dataErrorInfo.GetErrors(propertyName).OfType<ValidationResult>().FirstOrDefault();

            bar.Visibility = result is null ? Visibility.Collapsed : Visibility.Visible;

            if (result is not null && result.ErrorMessage is not null)
            {
                bar.Content = result.ErrorMessage;

                if (App.Current.Resources.TryGetValue("SystemFillColorCriticalBrush", out object brush))
                {
                    if (brush is Brush brush1)
                        numberBox.BorderBrush = brush1;
                }

                return;
            }

            if (App.Current.Resources.TryGetValue("TextControlElevationBorderFocusedBrush", out object resourceValue) && resourceValue is Brush brush2)
                box.BorderBrush = brush2;
        }

        // Template Properties

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(double),
            typeof(ValidationNumberBox),
            new(default(double)));

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        private static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header),
            typeof(string),
            typeof(ValidationNumberBox),
            new(default(string)));

        public string PropertyName
        {
            get => (string)GetValue(PropertyNameProperty);
            set => SetValue(PropertyNameProperty, value);
        }

        private static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(
            nameof(PropertyName),
            typeof(string),
            typeof(ValidationNumberBox),
            new(PropertyNameProperty, OnPropertyNamePropertyChanged));
    }
}
