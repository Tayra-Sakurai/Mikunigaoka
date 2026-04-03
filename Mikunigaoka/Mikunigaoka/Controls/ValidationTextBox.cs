using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Mikunigaoka.Controls;

[TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
[TemplatePart(Name = "PART_TextBlock", Type = typeof(TextBlock))]
public sealed partial class ValidationTextBox : Control
{
    private TextBox? textBox;
    private TextBlock? textBlock;
    private INotifyDataErrorInfo? oldDataContext;

    public ValidationTextBox()
    {
        DefaultStyleKey = typeof(ValidationTextBox);

        DataContextChanged += ValidationTextBox_DataContextChanged;
    }

    private void ValidationTextBox_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
    {
        if (oldDataContext is not null)
            oldDataContext.ErrorsChanged -= DataContext_ErrorsChanged;

        if (args.NewValue is INotifyDataErrorInfo notifyDataError)
        {
            oldDataContext = notifyDataError;

            oldDataContext.ErrorsChanged += DataContext_ErrorsChanged;
        }

        RefreshErrors();
    }

    private void DataContext_ErrorsChanged(object? sender, DataErrorsChangedEventArgs args)
    {
        RefreshErrors();
    }

    private void RefreshErrors()
    {
        if (textBlock is not TextBlock block ||
            textBox is not TextBox box ||
            DataContext is not INotifyDataErrorInfo dataErrorInfo ||
            PropertyName is not string propertyName)
            return;

        ValidationResult? result = dataErrorInfo.GetErrors(propertyName).OfType<ValidationResult>().FirstOrDefault();

        block.Visibility = result is not null ? Visibility.Visible : Visibility.Collapsed;

        if (result is not null)
        {
            block.Text = result.ErrorMessage ?? string.Empty;
            
            if (App.Current.Resources.TryGetValue("SystemFillColorCriticalBrush", out object resource))
            {
                Brush? brush = resource as Brush;
                if (brush is not null)
                    box.BorderBrush = brush;
            }

            return;
        }

        if (App.Current.Resources.TryGetValue("TextControlElevationBorderFocusedBrush", out object resourceValue) && resourceValue is Brush brush1)
            box.BorderBrush = brush1;
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        textBox = (TextBox)GetTemplateChild("PART_TextBox");
        textBlock = (TextBlock)GetTemplateChild("PART_TextBlock");

        textBox.TextChanged += TextBox_TextChanged;
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        Text = ((TextBox)sender).Text;
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    private static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(ValidationTextBox),
        new(default(string)));

    public string Header
    {
        get => (string)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    private static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
        nameof(Header),
        typeof(string),
        typeof(ValidationTextBox),
        new(default(string)));

    public string PlaceholderText
    {
        get => (string)GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    private static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(
        nameof(PlaceholderText),
        typeof(string),
        typeof(ValidationTextBox),
        new(default(string)));

    public string PropertyName
    {
        get => (string)GetValue(PropertyNameProperty);
        set => SetValue(PropertyNameProperty, value);
    }

    private static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register(
        nameof(PropertyName),
        typeof(string),
        typeof(ValidationTextBox),
        new(PropertyNameProperty, OnPropertyNamePropertyChanged));

    private static void OnPropertyNamePropertyChanged(object? sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is not string { Length: > 0 } propertyName ||
            sender is null)
            return;

        ((ValidationTextBox)sender).RefreshErrors();
    }

    public bool AcceptsReturn
    {
        get => (bool)GetValue(AcceptsReturnProperty);
        set => SetValue(AcceptsReturnProperty, value);
    }

    private static readonly DependencyProperty AcceptsReturnProperty = DependencyProperty.Register(
        nameof(AcceptsReturn),
        typeof(bool),
        typeof(ValidationTextBox),
        new(false));

    public new double MinHeight
    {
        get => (double)GetValue(MinHeightProperty);
        set => SetValue(MinHeightProperty, value);
    }

    private static readonly new DependencyProperty MinHeightProperty = DependencyProperty.Register(
        nameof(MinHeight),
        typeof(double),
        typeof(ValidationTextBox),
        new(default(double)));
}
