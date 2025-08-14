namespace TestProject.Core;

public class Class1
{
    public string GetMessage()
    {
        return "Hello from Class in Core Library!";
    }
     
    public static void Main(string[] args)
    {
        //var c = new Class1();
        //Class1 c = new Class1();
        Console.WriteLine("Hello from Class not from Runner Library!");
    }


}
