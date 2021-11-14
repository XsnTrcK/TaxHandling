// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TaxHandlingLibrary.Models;
using TaxHandlingLibrary.Services;

var baseUris = new Dictionary<string, Uri> { { "IMC_di", new Uri("https://api.taxjar.com/v2/") } };

Console.WriteLine("Customer: ");
var customer = Console.ReadLine();

Console.WriteLine("API Key: ");
var apiKey = Console.ReadLine();

var taxCalculatorFactory = new TaxCalculatorFactory(new AuthenticatedHttpClient(apiKey), baseUris);
var taxServiceFactory = new TaxServiceFactory(taxCalculatorFactory);

while (true)
{
    var taxService = taxServiceFactory.Build(customer);

    Console.WriteLine("Activity: ");
    var activity = Console.ReadLine();
    if (activity == "exit")
        break;
    if (activity == "rate")
    {
        Console.WriteLine("Zip: ");
        var zip = Console.ReadLine();
        var rate = await taxService.GetTaxRateForLocationAsync(new Location(zip));
        Console.WriteLine(JsonConvert.SerializeObject(rate, Formatting.Indented));
    }
    else if (activity == "total")
    {
        Console.WriteLine("What is order total: ");
        var orderTotalString = Console.ReadLine();
        if (!decimal.TryParse(orderTotalString, out var orderTotal))
        {
            Console.WriteLine("Give valid order total");
            continue;
        }
        Console.WriteLine("Zip: ");
        var zip = Console.ReadLine();
        var totalTax = await taxService.GetTotalTaxForOrderAsync(orderTotal, new Location(zip));
        Console.WriteLine($"Total Tax {totalTax}");
    }
}

