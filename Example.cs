using System;
using PixGenerator;

// Simple Example
class Program
{
    static void Main(string[] args)
    {
        string key = "gabrielmelobatista@hotmail.com"; // Chave PIX
        string receiverName = "Gabriel"; // Nome do recebedor
        decimal amount = 50.00m; // Valor (opcional)
        string city = "ANAPOLIS";

        var pixGenerator = new PixGenerator(key, receiverName, city);
        string pixCode = pixGenerator.GeneratePixCode(amount);

        Console.WriteLine("Código PIX:");
        Console.WriteLine(pixCode);
    }
}
