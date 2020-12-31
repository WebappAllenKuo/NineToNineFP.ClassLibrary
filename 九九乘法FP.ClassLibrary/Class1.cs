using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

using 被乘數=System.Int32;
using 乘數 = System.Int32;
namespace 九九乘法FP.ClassLibrary
{
    public class MultiplicationTable
    {
	    public string Execute(被乘數 n1 = 2, 乘數 n2 = 1)
	    {
			return Enumerable.Range(n1, 9 - n1 + 1)
				.MyZip(Enumerable.Range(n2, 9 - n2 + 1)) // {   {{2,1}, {2,2},...{2,9}},  {{3,1}, {3,2},...{3,9}},,,  {{9,1}, {9,2},...{9,9}}   }
				.ForEach(RenderBlock)
				.Aggregate((acc, next) => acc += next);
	    }

	    public virtual string RenderBlock((int, int)[] blockArray) // {{2,1}, {2,2},...{2,9}}
		    => blockArray.Map(RenderRow) // IEnumerable<string> {"3 * 1 = 3\r\n", ..."3 * 9 = 27\r\n"}
			    .ToList()
			    .Append("\r\n") // 加一個item
			    .Append("\r\n") // 加一個item
			    .Aggregate((acc, next)=> acc += next);

		public virtual string RenderRow((int n1, int n2) pair)
	    => pair.Map(p => $"{p.Item1} * {p.Item2} = {p.Item1 * p.Item2}\r\n"); // IEnumerable<string>
			
    }

    public static class IEnumerableExts
    {
	    public static IEnumerable<(T,T)[]> MyZip<T>(this IEnumerable<T> source, IEnumerable<T> target)
	    {
		    foreach (var item1 in source)
		    {
			    var row = new List<(T, T)>(); // Enumerable.Empty<(T,T)>();
			    foreach (var item2 in target)
			    {
				    row.Add((item1, item2));
			    }

			    yield return row.ToArray();
		    }
	    }

	    public static IEnumerable<TR> ForEach<T,TR>(this IEnumerable<T> source, Func<T, TR> func)
	    {
		    foreach (var item in source)
		    {
			    yield return func(item);
		    }
	    }
    }

    public static class ArrayExts
    {
	    public static IEnumerable<TR> Map<T, TR>(this T[] source, Func<T, TR> func)
	    {
		    foreach (var item in source)
		    {
			    yield return func(item);
		    }
	    }
    }
    public static class TupleExts
    {
	    public static T Map<T>(this (int, int) source, Func<(int, int), T> func)
		    => func(source);
    }

    public class IEnumerableExtsTests
    {
	    [Test]
	    public void Zip_WhenCalled()
	    {
		    IEnumerable<int> col1 = Enumerable.Range(1, 2);
			IEnumerable<int> col2 = Enumerable.Range(8, 3);
			var actual = col1.MyZip(col2);
			
			// Assert
			Assert.AreEqual(2, actual.Count());
	    }
    }

	public class MultiplicationTableTests
    {
	    [Test]
	    public void 實際叫用成果()
	    {
		    var sut = new MultiplicationTable();
		    string result = sut.Execute(2, 1);
		    Console.WriteLine(result);
		    
	    }
	    [Test]
	    public void RenderRow_傳回單一行算式()
	    {
			// Arrange
			var sut = new MultiplicationTable();
			(int n1, int n2) pair = (3, 5);
			string expected = "3 * 5 = 15";
			
			// Action
			string actual = sut.RenderRow(pair);
			
			// Assert
			Assert.AreEqual(expected, actual);
	    }

	    [Test]
	    public void RenderBlock_傳回一小組乘法表()
	    {
			// Arrange
			var sut = new TestbaleTable();
			(int, int)[] data = new (int, int)[] { (2,1), (2,2), (2,3) };
			var expected = "row row row \r\n\r\n";
			
			// Act
			var actual = sut.RenderBlock(data);
			
			// Assert
			Assert.AreEqual(expected,actual);
	    }

	    [Test]
	    public void Execute_傳回整份乘法表()
	    {
		    // Arrange
		    var sut = new TestbaleTable();
		    string expected = "blockblock";
		    // Act
		    var actual = sut.Execute(8, 7); // 8~9 * 7~9 =>2 * 3=6組
		    
		    // Assert
		    Assert.AreEqual(expected, actual);
		}

    }

    public class TestbaleTable : MultiplicationTable
    {
	    public override string RenderRow((int n1, int n2) pair)
	    =>"row ";

	    public override string RenderBlock((int, int)[] blockArray)
	    {
		    return "block";
	    }
    }


}
