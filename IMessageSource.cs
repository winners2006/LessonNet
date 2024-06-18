namespace LessonNet
{
	public interface IMessageSource
	{
		Task StartAsync(CancellationToken token);
	}
}
