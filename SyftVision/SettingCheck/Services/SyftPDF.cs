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
using Public.SettingConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingCheck.Services
{
    public class SyftPDF
    {
        private PdfDocument PDF;
        private Document Doc;

        private SettingProp SettingProp;
        private string Comments;
        private string FolderPath;

        private string instruNumber => SettingProp.SyftInfoList.Single(a => a.Category == "Instrument" && a.Item == "Number").Content;
        private string settingName => SettingProp.SyftInfoList.Single(a => a.Category == "Setting" && a.Item == "Name").Content;
        private string scanDate => SettingProp.SyftInfoList.Single(a => a.Category == "Scan" && a.Item == "Date").Content;
        public SyftPDF(SettingProp settingProp, string comments, string folderPath)
        {
            SettingProp = settingProp;
            Comments = comments;
            FolderPath = folderPath;

            Creation();
            Head();
            InfoTable();
            CommentField();
            SettingListPage();
            if (Doc != null) Doc.Close();
        }

        private void Creation()
        {
            PDF = new PdfDocument(new PdfWriter($"{FolderPath}/{instruNumber}_{new Options().Operator}_{settingName}_{scanDate}.pdf"));
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
            table.AddCell(new Cell().Add(new Paragraph("Syft Setting Check\nData Sheet Report")).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetTextAlignment(TextAlignment.CENTER).SetFontSize(20).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph($"Date: {DateTime.Now:yyyy/MM/dd}\r\nTime: {DateTime.Now:HH:mm:ss}")).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(Border.NO_BORDER));

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
            foreach (var info in SettingProp.SyftInfoList)
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

        private void SettingListPage()
        {
            // Add new page
            Doc.Add(new AreaBreak());

            float[] columnWidths;
            Table table;
            Cell cell;

            foreach (var setting in SettingProp.SettingList)
            {
                switch (setting.Type.Name)
                {
                    case "Map":
                        // Add table
                        columnWidths = new float[] { 2, 1, 2, 1 };
                        table = new Table(UnitValue.CreatePercentArray(columnWidths)).UseAllAvailableWidth();
                        table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                        table.SetFontSize(9);
                        // Add title cell
                        cell = new Cell(1, 4).Add(new Paragraph(setting.Name).SetMarginLeft(10));
                        cell.SetFontColor(new DeviceRgb(255, 255, 255));
                        cell.SetBackgroundColor(new DeviceRgb(63, 81, 181));
                        table.AddCell(cell);
                        // Add content
                        cell = new Cell(1, 4).Add(new Paragraph(setting.ContentList.First()));
                        table.AddCell(cell);
                        foreach (var map in setting.MapSetList)
                        {
                            table.AddCell(new Cell(1, 1).Add(new Paragraph(map.Key)).SetTextAlignment(TextAlignment.CENTER));
                            table.AddCell(new Cell(1, 1).Add(new Paragraph(map.Value.ValueList.First().ToString())).SetTextAlignment(TextAlignment.CENTER));
                            table.AddCell(new Cell(1, 1).Add(new Paragraph($"({map.Value.UnderLimit}, {map.Value.UpperLimit})")).SetTextAlignment(TextAlignment.CENTER));
                            table.AddCell(new Cell(1, 1).Add(new Paragraph(GetColoredStatusText(map.Value.IsOut))).SetTextAlignment(TextAlignment.CENTER));
                        }

                        Doc.Add(table);
                        break;
                    case "Table":
                        // Add table
                        columnWidths = new float[] { 2, 1, 2, 1 };
                        table = new Table(UnitValue.CreatePercentArray(columnWidths)).UseAllAvailableWidth();
                        table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                        table.SetFontSize(9);
                        // Add title cell
                        cell = new Cell(1, 4).Add(new Paragraph(setting.Name).SetMarginLeft(10));
                        cell.SetFontColor(new DeviceRgb(255, 255, 255));
                        cell.SetBackgroundColor(new DeviceRgb(63, 81, 181));
                        table.AddCell(cell);
                        // Add content
                        cell = new Cell(1, 4).Add(new Paragraph(setting.ContentList.First()));
                        table.AddCell(cell);
                        foreach (var t in setting.TableSetList)
                        {
                            table.AddCell(new Cell(1, 1).Add(new Paragraph(t.Key.ToString())).SetTextAlignment(TextAlignment.CENTER));
                            table.AddCell(new Cell(1, 1).Add(new Paragraph(t.Value.ValueList.First().ToString())).SetTextAlignment(TextAlignment.CENTER));
                            table.AddCell(new Cell(1, 1).Add(new Paragraph($"({t.Value.UnderLimit}, {t.Value.UpperLimit})")).SetTextAlignment(TextAlignment.CENTER));
                            table.AddCell(new Cell(1, 1).Add(new Paragraph(GetColoredStatusText(t.Value.IsOut))).SetTextAlignment(TextAlignment.CENTER));
                        }

                        Doc.Add(table);
                        break;
                    case "OnOff":
                        // Add table
                        columnWidths = new float[] { 2, 3, 1 };
                        table = new Table(UnitValue.CreatePercentArray(columnWidths)).UseAllAvailableWidth();
                        table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                        table.SetFontSize(9);
                        // Add title cell
                        cell = new Cell(1, 3).Add(new Paragraph(setting.Name).SetMarginLeft(10));
                        cell.SetFontColor(new DeviceRgb(255, 255, 255));
                        cell.SetBackgroundColor(new DeviceRgb(63, 81, 181));
                        table.AddCell(cell);
                        // Add content
                        table.AddCell(new Cell(1, 1).Add(new Paragraph(setting.OnOff.OnOffList.First().ToString())).SetTextAlignment(TextAlignment.CENTER));
                        table.AddCell(new Cell(1, 1).Add(new Paragraph($"({setting.OnOff.ReferOnOff})")).SetTextAlignment(TextAlignment.CENTER));
                        table.AddCell(new Cell(1, 1).Add(new Paragraph(GetColoredStatusText(setting.OnOff.IsOut))).SetTextAlignment(TextAlignment.CENTER));

                        Doc.Add(table);
                        break;
                    case "Value":
                        // Add table
                        columnWidths = new float[] { 2, 3, 1 };
                        table = new Table(UnitValue.CreatePercentArray(columnWidths)).UseAllAvailableWidth();
                        table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                        table.SetFontSize(9);
                        // Add title cell
                        cell = new Cell(1, 3).Add(new Paragraph(setting.Name).SetMarginLeft(10));
                        cell.SetFontColor(new DeviceRgb(255, 255, 255));
                        cell.SetBackgroundColor(new DeviceRgb(63, 81, 181));
                        table.AddCell(cell);
                        // Add content
                        table.AddCell(new Cell(1, 1).Add(new Paragraph(setting.Value.ValueList.First().ToString())).SetTextAlignment(TextAlignment.CENTER));
                        table.AddCell(new Cell(1, 1).Add(new Paragraph($"({setting.Value.UnderLimit}, {setting.Value.UpperLimit})")).SetTextAlignment(TextAlignment.CENTER));
                        table.AddCell(new Cell(1, 1).Add(new Paragraph(GetColoredStatusText(setting.Value.IsOut))).SetTextAlignment(TextAlignment.CENTER));

                        Doc.Add(table);
                        break;
                    case "Text":
                        columnWidths = new float[] { 2, 3, 1 };
                        table = new Table(UnitValue.CreatePercentArray(columnWidths)).UseAllAvailableWidth();
                        table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                        table.SetFontSize(9);
                        // Add title cell
                        cell = new Cell(1, 3).Add(new Paragraph(setting.Name).SetMarginLeft(10));
                        cell.SetFontColor(new DeviceRgb(255, 255, 255));
                        cell.SetBackgroundColor(new DeviceRgb(63, 81, 181));
                        table.AddCell(cell);
                        // Add content
                        table.AddCell(new Cell(1, 1).Add(new Paragraph(setting.Text.TextList.First())).SetTextAlignment(TextAlignment.CENTER));
                        table.AddCell(new Cell(1, 1).Add(new Paragraph($"({setting.Text.ReferText})")).SetTextAlignment(TextAlignment.CENTER));
                        table.AddCell(new Cell(1, 1).Add(new Paragraph(GetColoredStatusText(setting.Text.IsOut))).SetTextAlignment(TextAlignment.CENTER));

                        Doc.Add(table);
                        break;
                }
                Doc.Add(new Paragraph(new Text("\n")).SetHeight(3));
            }
        }
        private Text GetColoredStatusText(bool isOut)
        {
            Text text = new Text(isOut ? "Fail" : "Pass");
            text.SetFontColor(isOut ? ColorConstants.RED : ColorConstants.GREEN);
            return text;
        }
    }
}
