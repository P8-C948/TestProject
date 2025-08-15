using Avalonia.Controls;
using Avalonia.Interactivity;
using TestProject.Core;
using System.Threading.Tasks;

namespace TestProject.Avalonia;

public partial class MainWindow : Window
{
    public TestProjectModel ViewModel { get; }
    
    public MainWindow(TestProjectModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        DataContext = ViewModel;
        
        // Auto-connect on window load
        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        // Now we can use the ViewModel
        ViewModel.Host = "mqtt.myserver.com";
        ViewModel.Port = 1883;
        
        await ViewModel.ConnectAsync();
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        // Ensure proper cleanup when window is closing
        _ = Task.Run(async () => await ViewModel.DisconnectAsync());
        base.OnClosing(e);
    }
}