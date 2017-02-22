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
			var alunos = Aluno.ListaDePessoas();

			//var testWhere = pessoas.Where(a => a.Nome == "Gustavo");

			//var testSelect = pessoas.Select(a => a.Nome);

			//var numbers = GetNumbers(1000);
			//var fodase = pessoas.TakeWhile(a => a.Nome == "Gustavo");
			//var fodase = pessoas.Skip(5);

			var lista = new List<object>() { new Pessoa() { Id = 1, Nome = "Gustavo" }, new Aluno() { Id = 432423, RM = "0" } };
			var a = lista.OfType<Pessoa>();	
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

				return new List<Pessoa>() { new Pessoa() { Id = 2, Nome = "Patrick" }, new Pessoa() { Id = 2, Nome = "Patrick" }, new Pessoa() { Id = 3, Nome = "Gustavo" }, pessoa2, pessoa1, pessoa1 };
			}
		}

		public class Aluno
		{
			public int Id { get; set; }
			public string RM { get; set; } = "5";

			public static IEnumerable<Aluno> ListaDePessoas()
			{
				var pessoa1 = new Aluno() { Id = 1, RM = "0" };
				var pessoa2 = new Aluno() { Id = 432423, RM = "0" };

				return new List<Aluno>() { new Aluno() { Id = 2 }, new Aluno() { Id = 4 }, new Aluno() { Id = 3 }, pessoa2, pessoa1 };
			}
		}
	}
}
