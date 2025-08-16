using System;
using System.Buffers;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;

namespace TestProject.Core.Services
{
    public class MqttLoraService : IDisposable
    {
        private readonly IMqttClient _client;
        private readonly MqttClientOptions _options;

        public event Action<string, string>? OnUplinkReceived;

        public MqttLoraService(string brokerHost = "localhost", int brokerPort = 1883)
        {
            // MQTTnet v5: create client via factory
            _client = new MqttClientFactory().CreateMqttClient();

            _options = new MqttClientOptionsBuilder()
                .WithTcpServer(brokerHost, brokerPort)
                .Build();

            _client.ConnectedAsync += async _ =>
            {
                var subscribeOptions = new MqttClientSubscribeOptionsBuilder()
                    .WithTopicFilter("application/+/device/+/event/up")
                    .Build();

                await _client.SubscribeAsync(subscribeOptions);
            };

            _client.ApplicationMessageReceivedAsync += args =>
            {
                var topic = args.ApplicationMessage.Topic ?? string.Empty;

                // ✅ Convert ReadOnlySequence<byte> to byte[]
                var seq = args.ApplicationMessage.Payload;
                string payload;
                if (seq.Length == 0)
                {
                    payload = string.Empty;
                }
                else
                {
                    var buffer = seq.ToArray(); // Extension method works if you add System.Buffers
                    payload = Encoding.UTF8.GetString(buffer);
                }

                OnUplinkReceived?.Invoke(topic, payload);
                return Task.CompletedTask;
            };
        }

        public async Task StartAsync()
        {
            if (!_client.IsConnected)
            {
                await _client.ConnectAsync(_options);
            }
        }

        public async Task StopAsync()
        {
            if (_client.IsConnected)
                await _client.DisconnectAsync();
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }

    // ✅ Helper extension for ReadOnlySequence<byte> → byte[]
    public static class ReadOnlySequenceExtensions
    {
        public static byte[] ToArray(this ReadOnlySequence<byte> sequence)
        {
            if (sequence.IsSingleSegment)
            {
                return sequence.FirstSpan.ToArray();
            }

            return sequence.ToArray(); // Uses System.Buffers.SequenceReader
        }
    }
}
