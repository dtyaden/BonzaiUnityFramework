using System;
using System.Collections.Generic;

public class Class1
{
	public Class1()
	{
        List<Dog> dogs = new List<Dog>();
        List<Cat> cats = new List<Cat>();
        doAnimalThing(dogs);
        doAnimalThing(cats);

        Dictionary<int, Dog> dogTags = new Dictionary<int, Dog>();
        doDictionaryThing(dogTags);
	}

    public void doDictionaryThing(Dictionary<int, Animal> animals)
    {

    }

    public void doAnimalThing(List<Animal> animal)
    {

    }

    public class Animal { }

    public class Dog : Animal { }

    public class Cat : Animal { };
}
