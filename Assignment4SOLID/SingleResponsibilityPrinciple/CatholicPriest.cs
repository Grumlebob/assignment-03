namespace Assignment4SOLID.SingleResponsibilityPrinciple;

//S Refactored
public class CatholicPriest
{

    public void MolestChild(int age)
    {
        if (age > 10) throw new Exception("Too old");
    }
    
    //The class now only has 1 responsibility.
    //e.g. A ConstructionWorker class can now have the build() method, instead of a big Human class having all the methods.
}