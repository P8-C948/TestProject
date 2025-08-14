using TestProject.Core;

//namespace TestProject.Runner;

class Program
{
    static void Main(string[] args)
    {
        //var c = new Class1();
        Class1 c = new Class1();
        Console.WriteLine(c.GetMessage());
    }
}