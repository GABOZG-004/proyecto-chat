using System.Net.Sockets;

public class ChatRoom
{
    public required string Name { get; set; }
    public List<TcpClient> Clients { get; set; } = new List<TcpClient>();
}
