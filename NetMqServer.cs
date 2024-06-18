using NetMQ;
using NetMQ.Sockets;

namespace LessonNet
{
	public class NetMqServer : IMessageSource
	{
		public async Task StartAsync(CancellationToken token)
		{
			using (var server = new ResponseSocket("@tcp://localhost:12345"))
			{
				Console.WriteLine("Сервер ждет сообщение от клиента");

				while (!token.IsCancellationRequested)
				{
					try
					{
						var messageText = await Task.Run(() => server.ReceiveFrameString(), token);
						var message = Messenge.DeserealiazeMessFromJson(messageText);
						message?.PrintMessege();

						if (message?.Text.Equals("Exit", StringComparison.OrdinalIgnoreCase) == true)
						{
							Console.WriteLine("Сервер завершает работу.");
							break;
						}

						server.SendFrame("Сообщение получено");
					}
					catch (OperationCanceledException)
					{
						break;
					}
				}
			}

			Console.WriteLine("Сервер завершил работу.");
		}
	}
}
