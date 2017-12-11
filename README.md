# azure-what-to-use-when

![Pluralsight logo](https://www.pluralsight.com/content/dam/pluralsight/newsroom/brand-assets/logos/pluralsight-logo-vrt-color-2.png)  

Hi! 

Welcome to the GitHub repository for the Microsoft Azure for Developers: What to Use When? Pluralsight course.
This repository contains the examples for the Pluralsight course: [Microsoft Azure for Developers: What to Use When?](https://app.pluralsight.com/library/courses/dotnet-ecosystem-big-picture).

The solution consists of:

 - BasicSendReceiveUsingQueueClient
   - A .NET Core console application that sends and receives messages through an Azure Service Bus Queue
 - BasicSendReceiveUsingTopicSubscriptionClient
   - A .NET Framework console application that sends and receives messages through an Azure Service Bus Topic
 - TableStorageVsCosmosDB
   - A .NET Framework console application that performs various operations against Azure Table Storage and Azure Cosmos DB Table API
   

#### Running the samples

In order to run the projects in the solution, you need the following:
 - Visual Studio 2017
 - .NET Core 2.0
 - An Azure Service Bus account with one Queue, one Topic and two Subscriptions to that Topic
 - An Azure Cosmos DB account
 
And for the projects specifically:
 - BasicSendReceiveUsingQueueClient
   - The connectionstring to the Azure Service Bus and the name of the queue
   - you enter these values in Program.cs
 - BasicSendReceiveUsingTopicSubscriptionClient
   - The connectionstring to the Azure Service Bus and the name of the topic and subscriptions
   - you enter these values in Program.cs
 - TableStorageVsCosmosDB
   - The connectionstring to Azure Storage that you enter in App.config
   - The connectionstring to Azure Cosmos DB Table API that you enter in App.config
 
