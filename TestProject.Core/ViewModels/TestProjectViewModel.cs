using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestProject.Core;

public class TestProjectModel : INotifyPropertyChanged, IDisposable
{
    private string _title = "IoT Sensor Manager";
    private string _status = "Disconnected";
    private string _lastMessage = "No messages yet...";
    private string _host = "mqtt.myserver.com";
    private int _port = 1883;
    private bool _isConnected = false;
    private string _eventLog = "No events logged yet.\n";
    
    // Sensor states
    private bool _lightSensorValue = false;
    private bool _vibrationSensorValue = false;
    private bool _motionSensorValue = false;

    public TestProjectModel()
    {
        // Initialize commands
        ConnectCommand = new RelayCommand(async () => await ConnectAsync(), () => !IsConnected);
        DisconnectCommand = new RelayCommand(async () => await DisconnectAsync(), () => IsConnected);
        PublishHelloCommand = new RelayCommand(async () => await PublishAsync("house/kitchen/status", "Hello"), () => IsConnected);
        
        // Sensor toggle commands
        ToggleLightSensorCommand = new RelayCommand(() => ToggleSensor(ref _lightSensorValue, "Light"), () => IsConnected);
        ToggleVibrationSensorCommand = new RelayCommand(() => ToggleSensor(ref _vibrationSensorValue, "Vibration"), () => IsConnected);
        ToggleMotionSensorCommand = new RelayCommand(() => ToggleSensor(ref _motionSensorValue, "Motion"), () => IsConnected);
    }

    // Existing commands
    public ICommand ConnectCommand { get; }
    public ICommand DisconnectCommand { get; }
    public ICommand PublishHelloCommand { get; }
    
    // New sensor commands
    public ICommand ToggleLightSensorCommand { get; }
    public ICommand ToggleVibrationSensorCommand { get; }
    public ICommand ToggleMotionSensorCommand { get; }

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
    
    public string EventLog
    {
        get => _eventLog;
        set
        {
            _eventLog = value;
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
            ((RelayCommand)ToggleLightSensorCommand).RaiseCanExecuteChanged();
            ((RelayCommand)ToggleVibrationSensorCommand).RaiseCanExecuteChanged();
            ((RelayCommand)ToggleMotionSensorCommand).RaiseCanExecuteChanged();
        }
    }
    
    // Sensor properties
    public bool LightSensorValue 
    { 
        get => _lightSensorValue; 
        set 
        { 
            _lightSensorValue = value; 
            OnPropertyChanged();
            OnPropertyChanged(nameof(LightSensorText));
        } 
    }
    
    public bool VibrationSensorValue 
    { 
        get => _vibrationSensorValue; 
        set 
        { 
            _vibrationSensorValue = value; 
            OnPropertyChanged();
            OnPropertyChanged(nameof(VibrationSensorText));
        } 
    }
    
    public bool MotionSensorValue 
    { 
        get => _motionSensorValue; 
        set 
        { 
            _motionSensorValue = value; 
            OnPropertyChanged();
            OnPropertyChanged(nameof(MotionSensorText));
        } 
    }
    
    // Text display properties for the sensor values (0 or 1)
    public string LightSensorText => LightSensorValue ? "1" : "0";
    public string VibrationSensorText => VibrationSensorValue ? "1" : "0";
    public string MotionSensorText => MotionSensorValue ? "1" : "0";

    public async Task ConnectAsync()
    {
        try
        {
            Status = "Connecting...";
            await Task.Delay(1000); // Simulate connection

            IsConnected = true;
            Status = "Connected (simulated)";
            LogEvent($"Connected to {Host}:{Port}");
            
            // Initialize sensors with random values
            SimulateSensorValues();
        }
        catch (Exception ex)
        {
            Status = $"Connection failed: {ex.Message}";
            LogEvent($"ERROR: Connection failed - {ex.Message}");
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
            LogEvent("Disconnected from IoT hub");
        }
        catch (Exception ex)
        {
            Status = $"Disconnect error: {ex.Message}";
            LogEvent($"ERROR: Disconnect error - {ex.Message}");
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
            LogEvent($"Published: {topic} = {payload}");
        }
        catch (Exception ex)
        {
            Status = $"Publish failed: {ex.Message}";
            LogEvent($"ERROR: Publish failed - {ex.Message}");
        }
    }
    
    // Helper methods
    private void SimulateSensorValues()
    {
        var random = new Random();
        LightSensorValue = random.Next(2) == 1;
        VibrationSensorValue = random.Next(2) == 1;
        MotionSensorValue = random.Next(2) == 1;
        
        LogEvent($"Initial sensor values - Light: {LightSensorText} | Vibration: {VibrationSensorText} | Motion: {MotionSensorText}");
    }
    
    private void ToggleSensor(ref bool sensorValue, string sensorName)
    {
        sensorValue = !sensorValue;
        
        // Trigger property changed
        switch (sensorName)
        {
            case "Light":
                OnPropertyChanged(nameof(LightSensorValue));
                OnPropertyChanged(nameof(LightSensorText));
                break;
            case "Vibration":
                OnPropertyChanged(nameof(VibrationSensorValue));
                OnPropertyChanged(nameof(VibrationSensorText));
                break;
            case "Motion":
                OnPropertyChanged(nameof(MotionSensorValue));
                OnPropertyChanged(nameof(MotionSensorText));
                break;
        }
        
        LogEvent($"{sensorName} sensor changed to {(sensorValue ? "1 (5V)" : "0 (0V)")}");
    }
    
    private void LogEvent(string message)
    {
        EventLog = $"[{DateTime.Now:HH:mm:ss}] {message}\n{EventLog}";
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