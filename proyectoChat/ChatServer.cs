using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;

public class ChatServer
{
    private TcpListener server;
    private Dictionary<string, List<TcpClient>> chatRooms = new Dictionary<string, List<TcpClient>>();
    private List<TcpClient> clients = new List<TcpClient>();

    public ChatServer(string ip, int port)
    {
        server = new TcpListener(IPAddress.Parse(ip), port);
    }

    public void Start()
    {
        server.Start();
        Console.WriteLine("Server started...");

        while (true)
        {
            TcpClient client = server.AcceptTcpClient();
            clients.Add(client);
            Console.WriteLine("New client connected.");

            Thread clientThread = new Thread(() => HandleClient(client));
            clientThread.Start();
        }
    }

    private void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead;

        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Received: {message}");

            string[] splitMessage = message.Split("::");
            string command = splitMessage[0];
            string roomName = splitMessage[1];
            string chatMessage = splitMessage.Length > 2 ? splitMessage[2] : "";

            switch (command)
            {
                case "CREATE":
                    CreateRoom(roomName);
                    break;
                case "JOIN":
                    JoinRoom(roomName, client);
                    break;
                case "MESSAGE":
                    SendMessageToRoom(roomName, chatMessage, client);
                    break;
            }
        }

        client.Close();
    }

    private void CreateRoom(string roomName)
    {
        if (!chatRooms.ContainsKey(roomName))
        {
            chatRooms[roomName] = new List<TcpClient>();
            Console.WriteLine($"Room {roomName} created.");
        }
    }

    private void JoinRoom(string roomName, TcpClient client)
    {
        if (chatRooms.ContainsKey(roomName))
        {
            chatRooms[roomName].Add(client);
            Console.WriteLine($"Client joined room {roomName}.");
        }
    }

    private void SendMessageToRoom(string roomName, string message, TcpClient sender)
    {
        if (chatRooms.ContainsKey(roomName))
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            foreach (TcpClient client in chatRooms[roomName])
            {
                if (client != sender)
                {
                    client.GetStream().Write(messageBytes, 0, messageBytes.Length);
                }
            }
            Console.WriteLine($"Message sent to room {roomName}: {message}");
        }
    }
}
/*
namespace Servidor{

    class Servidor_Chat{
        private TcpListener server;
        private TcpClient client = new TcpClient();
        private IPEndPoint ipendpoint = new IPEndPoint(IPAddress.Any, 8000);
        private List<Connection> list = new List<Connection>();

        Connection con;

        private struct Connection{
            public NetworkStream stream;
            public StreamWriter streamw;
            public StreamReader streamr;
            public string nick;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public Servidor_Chat(){
            Inicio();
        }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public void Inicio(){
            Console.WriteLine("Servidor encontrado");
            server = new TcpListener(ipendpoint);
            server.Start();

            while(true){
                client = server.AcceptTcpClient();

                con.stream = client.GetStream();
                con.streamr = new StreamReader(con.stream);
                con.streamw = new StreamWriter(con.stream);

#pragma warning disable CS8601 // Possible null reference assignment.
                con.nick = con.streamr.ReadLine();
#pragma warning restore CS8601 // Possible null reference assignment.

                list.Add(con);
                Console.WriteLine(con.nick + " se a conectado ");

                Thread t = new Thread(Escuchar_conexion);
            }
        }

        void Escuchar_conexion(){
            Connection hcon = con;

            do{
                try{
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    string tmp = hcon.streamr.ReadLine();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    Console.WriteLine(hcon.nick + ": " + tmp);
                    foreach (Connection c in list){
                        try{
                            c.streamw.WriteLine(hcon.nick + ": " + tmp);
                            c.streamw.Flush();
                        } catch{

                        }
                    }
                } catch{
                    list.Remove(hcon);
                    Console.WriteLine(hcon.nick + " se a desconectado. ");
                    break;
                } 
            } while(true);
        }
    }

}
*/