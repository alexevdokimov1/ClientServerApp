using System.Net;
using System.Net.Sockets;

class Server
{
    private static int port = 13000;
    private static string? ip;
    public static void Main()
    {

        string host = Dns.GetHostName();
        IPHostEntry ipHost = Dns.GetHostEntry(host);
        foreach (IPAddress curAdd in ipHost.AddressList)
        {
            if (curAdd.AddressFamily.ToString() == ProtocolFamily.InterNetwork.ToString())
            {
                ip = curAdd.ToString();
                Console.WriteLine("Сеть: " + curAdd.AddressFamily.ToString());
                Console.WriteLine("Адрес: " + ip);
            }
        }

        TcpListener? server = null;
        bool serverActive = true;
        try
        {
            if (ip is null) throw new Exception("IP является null");
            IPAddress localAddr = IPAddress.Parse(ip);
            server = new TcpListener(localAddr, port);
            server.Start();

            Byte[] bytes = new Byte[4];

            while (serverActive)
            {
                Console.WriteLine("Ожидаю подключений...");
                using TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Кто-то подключился");

                NetworkStream stream = client.GetStream();

                stream.Read(bytes, 0, 4);
                int data = BitConverter.ToInt32(bytes);

                Console.WriteLine("Клиент отправил: {0}", data);

                if (data == 0) serverActive = false;

                data = Math.Abs(data);

                byte[] intBytes = BitConverter.GetBytes(data);

                Byte[] serverState = BitConverter.GetBytes(serverActive);
                stream.Write(serverState, 0, 1);
                stream.Write(intBytes, 0, 4);

                Console.WriteLine("Сервер ответил: {0}", data);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Исключение: {0}", e);
        }
        finally
        {
            if(server is not null) server.Stop();
        }
        Console.ReadLine();
    }
}