// See https://aka.ms/new-console-template for more information
using System.Data;
using Microsoft.Data.SqlClient;
using NLADotNetInternshipTraining.AdoDotNetSample2;


class Program
{
    static void Main(string[] args)
    {
        AdoDotNetSample adoDotNetSample = new AdoDotNetSample();
        adoDotNetSample.Read();
        adoDotNetSample.Edit();
        adoDotNetSample.Create();
        adoDotNetSample.Update();
        adoDotNetSample.Delete();
        adoDotNetSample.Read();
    }
}





