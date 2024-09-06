using System;
using System.Net.Sockets;
using System.Text;

public class ChatClient{
    
    private TcpClient cliente;
    private NetworkStream stream;

    public ChatClient(string ip, int port){
        cliente = new TcpClient(ip, port);
        stream = cliente.GetStream();
    }

    public void CreateRoom(string roomName){
        SendMessage($"CREATE::{roomName}");
    }

    public void JoinRoom(string roomName){
        SendMessage($"JOIN::{roomName}");
    }

    public void SendMessageToRoom(string roomName, string message){
        SendMessage($"MESSAGE::{roomName}::{message}");
    }

    public void SendMessage(string message){
        byte[] data = Encoding.ASCII.GetBytes(message);
        stream.Write(data, 0, data.Length);
    }

    public void Close(){
        stream.Close();
        cliente.Close();
    }
}