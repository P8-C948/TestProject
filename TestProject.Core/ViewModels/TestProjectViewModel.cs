using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestProject.Core;

public class TestProjectModel : INotifyPropertyChanged, IDisposable
{
    private string _title = "IoT Planner";
    private string _status = "Ready";
    private string _lastMessage = "No messages yet...";
    private string _host = "mqtt.myserver.com";
    private int _port = 1883;
    private bool _isConnected = false;

    public TestProjectModel()
    {
        // Initialize commands
        ConnectCommand = new RelayCommand(async () => await ConnectAsync(), () => !IsConnected);
        DisconnectCommand = new RelayCommand(async () => await DisconnectAsync(), () => IsConnected);
        PublishHelloCommand = new RelayCommand(async () => await PublishAsync("house/kitchen/status", "Hello"), () => IsConnected);
    }

    public ICommand ConnectCommand { get; }
    public ICommand DisconnectCommand { get; }
    public ICommand PublishHelloCommand { get; }

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged();
        }
    }

    public string Status
    {
        get => _status;
        set
        {
            _status = value;
            OnPropertyChanged();
        }
    }

    public string LastMessage
    {
        get => _lastMessage;
        set
        {
            _lastMessage = value;
            OnPropertyChanged();
        }
    }

    public string Host
    {
        get => _host;
        set
        {
            _host = value;
            OnPropertyChanged();
        }
    }

    public int Port
    {
        get => _port;
        set
        {
            _port = value;
            OnPropertyChanged();
        }
    }

    public bool IsConnected
    {
        get => _isConnected;
        private set
        {
            _isConnected = value;
            OnPropertyChanged();
            ((RelayCommand)ConnectCommand).RaiseCanExecuteChanged();
            ((RelayCommand)DisconnectCommand).RaiseCanExecuteChanged();
            ((RelayCommand)PublishHelloCommand).RaiseCanExecuteChanged();
        }
    }

    public async Task ConnectAsync()
    {
        try
        {
            Status = "Connecting...";
            await Task.Delay(1000); // Simulate connection

            IsConnected = true;
            Status = "Connected (simulated)";
            LastMessage = $"Connected to {Host}:{Port}";
        }
        catch (Exception ex)
        {
            Status = $"Connection failed: {ex.Message}";
        }
    }

    public async Task DisconnectAsync()
    {
        try
        {
            Status = "Disconnecting...";
            await Task.Delay(500);

            IsConnected = false;
            Status = "Disconnected";
        }
        catch (Exception ex)
        {
            Status = $"Disconnect error: {ex.Message}";
        }
    }

    public async Task PublishAsync(string topic, string payload)
    {
        try
        {
            if (!IsConnected)
            {
                Status = "Not connected to broker";
                return;
            }

            Status = "Publishing...";
            await Task.Delay(200);

            Status = $"Published to {topic}";
            LastMessage = $"{DateTime.Now:HH:mm:ss} - Published: {topic} = {payload}";
        }
        catch (Exception ex)
        {
            Status = $"Publish failed: {ex.Message}";
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void Dispose()
    {
        // Cleanup if needed
    }
}

public class RelayCommand : ICommand
{
    private readonly Func<Task> _executeAsync;
    private readonly Func<bool>? _canExecute;

    public RelayCommand(Func<Task> executeAsync, Func<bool>? canExecute = null)
    {
        _executeAsync = executeAsync;
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

    public async void Execute(object? parameter) => await _executeAsync();

    public event EventHandler? CanExecuteChanged;

    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);


}