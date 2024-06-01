using System.Reflection;
using System.Xml.Linq;
using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

class Lab7
{
    static void Main(string[] args)
    {
        var assembly = Assembly.GetExecutingAssembly(); // Получаем сборку текущего исполняемого файла 
        var types = assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Animal))); // Фильтруем классы, наследующие от Animal 

        XElement root = new XElement("Classes"); // Корневой элемент XML5

        foreach (var type in types)
        {
            // Получаем пользовательский атрибут Comment, если он есть 
            var commentAttr = type.GetCustomAttribute<CommentAttribute>();
            var comment = commentAttr != null ? commentAttr.Comment : "No comment";

            // Создаем элемент для каждого класса 
            XElement classElement = new XElement("Class",
                new XAttribute("Name", type.Name),
                new XAttribute("Comment", comment),
                new XElement("Properties",
                    type.GetProperties().Select(p => new XElement("Property", p.Name))),
                new XElement("Methods",
                    type.GetMethods().Where(m => m.DeclaringType == type).Select(m => new XElement("Method", m.Name)))
            );

            root.Add(classElement); // Добавляем класс в корневой элемент 
        }

        XDocument doc = new XDocument(root); // Создаем XDocument с нашим корневым элементом 
        Console.WriteLine(doc); // Выводим XML на экран 

        // Сохраняем XML в файл 
        doc.Save("classes.xml");
    }
}


// Определение пользовательского атрибута Comment
[AttributeUsage(AttributeTargets.Class)]
public class CommentAttribute : Attribute
{
    public string Comment { get; }

    public CommentAttribute(string comment)
    {
        Comment = comment;
    }
}

// Абстрактный класс Animal
public abstract class Animal
{
    public string Country { get; set; }
    public bool HideFromOtherAnimals { get; set; }
    public string Name { get; set; }
    public string WhatAnimal { get; set; }

    public abstract string GetClassification();
    public abstract string GetFavouriteFood();
    public abstract void SayHello();
}

// Enum для классификации животных
public enum ClassificationAnimal
{
    Herbivores,
    Carnivores,
    Omnivores
}

// Enum для любимой еды
public enum FavouriteFood
{
    Meat,
    Plants,
    Everything
}

// Класс Cow
[Comment("Это класс коровы")]
public class Cow : Animal
{
    public override string GetClassification() => ClassificationAnimal.Herbivores.ToString();
    public override string GetFavouriteFood() => FavouriteFood.Plants.ToString();
    public override void SayHello() => Console.WriteLine("Moo");
}

// Класс Lion
[Comment("Это класс льва")]
public class Lion : Animal
{
    public override string GetClassification() => ClassificationAnimal.Carnivores.ToString();
    public override string GetFavouriteFood() => FavouriteFood.Meat.ToString();
    public override void SayHello() => Console.WriteLine("Roar");
}

// Класс Pig
[Comment("Это класс свиньи")]
public class Pig : Animal
{
    public override string GetClassification() => ClassificationAnimal.Omnivores.ToString();
    public override string GetFavouriteFood() => FavouriteFood.Everything.ToString();
    public override void SayHello() => Console.WriteLine("Oink");
}
