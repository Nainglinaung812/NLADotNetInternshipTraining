// See https://aka.ms/new-console-template for more information
using NLADotNetInternshipTraining.EFCoreDatabaseFirst;
using NLADotNetInternshipTraining.EFCoreDatabaseFirstSample;
class Program
{
    static void Main(string[] args)
    {
        EFCoreDatabaseSample sample = new EFCoreDatabaseSample();
        sample.Edit();
        sample.Create();
        sample.Update();
        sample.Delete();
        sample.Read();
    }
}
