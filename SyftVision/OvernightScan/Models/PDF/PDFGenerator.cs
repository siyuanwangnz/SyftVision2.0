using iText.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Image;
using Public.Global;
using iText.Kernel.Colors;

namespace OvernightScan.Models
{
    class PDFGenerator
    {
        public static void PDFHead(ref Document doc, string title)
        {
            doc.Add(new Paragraph(new Text("\n")));

            float[] columnWidths = { 1, 2, 1 };
            Table table = new Table(UnitValue.CreatePercentArray(columnWidths));
            table.SetHorizontalAlignment(HorizontalAlignment.CENTER);

            Image image = new Image(ImageDataFactory.Create($"./Image/Syft Logo.png"));
            image.Scale(0.05f, 0.05f);

            table.AddCell(new Cell(1, 1).Add(image).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell(1, 1).Add(new Paragraph(title)).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetTextAlignment(TextAlignment.CENTER).SetFontSize(20).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell(1, 1).Add(new Paragraph($"Date: {DateTime.Now.ToString("yyyy/MM/dd")}\r\nTime: {DateTime.Now.ToString("HH:mm:ss")}")).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(Border.NO_BORDER));

            doc.Add(table);

            doc.Add(new Paragraph(new Text("\n")));

            doc.Add(new LineSeparator(new DottedLine(1, 2)).SetMarginTop(-4));
        }

        public static void PDFInstrumentInfo(ref Document doc, SyftXML.Instrument instrument, string comment)
        {
            doc.Add(new Paragraph(new Text("\n")));

            //Instrument Info
            float[] columnWidths = { 2, 1 };
            Table table = new Table(UnitValue.CreatePercentArray(columnWidths));
            table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            table.SetTextAlignment(TextAlignment.CENTER);
            table.SetWidth(400);
            table.SetFontSize(8);

            table.AddCell(new Cell(1, 1).Add(new Paragraph($"Operator Name")));
            table.AddCell(new Cell(1, 1).Add(new Paragraph($"{Global.UserName}")));

            table.AddCell(new Cell(1, 1).Add(new Paragraph($"Instrument Model")));
            table.AddCell(new Cell(1, 1).Add(new Paragraph($"{instrument.Model}")));

            table.AddCell(new Cell(1, 1).Add(new Paragraph($"Instrument Number")));
            table.AddCell(new Cell(1, 1).Add(new Paragraph($"{instrument.Number}")));

            table.AddCell(new Cell(1, 1).Add(new Paragraph($"Instrument Serial Number")));
            table.AddCell(new Cell(1, 1).Add(new Paragraph($"{instrument.SN}")));

            doc.Add(table);

            doc.Add(new Paragraph(new Text("\n")));

            //Comments
            float[] commentColumnWidths = { 1 };
            Table commentTable = new Table(UnitValue.CreatePercentArray(commentColumnWidths));
            commentTable.SetWidth(500);
            commentTable.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            commentTable.SetTextAlignment(TextAlignment.LEFT);
            commentTable.SetFontSize(8);
            //Title cell
            Cell cellTitle = new Cell().Add(new Paragraph("Comments"));
            cellTitle.SetTextAlignment(TextAlignment.LEFT);
            cellTitle.SetPadding(4);
            cellTitle.SetBackgroundColor(new DeviceRgb(159, 168, 218));
            commentTable.AddCell(cellTitle);
            //Comments content
            Cell cellComment = new Cell().Add(new Paragraph(comment));
            cellComment.SetHeight(300);
            cellComment.SetTextAlignment(TextAlignment.LEFT);
            commentTable.AddCell(cellComment);

            doc.Add(commentTable);

            doc.Add(new Paragraph(new Text("\n")));
        }

        public static void ChartTable(ref Document doc, bool newPage, string tableTitleName, int batchNumber, params string[] pathArray)
        {
            //add new page
            if(newPage) doc.Add(new AreaBreak());
            else doc.Add(new Paragraph(new Text("\n")));
            //add table with title
            Table table = new Table(new float[1]).UseAllAvailableWidth();
            Cell cell = new Cell().Add(new Paragraph(tableTitleName));
            cell.SetTextAlignment(TextAlignment.CENTER);
            cell.SetPadding(5);
            cell.SetBackgroundColor(new DeviceRgb(159, 168, 218));
            table.AddCell(cell);
            //add subtable with title
            Table subTable = new Table(new float[2]).UseAllAvailableWidth();
            if (batchNumber > 1)
                cell = new Cell(1, 2).Add(new Paragraph($"The last Batch"));
            else
                cell = new Cell(1, 2).Add(new Paragraph($"Selected Batch"));
            cell.SetTextAlignment(TextAlignment.CENTER);
            cell.SetPadding(5);
            cell.SetBackgroundColor(new DeviceRgb(197, 202, 233));
            subTable.AddCell(cell);
            //add image
            if (pathArray.Count() > 0)
            {
                //check odd
                if (Convert.ToBoolean(pathArray.Count() % 2))
                {
                    bool isFirst = true;
                    foreach (var path in pathArray)
                    {
                        Image img = new Image(ImageDataFactory.Create($"{path}")).Scale(0.25f, 0.25f);
                        if (isFirst)
                        {
                            img.SetMarginLeft(145);
                            subTable.AddCell(new Cell(1, 2).Add(img));
                            isFirst = false;
                        }
                        else
                            subTable.AddCell(new Cell().Add(img));
                    }

                }
                else
                {
                    foreach (var path in pathArray)
                    {
                        Image img = new Image(ImageDataFactory.Create($"{path}")).Scale(0.25f, 0.25f);
                        subTable.AddCell(new Cell().Add(img));
                    }

                }
            }
            table.AddCell(subTable);
            doc.Add(table);
        }

        public static void ChartTable_SPISDoubleOvernight(ref Document doc, bool newPage,
            string SPISOvernightConcentrationsRSDChartOverallSavedPath,
            string SPISOvernightConcentrationsRSDChartSavedPath,
            string SPISOvernightConcentrationsChartSavedPath,
            string SPISOvernightReagentIonsChartSavedPath,
            string SPISOvernightProductIonsChartSavedPath,
            string SPISOvernightQuadStabilityChartSavedPath,
            string SPISOvernightConcentrationsRSDChart2SavedPath,
            string SPISOvernightConcentrationsChart2SavedPath,
            string SPISOvernightReagentIonsChart2SavedPath,
            string SPISOvernightProductIonsChart2SavedPath,
            string SPISOvernightQuadStabilityChart2SavedPath)
        {
            if (newPage) doc.Add(new AreaBreak());
            else doc.Add(new Paragraph(new Text("\n")));
            Table table = new Table(new float[1]).UseAllAvailableWidth();
            Cell cellTitle = new Cell().Add(new Paragraph("Overnight"));
            cellTitle.SetTextAlignment(TextAlignment.CENTER);
            cellTitle.SetPadding(5);
            cellTitle.SetBackgroundColor(new DeviceRgb(159, 168, 218));
            table.AddCell(cellTitle);

            //Overall
            Table tableTemp = new Table(new float[2]).UseAllAvailableWidth();
            Cell cellTitleTemp = new Cell(1, 2).Add(new Paragraph($"Overall"));
            cellTitleTemp.SetTextAlignment(TextAlignment.CENTER);
            cellTitleTemp.SetPadding(5);
            cellTitleTemp.SetBackgroundColor(new DeviceRgb(197, 202, 233));
            tableTemp.AddCell(cellTitleTemp);

            Image img1Temp = new Image(ImageDataFactory.Create($"{SPISOvernightConcentrationsRSDChartOverallSavedPath}")).Scale(0.25f, 0.25f).SetMarginLeft(145);

            tableTemp.AddCell(new Cell(1, 2).Add(img1Temp));
            table.AddCell(tableTemp);

            //Overnight 1
            tableTemp = new Table(new float[2]).UseAllAvailableWidth();
            cellTitleTemp = new Cell(1, 2).Add(new Paragraph($"Overnight 1"));
            cellTitleTemp.SetTextAlignment(TextAlignment.CENTER);
            cellTitleTemp.SetPadding(5);
            cellTitleTemp.SetBackgroundColor(new DeviceRgb(197, 202, 233));
            tableTemp.AddCell(cellTitleTemp);

            img1Temp = new Image(ImageDataFactory.Create($"{SPISOvernightConcentrationsRSDChartSavedPath}")).Scale(0.25f, 0.25f).SetMarginLeft(145);
            Image img2Temp = new Image(ImageDataFactory.Create($"{SPISOvernightConcentrationsChartSavedPath}")).Scale(0.25f, 0.25f);
            Image img3Temp = new Image(ImageDataFactory.Create($"{SPISOvernightReagentIonsChartSavedPath}")).Scale(0.25f, 0.25f);
            Image img4Temp = new Image(ImageDataFactory.Create($"{SPISOvernightProductIonsChartSavedPath}")).Scale(0.25f, 0.25f);
            Image img5Temp = new Image(ImageDataFactory.Create($"{SPISOvernightQuadStabilityChartSavedPath}")).Scale(0.25f, 0.25f);

            tableTemp.AddCell(new Cell(1, 2).Add(img1Temp));
            tableTemp.AddCell(new Cell().Add(img2Temp));
            tableTemp.AddCell(new Cell().Add(img3Temp));
            tableTemp.AddCell(new Cell().Add(img4Temp));
            tableTemp.AddCell(new Cell().Add(img5Temp));

            table.AddCell(tableTemp);
            //Overnight 2
            tableTemp = new Table(new float[2]).UseAllAvailableWidth();
            cellTitleTemp = new Cell(1, 2).Add(new Paragraph($"Overnight 2"));
            cellTitleTemp.SetTextAlignment(TextAlignment.CENTER);
            cellTitleTemp.SetPadding(5);
            cellTitleTemp.SetBackgroundColor(new DeviceRgb(197, 202, 233));
            tableTemp.AddCell(cellTitleTemp);

            img1Temp = new Image(ImageDataFactory.Create($"{SPISOvernightConcentrationsRSDChart2SavedPath}")).Scale(0.25f, 0.25f).SetMarginLeft(145);
            img2Temp = new Image(ImageDataFactory.Create($"{SPISOvernightConcentrationsChart2SavedPath}")).Scale(0.25f, 0.25f);
            img3Temp = new Image(ImageDataFactory.Create($"{SPISOvernightReagentIonsChart2SavedPath}")).Scale(0.25f, 0.25f);
            img4Temp = new Image(ImageDataFactory.Create($"{SPISOvernightProductIonsChart2SavedPath}")).Scale(0.25f, 0.25f);
            img5Temp = new Image(ImageDataFactory.Create($"{SPISOvernightQuadStabilityChart2SavedPath}")).Scale(0.25f, 0.25f);

            tableTemp.AddCell(new Cell(1, 2).Add(img1Temp));
            tableTemp.AddCell(new Cell().Add(img2Temp));
            tableTemp.AddCell(new Cell().Add(img3Temp));
            tableTemp.AddCell(new Cell().Add(img4Temp));
            tableTemp.AddCell(new Cell().Add(img5Temp));

            table.AddCell(tableTemp);
            doc.Add(table);
        }

        public static void ChartTable_SenAndImp(ref Document doc, bool newPage, int batchNumber, 
            List<string> SensitiveChartListSavedPath,
            List<string> ImpurityChartListSavedPath)
        {
            if (newPage) doc.Add(new AreaBreak());
            else doc.Add(new Paragraph(new Text("\n")));
            Table table = new Table(new float[1]).UseAllAvailableWidth();
            Cell cellTitle = new Cell().Add(new Paragraph("Sensitivities and Impurities"));
            cellTitle.SetTextAlignment(TextAlignment.CENTER);
            cellTitle.SetPadding(5);
            cellTitle.SetBackgroundColor(new DeviceRgb(159, 168, 218));
            table.AddCell(cellTitle);

            int subTableCount = SensitiveChartListSavedPath.Count;
            for (int i = 0; i < subTableCount; i++)
            {
                Table tableTemp = new Table(new float[2]).UseAllAvailableWidth();
                Cell cellTitleTemp;
                if (batchNumber > 1)
                    cellTitleTemp = new Cell(1, 2).Add(new Paragraph($"Batch {i + 1}"));
                else
                    cellTitleTemp = new Cell(1, 2).Add(new Paragraph($"Selected Batch"));
                cellTitleTemp.SetTextAlignment(TextAlignment.CENTER);
                cellTitleTemp.SetPadding(5);
                cellTitleTemp.SetBackgroundColor(new DeviceRgb(197, 202, 233));
                tableTemp.AddCell(cellTitleTemp);

                Image img1Temp = new Image(ImageDataFactory.Create($"{SensitiveChartListSavedPath[i]}"));
                Image img2Temp = new Image(ImageDataFactory.Create($"{ImpurityChartListSavedPath[i]}"));
                img1Temp.Scale(0.25f, 0.25f);
                img2Temp.Scale(0.25f, 0.25f);

                Cell cell1Temp = new Cell().Add(img1Temp);
                Cell cell2Temp = new Cell().Add(img2Temp);
                tableTemp.AddCell(cell1Temp);
                tableTemp.AddCell(cell2Temp);

                table.AddCell(tableTemp);
            }
            doc.Add(table);
        }

        public static void ChartTable_DPISLongTerm(ref Document doc, bool newPage,
            List<string> DPISLongTermStabilityReagentRSDChartListSavedPath,
            List<string> DPISLongTermStabilityPosWetReagentChartListSavedPath,
            List<string> DPISLongTermStabilityPosWetProductChartListSavedPath,
            List<string> DPISLongTermStabilityNegWetReagentChartListSavedPath,
            List<string> DPISLongTermStabilityNegWetProductChartListSavedPath,
            List<string> DPISLongTermStabilityNegDryReagentChartListSavedPath,
            List<string> DPISLongTermStabilityNegDryProductChartListSavedPath)
        {
            if (newPage) doc.Add(new AreaBreak());
            else doc.Add(new Paragraph(new Text("\n")));
            Table table = new Table(new float[1]).UseAllAvailableWidth();
            Cell cellTitle = new Cell().Add(new Paragraph("Long Term Stability"));
            cellTitle.SetTextAlignment(TextAlignment.CENTER);
            cellTitle.SetPadding(5);
            cellTitle.SetBackgroundColor(new DeviceRgb(159, 168, 218));
            table.AddCell(cellTitle);

            int subTableCount = DPISLongTermStabilityPosWetReagentChartListSavedPath.Count;
            for (int i = 0; i < subTableCount; i++)
            {
                if (subTableCount > 1)
                {
                    if (i == 0)
                    {
                        Table tableTempOverall = new Table(new float[1]).UseAllAvailableWidth();
                        Cell cellTitleTempOverall = new Cell().Add(new Paragraph($"Overall"));
                        cellTitleTempOverall.SetTextAlignment(TextAlignment.CENTER);
                        cellTitleTempOverall.SetPadding(5);
                        cellTitleTempOverall.SetBackgroundColor(new DeviceRgb(197, 202, 233));
                        tableTempOverall.AddCell(cellTitleTempOverall);

                        Image img1TempOverall = new Image(ImageDataFactory.Create($"{DPISLongTermStabilityReagentRSDChartListSavedPath[0]}")).Scale(0.25f, 0.25f).SetMarginLeft(145);
                        Cell cell1TempOverall = new Cell().Add(img1TempOverall);
                        tableTempOverall.AddCell(cell1TempOverall);

                        table.AddCell(tableTempOverall);
                    }

                    Table tableTemp = new Table(new float[2]).UseAllAvailableWidth();
                    Cell cellTitleTemp = new Cell(1, 2).Add(new Paragraph($"Batch {i + 1}"));
                    cellTitleTemp.SetTextAlignment(TextAlignment.CENTER);
                    cellTitleTemp.SetPadding(5);
                    cellTitleTemp.SetBackgroundColor(new DeviceRgb(197, 202, 233));
                    tableTemp.AddCell(cellTitleTemp);

                    Image img1Temp = new Image(ImageDataFactory.Create($"{DPISLongTermStabilityReagentRSDChartListSavedPath?[i + 1]}")).Scale(0.25f, 0.25f).SetMarginLeft(145);
                    Image img2Temp = new Image(ImageDataFactory.Create($"{DPISLongTermStabilityPosWetReagentChartListSavedPath[i]}")).Scale(0.25f, 0.25f);
                    Image img3Temp = new Image(ImageDataFactory.Create($"{DPISLongTermStabilityPosWetProductChartListSavedPath[i]}")).Scale(0.25f, 0.25f);
                    Image img4Temp = new Image(ImageDataFactory.Create($"{DPISLongTermStabilityNegWetReagentChartListSavedPath[i]}")).Scale(0.25f, 0.25f);
                    Image img5Temp = new Image(ImageDataFactory.Create($"{DPISLongTermStabilityNegWetProductChartListSavedPath[i]}")).Scale(0.25f, 0.25f);
                    Image img6Temp = new Image(ImageDataFactory.Create($"{DPISLongTermStabilityNegDryReagentChartListSavedPath[i]}")).Scale(0.25f, 0.25f);
                    Image img7Temp = new Image(ImageDataFactory.Create($"{DPISLongTermStabilityNegDryProductChartListSavedPath[i]}")).Scale(0.25f, 0.25f);

                    tableTemp.AddCell(new Cell(1, 2).Add(img1Temp));
                    tableTemp.AddCell(new Cell().Add(img2Temp));
                    tableTemp.AddCell(new Cell().Add(img3Temp));
                    tableTemp.AddCell(new Cell().Add(img4Temp));
                    tableTemp.AddCell(new Cell().Add(img5Temp));
                    tableTemp.AddCell(new Cell().Add(img6Temp));
                    tableTemp.AddCell(new Cell().Add(img7Temp));

                    table.AddCell(tableTemp);
                }
                else
                {
                    Table tableTemp = new Table(new float[2]).UseAllAvailableWidth();
                    Cell cellTitleTemp = new Cell(1, 2).Add(new Paragraph($"Selected Batch"));
                    cellTitleTemp.SetTextAlignment(TextAlignment.CENTER);
                    cellTitleTemp.SetPadding(5);
                    cellTitleTemp.SetBackgroundColor(new DeviceRgb(197, 202, 233));
                    tableTemp.AddCell(cellTitleTemp);

                    Image img1Temp = new Image(ImageDataFactory.Create($"{DPISLongTermStabilityReagentRSDChartListSavedPath[i]}")).Scale(0.25f, 0.25f).SetMarginLeft(145);
                    Image img2Temp = new Image(ImageDataFactory.Create($"{DPISLongTermStabilityPosWetReagentChartListSavedPath[i]}")).Scale(0.25f, 0.25f);
                    Image img3Temp = new Image(ImageDataFactory.Create($"{DPISLongTermStabilityPosWetProductChartListSavedPath[i]}")).Scale(0.25f, 0.25f);
                    Image img4Temp = new Image(ImageDataFactory.Create($"{DPISLongTermStabilityNegWetReagentChartListSavedPath[i]}")).Scale(0.25f, 0.25f);
                    Image img5Temp = new Image(ImageDataFactory.Create($"{DPISLongTermStabilityNegWetProductChartListSavedPath[i]}")).Scale(0.25f, 0.25f);
                    Image img6Temp = new Image(ImageDataFactory.Create($"{DPISLongTermStabilityNegDryReagentChartListSavedPath[i]}")).Scale(0.25f, 0.25f);
                    Image img7Temp = new Image(ImageDataFactory.Create($"{DPISLongTermStabilityNegDryProductChartListSavedPath[i]}")).Scale(0.25f, 0.25f);

                    tableTemp.AddCell(new Cell(1, 2).Add(img1Temp));
                    tableTemp.AddCell(new Cell().Add(img2Temp));
                    tableTemp.AddCell(new Cell().Add(img3Temp));
                    tableTemp.AddCell(new Cell().Add(img4Temp));
                    tableTemp.AddCell(new Cell().Add(img5Temp));
                    tableTemp.AddCell(new Cell().Add(img6Temp));
                    tableTemp.AddCell(new Cell().Add(img7Temp));

                    table.AddCell(tableTemp);
                }
            }
            doc.Add(table);
        }

        public static void ChartTable_InfinityStability(ref Document doc, bool newPage, int batchNumber,
            List<string> StabilityReagentProductRSDChartListSavedPath,
            string StabilityConcentrationsChartSavedPath,
            string StabilityReagentIonsChartSavedPath,
            string StabilityProductIonsChartSavedPath,
            string StabilityConcentrationswEOVChartSavedPath,
            string StabilityReactionTimeEOVChartSavedPath)
        {
            if (newPage) doc.Add(new AreaBreak());
            else doc.Add(new Paragraph(new Text("\n")));
            Table table = new Table(new float[1]).UseAllAvailableWidth();
            Cell cellTitle = new Cell().Add(new Paragraph("Stability"));
            cellTitle.SetTextAlignment(TextAlignment.CENTER);
            cellTitle.SetPadding(5);
            cellTitle.SetBackgroundColor(new DeviceRgb(159, 168, 218));
            table.AddCell(cellTitle);

            if (StabilityReagentProductRSDChartListSavedPath.Count > 1)
            {
                Table tableTempOverall = new Table(new float[1]).UseAllAvailableWidth();
                Cell cellTitleTempOverall = new Cell().Add(new Paragraph($"Overall"));
                cellTitleTempOverall.SetTextAlignment(TextAlignment.CENTER);
                cellTitleTempOverall.SetPadding(5);
                cellTitleTempOverall.SetBackgroundColor(new DeviceRgb(197, 202, 233));
                tableTempOverall.AddCell(cellTitleTempOverall);

                Image img1TempOverall = new Image(ImageDataFactory.Create($"{StabilityReagentProductRSDChartListSavedPath[0]}")).Scale(0.25f, 0.25f).SetMarginLeft(145);
                Cell cell1TempOverall = new Cell().Add(img1TempOverall);
                tableTempOverall.AddCell(cell1TempOverall);

                table.AddCell(tableTempOverall);
            }

            Table tableTemp = new Table(new float[2]).UseAllAvailableWidth();
            Cell cellTitleTemp;
            int RSDnumber = 0;
            if (batchNumber > 1)
            {
                cellTitleTemp = new Cell(1, 2).Add(new Paragraph($"The last Batch"));
                RSDnumber = 1;
            }
            else
                cellTitleTemp = new Cell(1, 2).Add(new Paragraph($"Selected Batch"));
            cellTitleTemp.SetTextAlignment(TextAlignment.CENTER);
            cellTitleTemp.SetPadding(5);
            cellTitleTemp.SetBackgroundColor(new DeviceRgb(197, 202, 233));
            tableTemp.AddCell(cellTitleTemp);

            Image img1Temp = new Image(ImageDataFactory.Create($"{StabilityReagentProductRSDChartListSavedPath[RSDnumber]}")).Scale(0.25f, 0.25f);
            Image img2Temp = new Image(ImageDataFactory.Create($"{StabilityConcentrationsChartSavedPath}")).Scale(0.25f, 0.25f);
            Image img3Temp = new Image(ImageDataFactory.Create($"{StabilityReagentIonsChartSavedPath}")).Scale(0.25f, 0.25f);
            Image img4Temp = new Image(ImageDataFactory.Create($"{StabilityProductIonsChartSavedPath}")).Scale(0.25f, 0.25f);
            Image img5Temp = new Image(ImageDataFactory.Create($"{StabilityConcentrationswEOVChartSavedPath}")).Scale(0.25f, 0.25f);
            Image img6Temp = new Image(ImageDataFactory.Create($"{StabilityReactionTimeEOVChartSavedPath}")).Scale(0.25f, 0.25f);

            tableTemp.AddCell(new Cell().Add(img1Temp));
            tableTemp.AddCell(new Cell().Add(img2Temp));
            tableTemp.AddCell(new Cell().Add(img3Temp));
            tableTemp.AddCell(new Cell().Add(img4Temp));
            tableTemp.AddCell(new Cell().Add(img5Temp));
            tableTemp.AddCell(new Cell().Add(img6Temp));

            table.AddCell(tableTemp);

            doc.Add(table);
        }
    }
}
