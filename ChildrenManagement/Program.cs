﻿// See https://aka.ms/new-console-template for more information


using ChildrenManagement.staticClasses;

Directory.SetCurrentDirectory(@"C:\Users\geoff\Desktop\C#.Net\DaycareManagement\ChildrenManagement\data");

Datas.InitializeAllDatas();

Registration.Register();

await Utilities.ExitApplication();









