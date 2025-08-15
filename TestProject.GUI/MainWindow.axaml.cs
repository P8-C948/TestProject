using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using TestProject.Core;
using System.Threading.Tasks;

namespace TestProject.GUI
{
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

        // Ensure XAML is loaded even if the generated InitializeComponent is missing
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void MainWindow_Loaded(object? sender, RoutedEventArgs e)
        {
            // Set the broker details and connect
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
}