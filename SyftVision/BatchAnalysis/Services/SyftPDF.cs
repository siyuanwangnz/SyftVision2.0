using BatchAnalysis.Models;
using ChartDirector;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Public.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchAnalysis.Services
{
    public class SyftPDF
    {
        private readonly string LocalChartImagePath = "./Temp/Chart/";
        private readonly string LocalChartImageTempFile = "ChartTemp.png";
        private string LocalChartImageTempFilePath => LocalChartImagePath + LocalChartImageTempFile;

        private PdfDocument PDF;
        private Document Doc;

        private SyftDataHub SyftDatahub;
        private string Comments;
        private string FolderPath;

        private string instruNumber => SyftDatahub.SyftInfoList.Single(a => a.Category == "Instrument" && a.Item == "Number").Content;
        private string batchName => SyftDatahub.SyftInfoList.Single(a => a.Category == "Batch" && a.Item == "Name").Content;
        private string batchNumber => SyftDatahub.SyftInfoList.Single(a => a.Category == "Batch" && a.Item == "Number").Content;
        public SyftPDF(SyftDataHub syftDataHub, string comments, string folderPath)
        {
            SyftDatahub = syftDataHub;
            Comments = comments;
            FolderPath = folderPath;

            // Check local directory
            if (!Directory.Exists(LocalChartImagePath)) Directory.CreateDirectory(LocalChartImagePath);

            Creation();
            Head();
            InfoTable();
            CommentField();
            ChartPage();
            ScanListPage();
            if (Doc != null) Doc.Close();
        }

        private void Creation()
        {
            PDF = new PdfDocument(new PdfWriter($"{FolderPath}/{instruNumber}_{new Options().Operator}_{batchName}_{batchNumber}.pdf"));
            Doc = new Document(PDF, PageSize.A4);
            Doc.SetMargins(30, 30, 30, 30);
        }

        private void Head()
        {
            Doc.Add(new Paragraph(new Text("\n")));
            // Add table
            float[] columnWidths = { 1, 2, 1 };
            Table table = new Table(UnitValue.CreatePercentArray(columnWidths));
            table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            // Get image
            Image image = new Image(ImageDataFactory.Create($"./Image/Syft Logo.png"));
            image.Scale(0.05f, 0.05f);
            // Add image, title and date time
            table.AddCell(new Cell().Add(image).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Syft Batch Analysis\nChart Report")).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetTextAlignment(TextAlignment.CENTER).SetFontSize(20).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph($"Date: {DateTime.Now.ToString("yyyy/MM/dd")}\r\nTime: {DateTime.Now.ToString("HH:mm:ss")}")).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(Border.NO_BORDER));

            Doc.Add(table);

            Doc.Add(new Paragraph(new Text("\n")));

            Doc.Add(new LineSeparator(new DottedLine(1, 2)).SetMarginTop(-4));
        }

        private void InfoTable()
        {
            Doc.Add(new Paragraph(new Text("\n")));
            // Add table
            float[] columnWidths = { 1, 1, 1 };
            Table table = new Table(UnitValue.CreatePercentArray(columnWidths));
            table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            table.SetTextAlignment(TextAlignment.CENTER);
            table.SetWidth(400);
            table.SetFontSize(9);
            // Add info
            foreach (var info in SyftDatahub.SyftInfoList)
            {
                table.AddCell(new Cell().Add(new Paragraph(info.Category)));
                table.AddCell(new Cell().Add(new Paragraph(info.Item)));
                table.AddCell(new Cell().Add(new Paragraph(info.Content)));
            }

            Doc.Add(table);
        }

        private void CommentField()
        {
            Doc.Add(new Paragraph(new Text("\n")));
            // Add Table
            float[] commentColumnWidths = { 1 };
            Table table = new Table(UnitValue.CreatePercentArray(commentColumnWidths));
            table.SetWidth(500);
            table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            // Add title cell
            Cell cellTitle = new Cell().Add(new Paragraph("Comments").SetMarginLeft(10));
            cellTitle.SetFontColor(new DeviceRgb(255, 255, 255));
            cellTitle.SetBackgroundColor(new DeviceRgb(63, 81, 181));
            table.AddCell(cellTitle);
            // Add comments cell
            Cell cellComment = new Cell().Add(new Paragraph(Comments));
            cellComment.SetMinHeight(300);
            cellComment.SetFontSize(9);
            table.AddCell(cellComment);

            Doc.Add(table);
        }

        private void ChartPage()
        {
            foreach (var chart in SyftDatahub.SyftChartList)
            {
                // Add new page
                Doc.Add(new AreaBreak());
                // Add table
                Table table = new Table(new float[2]).UseAllAvailableWidth();
                table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                // Add title cell
                Cell cell = new Cell(1, 2).Add(new Paragraph(chart.ChartProp.Code));
                cell.SetTextAlignment(TextAlignment.CENTER);
                cell.SetFontColor(new DeviceRgb(255, 255, 255));
                cell.SetBackgroundColor(new DeviceRgb(63, 81, 181));
                table.AddCell(cell);
                // Add chart
                chart.Chart.makeChart(LocalChartImageTempFilePath);
                Image img = new Image(ImageDataFactory.Create(LocalChartImageTempFilePath)).Scale(0.45f, 0.45f);
                img.SetHorizontalAlignment((HorizontalAlignment)HorizontalAlignment.CENTER);
                table.AddCell(new Cell(1, 2).Add(img));
                // Add Scan List
                string scanList = $"Scan File List: Count ({chart.ScanFileList.Count})\r\n";
                chart.ScanFileList.ForEach(a => scanList = scanList + a.File + "\r\n");
                Cell scanListCell = new Cell().Add(new Paragraph(scanList));
                scanListCell.SetFontSize(8);
                table.AddCell(scanListCell);
                // Add legend
                float[] columnWidths = { 1 };
                Table legendTable = new Table(UnitValue.CreatePercentArray(columnWidths)).UseAllAvailableWidth();
                legendTable.SetFontSize(8);
                foreach (var legend in chart.XYLegendList)
                {
                    // Add legend cell
                    Cell cellColor = new Cell().Add(new Paragraph(legend.Content)).SetBorder(Border.NO_BORDER);
                    cellColor.SetFontColor(new DeviceRgb(255, 255, 255));
                    System.Drawing.Color c = System.Drawing.Color.FromArgb(legend.Color);
                    cellColor.SetBackgroundColor(new DeviceRgb(c.R, c.G, c.B));
                    legendTable.AddCell(cellColor);
                }
                table.AddCell(legendTable);

                Doc.Add(table);
            }
        }
        private void ScanListPage()
        {
            // Add new page
            Doc.Add(new AreaBreak());
            // Add table
            float[] columnWidths = { 5, 1, 1 };
            Table table = new Table(UnitValue.CreatePercentArray(columnWidths)).UseAllAvailableWidth();
            table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            // Add title cell
            Cell cell = new Cell().Add(new Paragraph("Scan").SetMarginLeft(10));
            cell.SetFontColor(new DeviceRgb(255, 255, 255));
            cell.SetBackgroundColor(new DeviceRgb(63, 81, 181));
            table.AddCell(cell);
            cell = new Cell().Add(new Paragraph("Status"));
            cell.SetTextAlignment(TextAlignment.CENTER);
            cell.SetFontColor(new DeviceRgb(255, 255, 255));
            cell.SetBackgroundColor(new DeviceRgb(63, 81, 181));
            table.AddCell(cell);
            cell = new Cell().Add(new Paragraph("Result"));
            cell.SetTextAlignment(TextAlignment.CENTER);
            cell.SetFontColor(new DeviceRgb(255, 255, 255));
            cell.SetBackgroundColor(new DeviceRgb(63, 81, 181));
            table.AddCell(cell);
            //Add scan
            foreach (var scan in SyftDatahub.SyftScanList)
            {
                // Add scan
                table.AddCell(new Cell().Add(new Paragraph(scan.Scan)).SetFontSize(9));
                // Add status
                Text text = new Text(scan.Status);
                if (scan.Status == "SUCCESS") text.SetFontColor(ColorConstants.GREEN);
                else text.SetFontColor(ColorConstants.RED);
                table.AddCell(new Cell().Add(new Paragraph(text)).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                // Add result
                text = new Text(scan.Result);
                if (scan.Status == "BAD") text.SetFontColor(ColorConstants.RED);
                else text.SetFontColor(ColorConstants.GREEN);
                table.AddCell(new Cell().Add(new Paragraph(text)).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
            }

            Doc.Add(table);
        }


    }
}
