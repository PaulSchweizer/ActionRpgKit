using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.Spreadsheets;

namespace
{
    public class GoogleSheetsDatabase
    {

        public GoogleSheetsDatabase ()
        {
            SpreadsheetsService myService = new SpreadsheetsService("exampleCo-exampleApp-1");
            myService.setUserCredentials("schweizerpau@gmail.com", "mypassword");

            SpreadsheetQuery query = new SpreadsheetQuery();
            SpreadsheetFeed feed = myService.Query(query);

            Console.WriteLine("Your spreadsheets: ");
            foreach (SpreadsheetEntry entry in feed.Entries)
            {
                Console.WriteLine(entry.Title.Text);
            }

            AtomLink link = entry.Links.FindService(GDataSpreadsheetsNameTable.WorksheetRel, null);

            WorksheetQuery query = new WorksheetQuery(link.HRef.ToString());
            WorksheetFeed feed = service.Query(query);

            foreach (WorksheetEntry worksheet in feed.Entries)
            {
                Console.WriteLine(worksheet.Title.Text);
            }

            AtomLink cellFeedLink = worksheetentry.Links.FindService(GDataSpreadsheetsNameTable.CellRel, null);

            CellQuery query = new CellQuery(cellFeedLink.HRef.ToString());
            CellFeed feed = service.Query(query);

            Console.WriteLine("Cells in this worksheet:");
            foreach (CellEntry curCell in feed.Entries)
            {
                Console.WriteLine("Row {0}, column {1}: {2}", curCell.Cell.Row,
                    curCell.Cell.Column, curCell.Cell.Value);
            }
        }
    }
}
