using System;
using System.Text;

public class PixGenerator
{
    private readonly string key; // Chave PIX
    private readonly string receiverName; // Nome do recebedor
    private readonly string city; // Cidade do recebedor

    public PixGenerator(string key, string receiverName, string city = "BRASILIA")
    {
        this.key = key;
        this.receiverName = receiverName.ToUpper();
        this.city = city.ToUpper();
    }

    public string GeneratePixCode(decimal amount = 0)
    {
        string payload = BuildPayload(amount);
        string crc = CalculateCRC16(payload);
        return payload + "6304" + crc;
    }

    private string BuildPayload(decimal amount)
    {
        var payload = new StringBuilder();

        // Payload Format Indicator
        payload.Append(FormatTLV("00", "01"));

        // Merchant Account Information
        string merchantInfo = FormatTLV("00", "BR.GOV.BCB.PIX") +
                              FormatTLV("01", key);
        payload.Append(FormatTLV("26", merchantInfo));

        // Merchant Category Code
        payload.Append(FormatTLV("52", "0000"));

        // Transaction Currency
        payload.Append(FormatTLV("53", "986"));

        // Transaction Amount (if provided)
        if (amount > 0)
        {
            payload.Append(FormatTLV("54", amount.ToString("F2").Replace(',', '.')));
        }

        // Country Code
        payload.Append(FormatTLV("58", "BR"));

        // Merchant Name
        payload.Append(FormatTLV("59", receiverName));

        // Merchant City
        payload.Append(FormatTLV("60", city));

        // Additional Data Field Template (TXID)
        string additionalData = FormatTLV("05", "***"); // TXID genérico
        payload.Append(FormatTLV("62", additionalData));

        return payload.ToString();
    }

    private string FormatTLV(string tag, string value)
    {
        string length = value.Length.ToString("D2");
        return $"{tag}{length}{value}";
    }

    private string CalculateCRC16(string payload)
    {
        // Append "6304" to the payload
        payload += "6304";

        ushort polynomial = 0x1021;
        ushort crc = 0xFFFF;

        foreach (char c in payload)
        {
            crc ^= (ushort)(c << 8);
            for (int i = 0; i < 8; i++)
            {
                if ((crc & 0x8000) != 0)
                {
                    crc = (ushort)((crc << 1) ^ polynomial);
                }
                else
                {
                    crc <<= 1;
                }
            }
        }

        return crc.ToString("X4").ToUpper();
    }
}

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
