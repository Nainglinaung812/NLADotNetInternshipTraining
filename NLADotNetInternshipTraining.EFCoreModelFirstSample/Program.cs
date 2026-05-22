// See https://aka.ms/new-console-template for more information
using NLADotNetInternshipTraining.EFCoreModelFirstSample;
class Prgram
{
    static void Main(string[] args)
    {
        EFCoreModelSample sample = new EFCoreModelSample();
        sample.Edit();
        sample.Create();
        sample.Update();
        sample.Delete();
        sample.Read();
    }
}
