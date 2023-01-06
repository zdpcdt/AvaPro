using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AvaPro;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        button.Content = "Hello, Avalonia!";
    }
}