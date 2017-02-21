using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LQ;

namespace CodeDojo
{
	class Program
	{
		static Random rnd = new Random();
		static void Main(string[] args)
		{
			Class1.Example();

			var pessoas = Pessoa.ListaDePessoas();

			//var testWhere = pessoas.Where(a => a.Nome == "Gustavo");

			//var testSelect = pessoas.Select(a => a.Nome);

			//var numbers = GetNumbers(1000);
			//var fodase = pessoas.TakeWhile(a => a.Nome == "Gustavo");
			//var fodase = pessoas.Skip(5);


			int[] x = new int[] { 1, 2, 3, 4, 5, 6, 7 };
			int[] y = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
			var fodase = x.SequenceEqual(y);
		}


		public static IEnumerable<double> GetNumbers(int count, int min = 0, int max = 1)
		{
			for (int i = 0; i < count; i++)
			{
				yield return rnd.NextDouble() * max + min;
			}
		}

		public static double[] GetNumbers(int count)
		{
			double[] result = new double[count];
			for (int i = 0; i < count; i++)
			{
				result[i] = rnd.NextDouble();
			}
			return result;
		}

		public class Pessoa
		{
			public int Id { get; set; }
			public string Nome { get; set; }

			public static IEnumerable<Pessoa> ListaDePessoas()
			{
				var pessoa1 = new Pessoa() { Id = 1, Nome = "Gustavo" };
				var pessoa2 = new Pessoa() { Id = 1, Nome = "Patrick" };

				return  new List<Pessoa>() {new Pessoa() { Id = 2, Nome = "Patrick" }, new Pessoa() { Id = 2, Nome = "Patrick" }, new Pessoa() { Id = 3, Nome = "Gustavo" }, pessoa2, pessoa1, pessoa1 };
			}
		}
	}
}
