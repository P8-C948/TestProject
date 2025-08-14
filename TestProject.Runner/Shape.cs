public class Shape
{
    public virtual void Draw() => Console.WriteLine("Drawing a shape");
}

public class Circle : Shape
{
    public override void Draw() => Console.WriteLine("Drawing a circle");
}

public class Square : Shape
{
    public override void Draw() => Console.WriteLine("Drawing a square");
}