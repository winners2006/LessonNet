using System;
using System.Threading;
using System.Threading.Tasks;
using LessonNet;

namespace LessonNetServer
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var cancellationTokenSource = new CancellationTokenSource();
			var token = cancellationTokenSource.Token;

			var server = new NetMqServer();
			var serverTask = server.StartAsync(token);

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
	}
}
