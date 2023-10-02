using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using I18nApi.Domain.Extensions;
using I18nApi.Infrastructure.Common.Abstractions;
using Microsoft.AspNetCore.Http;
namespace I18nApi.Infrastructure.Services;

public class ParallelExcelFileReaderHandler: BaseFileReaderHandler
{
    public override IList<Dictionary<string, string?>> Handle(IFormFile file)
    {
        string extension = GetFileExtension(file);
        if(extension != ".xlsx" && extension != ".xls")
            return base.Handle(file);
        
        using SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(file.OpenReadStream(), false);
        WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart!;
        Worksheet worksheet = workbookPart!.WorksheetParts.First().Worksheet;
        SheetData sheetData = worksheet.Elements<SheetData>().First();

        List<Row> rows = sheetData.Elements<Row>().ToList();
        Dictionary<string, string> headers = GetSheetHeaders(workbookPart, rows[0]);
        rows.RemoveAt(0);

        ConcurrentBag<Dictionary<string, string?>> dataList = new();

        List<Row>[] batchList = ListExtensions.SplitList(rows, 1000).ToArray();

        Parallel.For(0, batchList.Length, index =>
        {
            List<Row> list = batchList[index];
            ref Row start = ref MemoryMarshal.GetArrayDataReference(list.ToArray());
            ref Row end = ref Unsafe.Add(ref start, list.Count);
            while (Unsafe.IsAddressLessThan(ref start, ref end))
            {
                Dictionary<string, string?> data = new();
                IEnumerable<Cell> cells = start.Elements<Cell>();
                Span<Cell> span = cells.ToArray().AsSpan();

                for (int i = 0; i < span.Length; i++)
                {
                    string? value = span[i]?.CellValue?.Text;
                    string key = headers!.GetValueOrDefault(GetColumnLetter(span[i]))!;
                    data.Add(key, value);
                }

                dataList.Add(data);
                start = ref Unsafe.Add(ref start, 1)!;
            }
        });

        return dataList.ToList();
    }
    
    private Dictionary<string, string> GetSheetHeaders(WorkbookPart workbookPart, Row headerRow)
    {
        SharedStringTablePart shareStringPart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
        SharedStringItem[] items = shareStringPart.SharedStringTable.Elements<SharedStringItem>().ToArray();

        IEnumerable<Cell> headerCells = headerRow.Elements<Cell>();


        Dictionary<string, string> headers = new();

        ref Cell start = ref MemoryMarshal.GetArrayDataReference(headerCells.ToArray());
        ref Cell end = ref Unsafe.Add(ref start, headerCells.Count());

        while (Unsafe.IsAddressLessThan(ref start, ref end))
        {
            string key = GetColumnLetter(start);
            string value = items[int.Parse(start.CellValue!.Text)].InnerText;
            headers.Add(key, value.ToNormalizedString());
            start = ref Unsafe.Add(ref start, 1)!;
        }

        return headers;
    }

    private string GetColumnLetter(Cell cell)
    {
        return Regex.Replace(cell.CellReference!.Value!, @"[\d-]", string.Empty);
    }
}