namespace Servidor{
    class Program{
        static void Main(string[] args)
        {
            ChatClient client = new ChatClient("127.0.0.1", 5000);
            client.CreateRoom("General");
            client.JoinRoom("General");
            client.SendMessageToRoom("General", "Hello, World!");
            ChatServer server = new ChatServer("127.0.0.1", 5000);
            server.Start();

            Console.ReadLine();
            client.Close();
        }
    }
}