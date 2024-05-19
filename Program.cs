using System.Net.Sockets;
using System.Net;
using System.Text;

namespace LessonNet
{
	public class Program
	{
		public static void Main(string[] args)
		{
			UdpServer("Hello");

		}

		public void Task1()
		{
			Messenge msg = new Messenge() { Text = "Test", DateTime = DateTime.Now, NicNameFrom = "from", NicNameTo = "all" };
			string json = msg.SerializeMessToJson();
			Console.WriteLine(json);
			Messenge? msgDes = Messenge.DeserealiazeMessFromJson(json);

		}
		public static void UdpServer(string name)
		{
			UdpClient udp = new UdpClient(12345);
			IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
			Console.WriteLine("Сервер ждет сообщение от клиента");
			
			while (true)
			{
				byte[] buffer = udp.Receive(ref iPEndPoint);
				if (buffer == null) { break; }
				var messageText = Encoding.UTF8.GetString(buffer);

				Messenge? message = Messenge.DeserealiazeMessFromJson(messageText);
				message.PrintMessege();

				// Отправка подтверждения обратно клиенту
				string confirmation = "Сообщение получено";
				byte[] confirmationBytes = Encoding.UTF8.GetBytes(confirmation);
				udp.Send(confirmationBytes, confirmationBytes.Length, iPEndPoint);
			}
		}
		
	}
}
