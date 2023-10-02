using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using I18nApi.Infrastructure.Common.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace I18nApi.Infrastructure.Services;

public class CsvFileReader : BaseFileReaderHandler
{
    public override IList<Dictionary<string, string?>> Handle(IFormFile file)
    {
        string extension = GetFileExtension(file);
        if (extension != ".csv")
            return base.Handle(file);

        Span<string> headers = GetCsvHeaders(file);

        IList<Dictionary<string, string?>> dataList = new List<Dictionary<string, string?>>();

        int lineIndex = 0;
        byte[] buffer = new byte[1024 * 1024];

        using Stream fs = file.OpenReadStream();
        int bufferedBytes = 0;
        int consumedBytes = 0;

        while (true)
        {
            int bytesLidos = fs.Read(buffer, bufferedBytes, buffer.Length - bufferedBytes);

            if (bytesLidos == 0) break;
            bufferedBytes += bytesLidos;

            int linePosition;

            do
            {
                linePosition = Array.IndexOf(buffer, (byte)'\n', consumedBytes,
                    bufferedBytes - consumedBytes);

                if (linePosition < 0) continue;
                Dictionary<string, string?> data = new();

                int lineLength = linePosition - consumedBytes;
                Span<byte> line = new (buffer, consumedBytes, lineLength);
                consumedBytes += lineLength + 1;

                if (lineIndex == 0)
                {
                    lineIndex++;
                    continue;
                }

                ref string currentHeader = ref MemoryMarshal.GetArrayDataReference(headers.ToArray());
                ref string end = ref Unsafe.Add(ref currentHeader, headers.Length);
                while (Unsafe.IsAddressLessThan(ref currentHeader, ref end))
                {
                    Span<byte> span;
                    string? value;
                    if (currentHeader == headers[0])
                    {
                        span = line.Slice(0, line.IndexOf((byte)','));
                        value = Encoding.UTF8.GetString(span);
                    }
                    else
                    {
                        span = line.Slice(line.IndexOf((byte)',') + 1);
                        int sliceLength = span.IndexOf((byte)',');
                        if (sliceLength == -1)
                        {
                            sliceLength = span.Length;
                        }
                        value = Encoding.UTF8.GetString(span.Slice(0, sliceLength));
                        line = span;
                    }

                    data.Add(currentHeader, value);
                    
                    currentHeader = ref Unsafe.Add(ref currentHeader, 1)!;
                }
                dataList.Add(data);
            } while (linePosition >= 0);

            Array.Copy(buffer, consumedBytes, buffer, 0, (bufferedBytes - consumedBytes));
            bufferedBytes -= consumedBytes;
            consumedBytes = 0;
        }


        return dataList;
    }


    private Span<string> GetCsvHeaders(IFormFile file)
    {
        using StreamReader reader = new(file.OpenReadStream());
        string headerLine = reader.ReadLine()!;

        return headerLine.Split(',').AsSpan();
    }

    private string[] ParseCsvLine(string line)
    {
        IList<string> values = new List<string>();
        bool inQuotes = false;
        int startIndex = 0;

        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (line[i] == ',' && !inQuotes)
            {
                values.Add(line.Substring(startIndex, i - startIndex));
                startIndex = i + 1;
            }
        }

        values.Add(line.Substring(startIndex));

        for (int i = 0; i < values.Count; i++)
        {
            if (values[i].StartsWith("\"") && values[i].EndsWith("\""))
            {
                values[i] = values[i].Substring(1, values[i].Length - 2);
            }
        }

        return values.ToArray();
    }

}