using System.Net.Sockets;

class Client
{
    private static string? ip;
    private static int port = 13000;
    private static bool SendAndRecive(int number)
    {
        try
        {
            if (ip is null) throw new Exception("Ip не определён");
            using TcpClient client = new TcpClient(ip, port);
            NetworkStream stream = client.GetStream();

            Byte[] data = BitConverter.GetBytes(number);
            stream.Write(data, 0, 4);
            
            Byte[] state = new Byte[1];
            stream.Read(state, 0, 1);
            stream.Read(data, 0, 4);
            int recived = BitConverter.ToInt32(data);
            Console.WriteLine("Сервер ответил: {0}", recived);
            return BitConverter.ToBoolean(state);
        }
        catch (Exception e)
        {
            Console.WriteLine("Исключение: {0}", e.Message);
            Console.WriteLine("Введите IP заново:");
            ip = Console.ReadLine();
            return true;
        }
    }

    public static void Main()
    {
        Console.WriteLine("Введите IP:");
        ip = Console.ReadLine();
        int number;
        Console.WriteLine("Введите число для нахождения модуля");
        bool serverOn = true;
        while (serverOn)
        {
            Console.Write("Введите число: ");
            try
            {
                number = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                continue;
            }

            serverOn = SendAndRecive(number);
        }
        if (!serverOn) Console.WriteLine("Сервер отключён");
        Console.ReadLine();
    }
}