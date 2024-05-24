using System.Net.Sockets;
using System.Net;
using System.Text;

namespace LessonNet
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var cancellationTokenSource = new CancellationTokenSource();
			var token = cancellationTokenSource.Token;

			var serverTask = Task.Run(() => UdpServer(token), token);

			Console.WriteLine("Нажмите любую клавишу для завершения работы сервера...");
			Console.ReadKey();
			cancellationTokenSource.Cancel();

			try
			{
				await serverTask;
			}
			catch (OperationCanceledException)
			{
				Console.WriteLine("Сервер завершил работу.");
			}
		}

		public static async Task UdpServer(CancellationToken token)
		{
			UdpClient udp = new UdpClient(12345);
			IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
			Console.WriteLine("Сервер ждет сообщение от клиента");

			try
			{
				while (!token.IsCancellationRequested)
				{
					var receiveTask = udp.ReceiveAsync();
					var completedTask = await Task.WhenAny(receiveTask, Task.Delay(-1, token));

					if (completedTask == receiveTask)
					{
						byte[] buffer = receiveTask.Result.Buffer;
						var messageText = Encoding.UTF8.GetString(buffer);

						Messenge? message = Messenge.DeserealiazeMessFromJson(messageText);
						message.PrintMessege();

						if (message.Text.Equals("Exit", StringComparison.OrdinalIgnoreCase))
						{
							Console.WriteLine("Сервер завершает работу.");
							Environment.Exit(0);
						}

						// Отправка подтверждения клиенту
						byte[] confirmationBytes = Encoding.UTF8.GetBytes("Сообщение получено");
						await udp.SendAsync(confirmationBytes, confirmationBytes.Length, receiveTask.Result.RemoteEndPoint);
					}
				}
			}
			catch (OperationCanceledException)
			{
				// Обработка завершения по токену отмены
			}
			finally
			{
				udp.Close();
				Console.WriteLine("Сервер завершил работу.");
			}
		}
	}
}
