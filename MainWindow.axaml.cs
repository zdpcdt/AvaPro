using Avalonia.Controls;
using AvaPro.ViewModels;

namespace AvaPro;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}

