// Not recommended: Listen for gateway Semtech UDP packats (raw packet-forwarder)
// Only for testing:
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class SemtechUdpListener
{
    public async Task StartAsync(int port = 1700)
    {
        using var client = new UdpClient(port);
        while (true)
        {
            var result = await client.ReceiveAsync();
            var bytes = result.Buffer; // decode semtech prototcol (not trivial)
            Console.WriteLine($"Got {bytes.Length} bytes from {result.RemoteEndPoint}");
        }
    }
}