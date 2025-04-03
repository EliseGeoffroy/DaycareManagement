// See https://aka.ms/new-console-template for more information
using ChildrenManagementClasses;
using General;

Identity identity = new();
Utilities.InputData<Identity>(identity);

Child child = new(identity);
Utilities.InputData<Child>(child);

System.Console.WriteLine(child);
