using System.Text.Json;

namespace LessonNet
{
	public class Messenge
	{
		public string Text { get; set; }
		public DateTime DateTime { get; set; }
		public string NicNameFrom { get; set; }
		public string NicNameTo { get; set; }

		public string SerializeMessToJson() => JsonSerializer.Serialize(this); 
		public static Messenge? DeserealiazeMessFromJson(string json) => JsonSerializer.Deserialize<Messenge>(json);

		public void PrintMessege()
		{
			Console.WriteLine(ToString());
		}
		public override string ToString()
		{
			return $"{DateTime} Получено сообщение {Text} от {NicNameFrom}";
		}
	}
}
