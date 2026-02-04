namespace SamplesAndConcepts;


public class Person
{
    public int Age { get; set; }
    public string? Name { get; set; }
    public bool IsMale { get; set; }
}

public static class Sample
{
    static void GoDrinkBear() { }

    public static void Test(Person person, State state)
    {
        // 1
        if (person.Age > 18 && person.Name == "Orel" && person.IsMale)
        {
            GoDrinkBear();
        }

        // 2
        if (Utils.CanDrinkBear(person))
        {
            GoDrinkBear();
        }

        // 3
        if (person.CanDrinkBear1())
        {
            GoDrinkBear();
        }
        // 4
        {
            if (state.ValidForBeer())
            {
                GoDrinkBear();
            }
        }
    }

}

public enum State
{
    Valid
};

public static class Utils
{
    public static bool ValidForBeer(this State s)
    {
        return s is State.Valid;

    }
    public static bool CanDrinkBear(Person person)
    {
        return person.Age > 18 && person.Name == "orel" && person.IsMale;
    }
    public static bool CanDrinkBear1(this Person person)
    {
        return person.Age > 18 && person.Name == "orel" && person.IsMale;
    }
}