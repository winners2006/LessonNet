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

			Console.WriteLine("������� ����� ������� ��� ���������� ������ �������...");
			Console.ReadKey();
			cancellationTokenSource.Cancel();

			try
			{
				await serverTask;
			}
			catch (OperationCanceledException)
			{
				Console.WriteLine("������ �������� ������.");
			}
		}

		public static async Task UdpServer(CancellationToken token)
		{
			UdpClient udp = new UdpClient(12345);
			IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);
			Console.WriteLine("������ ���� ��������� �� �������");

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
							Console.WriteLine("������ ��������� ������.");
							Environment.Exit(0);
						}

						// �������� ������������� �������
						byte[] confirmationBytes = Encoding.UTF8.GetBytes("��������� ��������");
						await udp.SendAsync(confirmationBytes, confirmationBytes.Length, receiveTask.Result.RemoteEndPoint);
					}
				}
			}
			catch (OperationCanceledException)
			{
				// ��������� ���������� �� ������ ������
			}
			finally
			{
				udp.Close();
				Console.WriteLine("������ �������� ������.");
			}
		}
	}
}
