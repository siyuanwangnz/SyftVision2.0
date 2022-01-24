using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
using SettingCheck.Services;

namespace SettingCheck.Models
{
    class PDFGenerator
    {
        public static void GenerateHeadPDF(Document doc, string title)
        {
            string CurrentDirectory = Directory.GetCurrentDirectory();

            doc.Add(new Paragraph(new Text("\n")));

            float[] columnWidths = { 1, 2, 1 };
            Table table = new Table(UnitValue.CreatePercentArray(columnWidths));
            table.SetHorizontalAlignment(HorizontalAlignment.CENTER);

            Image image = new Image(ImageDataFactory.Create($"{CurrentDirectory}/Image/Syft Logo.png"));
            image.Scale(0.05f, 0.05f);

            table.AddCell(new Cell(1, 1).Add(image).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell(1, 1).Add(new Paragraph(title)).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetTextAlignment(TextAlignment.CENTER).SetFontSize(20).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell(1, 1).Add(new Paragraph($"Date: {DateTime.Now.ToString("yyyy/MM/dd")}\r\nTime: {DateTime.Now.ToString("HH:mm:ss")}")).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetBorder(Border.NO_BORDER));

            doc.Add(table);

            doc.Add(new Paragraph(new Text("\n")));

            doc.Add(new LineSeparator(new DottedLine(1, 2)).SetMarginTop(-4));
        }

        public static void GenerateInfoPDF(Document doc, XElement SavedScanRootNood)
        {
            doc.Add(new Paragraph(new Text("\n")));

            float[] columnWidths = { 2, 1 };
            Table table = new Table(UnitValue.CreatePercentArray(columnWidths));
            table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            table.SetTextAlignment(TextAlignment.CENTER);
            table.SetWidth(400);
            table.SetFontSize(8);

            table.AddCell(new Cell(1, 1).Add(new Paragraph($"Operator Name")));
            table.AddCell(new Cell(1, 1).Add(new Paragraph($"{Global.UserName}")));

            table.AddCell(new Cell(1, 1).Add(new Paragraph($"Instrument Model")));
            table.AddCell(new Cell(1, 1).Add(new Paragraph($"{GetSetting.Content(SavedScanRootNood, "instrument.model")}")));

            table.AddCell(new Cell(1, 1).Add(new Paragraph($"Instrument Number")));
            table.AddCell(new Cell(1, 1).Add(new Paragraph($"{GetSetting.Content(SavedScanRootNood, "instrument.name")}")));

            table.AddCell(new Cell(1, 1).Add(new Paragraph($"Instrument Serial Number")));
            table.AddCell(new Cell(1, 1).Add(new Paragraph($"{GetSetting.Content(SavedScanRootNood, "instrument.serial")}")));

            doc.Add(table);

            doc.Add(new Paragraph(new Text("\n")));
        }

        public static void GenerateChartPDF(Document doc, string title, bool firstPage, string psPath, string mvPath, string meshPath,
            string powWetPath, string negWetPath, string negDryPath, string dwsPath, string dwsRFPath, string dwsABPath, string dwsLens5Path,
            string dvPath, string dsPath, string stPath, string icfPath)
        {
            //PdfDocument pdf = new PdfDocument(new PdfWriter($"{pdfFilePath}"));

            //Document doc = new Document(pdf, PageSize.A4);
            //doc.SetMargins(10, 10, 10, 10);
            if(!firstPage)
                doc.Add(new AreaBreak());

            Table table = new Table(new float[2]).UseAllAvailableWidth();

            // Title cell
            Cell cellTitle = new Cell(1, 2).Add(new Paragraph(title));
            cellTitle.SetTextAlignment(TextAlignment.CENTER);
            cellTitle.SetPadding(5);
            cellTitle.SetBackgroundColor(new DeviceRgb(140, 221, 8));
            table.AddCell(cellTitle);

            Image img1;
            Image img2;
            Image img3;
            //Source Image cell
            Paragraph pSource = new Paragraph();
            if (psPath != "")
            {
                img1 = new Image(ImageDataFactory.Create($"{psPath}")).Scale(0.25f, 0.25f);
                pSource.Add(img1);
            }
            if (mvPath != "")
            {
                img2 = new Image(ImageDataFactory.Create($"{mvPath}")).Scale(0.25f, 0.25f);
                pSource.Add(img2);
            }
            if (meshPath != "")
            {
                img3 = new Image(ImageDataFactory.Create($"{meshPath}")).Scale(0.25f, 0.25f);
                pSource.Add(img3);
            }
            table.AddCell(pSource);

            //PosWet NegWet NegDry Image cell
            Cell cellPosWet = new Cell();
            Cell cellNegWet = new Cell();
            Cell cellNegDry = new Cell();
            if (powWetPath != "")
            {
                img1 = new Image(ImageDataFactory.Create($"{powWetPath}")).Scale(0.25f, 0.25f);
                cellPosWet.Add(img1);
            }
            if (negWetPath != "")
            {
                img2 = new Image(ImageDataFactory.Create($"{negWetPath}")).Scale(0.25f, 0.25f);
                cellNegWet.Add(img2);
            }
            if (negDryPath != "")
            {
                img3 = new Image(ImageDataFactory.Create($"{negDryPath}")).Scale(0.25f, 0.25f);
                cellNegDry.Add(img3);
            }
            table.AddCell(cellPosWet);
            table.AddCell(cellNegWet);
            table.AddCell(cellNegDry);

            //DWS Image cell
            Cell cellDWS = new Cell();
            if (dwsPath != "")
            {
                img1 = new Image(ImageDataFactory.Create($"{dwsPath}")).Scale(0.25f, 0.25f);
                cellDWS.Add(img1);
            }
            table.AddCell(cellDWS);

            //DWS Specific Image cell
            Cell cellDWSSpecific = new Cell();
            if (dwsRFPath != "")
            {
                img1 = new Image(ImageDataFactory.Create($"{dwsRFPath}")).Scale(0.25f, 0.25f);
                cellDWSSpecific.Add(img1);
            }
            if (dwsABPath != "")
            {
                img2 = new Image(ImageDataFactory.Create($"{dwsABPath}")).Scale(0.25f, 0.25f);
                cellDWSSpecific.Add(img2);
            }
            if (dwsLens5Path != "")
            {
                img3 = new Image(ImageDataFactory.Create($"{dwsLens5Path}")).Scale(0.25f, 0.25f);
                cellDWSSpecific.Add(img3);
            }
            table.AddCell(cellDWSSpecific);

            //Detection Image cell
            Paragraph pDetection = new Paragraph();
            if (dvPath != "")
            {
                img1 = new Image(ImageDataFactory.Create($"{dvPath}")).Scale(0.25f, 0.25f);
                pDetection.Add(img1);
            }
            if (dsPath != "")
            {
                img2 = new Image(ImageDataFactory.Create($"{dsPath}")).Scale(0.25f, 0.25f);
                pDetection.Add(img2);
            }
            if (stPath != "")
            {
                img3 = new Image(ImageDataFactory.Create($"{stPath}")).Scale(0.25f, 0.25f);
                pDetection.Add(img3);
            }
            table.AddCell(pDetection);
            //ICF Image cell
            Cell cellICF = new Cell();
            if (icfPath != "")
            {
                img1 = new Image(ImageDataFactory.Create($"{icfPath}")).Scale(0.25f, 0.25f);
                cellICF.Add(img1);
            }
            table.AddCell(cellICF);

            doc.Add(table);
        }

        public static void GenerateTablePDF(Document doc,
            SourceSettings sourceSettings,
            UPSPhaseSettings posWetPhaseSettings,
            UPSPhaseSettings negWetPhaseSettings,
            UPSPhaseSettings negDryPhaseSettings,
            DWSSettings dwsSettings,
            DWSSpecificSettings dwsSpecificSettings,
            DetectionSettings detectionSettings)
        {
            doc.Add(GenerateSourceTablePDF(sourceSettings));
            doc.Add(new Paragraph(new Text("\n")).SetHeight(3));
            doc.Add(GenerateSelectionTablePDF(posWetPhaseSettings, negWetPhaseSettings, negDryPhaseSettings));
            doc.Add(new Paragraph(new Text("\n")).SetHeight(3));
            doc.Add(GenerateDWSTablePDF(dwsSettings));
            doc.Add(new Paragraph(new Text("\n")).SetHeight(3));
            doc.Add(GenerateDWSSpecificTablePDF(dwsSpecificSettings));
            doc.Add(new Paragraph(new Text("\n")).SetHeight(3));
            doc.Add(GenerateDetectionTablePDF(detectionSettings));
        }

        private static Table GenerateSourceTablePDF(SourceSettings sourceSettings)
        {
            float[] columnWidths = { 2, 2, 1, 2, 1 };
            Table table = new Table(UnitValue.CreatePercentArray(columnWidths));
            table.SetWidth(500);
            table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            table.SetTextAlignment(TextAlignment.CENTER);
            table.SetFontSize(6);

            // Title cell
            Cell cellTitle = new Cell(1, 5).Add(new Paragraph("Source&Vacuum"));
            cellTitle.SetTextAlignment(TextAlignment.CENTER);
            cellTitle.SetPadding(4);
            cellTitle.SetBackgroundColor(new DeviceRgb(140, 221, 8));
            table.AddCell(cellTitle);

            //Source pressure cell
            if (sourceSettings.Pressure != null)
            {
                table.AddCell(new Cell(sourceSettings.Pressure.SourPhaseList.Count, 1).Add(new Paragraph("Source Pressure")).SetVerticalAlignment(VerticalAlignment.MIDDLE));

                foreach (var item in sourceSettings.Pressure.SourPhaseList)
                {
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"{item.Name}")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"{item.SettingValue}")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"({item.MinimumValue}, {item.MaximumValue})")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(GetColoredStatusText(item.Color))));
                }
            }

            //MV cell
            if (sourceSettings.MV != null)
            {
                table.AddCell(new Cell(sourceSettings.MV.SourPhaseList.Count, 1).Add(new Paragraph("MV")).SetVerticalAlignment(VerticalAlignment.MIDDLE));

                foreach (var item in sourceSettings.MV.SourPhaseList)
                {
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"{item.Name}")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"{item.SettingValue}")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"({item.MinimumValue}, {item.MaximumValue})")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(GetColoredStatusText(item.Color))));
                }
            }

            //Mesh Voltage cell
            if (sourceSettings.Mesh != null)
            {
                table.AddCell(new Cell(sourceSettings.Mesh.SourPhaseList.Count, 1).Add(new Paragraph("Mesh Voltage")).SetVerticalAlignment(VerticalAlignment.MIDDLE));

                foreach (var item in sourceSettings.Mesh.SourPhaseList)
                {
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"{item.Name}")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"{item.SettingValue}")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"({item.MinimumValue}, {item.MaximumValue})")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(GetColoredStatusText(item.Color))));
                }
            }

            return table;
        }

        private static Table GenerateSelectionTablePDF(UPSPhaseSettings posWetPhaseSettings, UPSPhaseSettings negWetPhaseSettings, UPSPhaseSettings negDryPhaseSettings)
        {
            float[] columnWidths = { 2, 2, 1, 2, 1 };
            Table table = new Table(UnitValue.CreatePercentArray(columnWidths));
            table.SetWidth(500);
            table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            table.SetTextAlignment(TextAlignment.CENTER);
            table.SetFontSize(6);

            // Title cell
            Cell cellTitle = new Cell(1, 5).Add(new Paragraph("Selection"));
            cellTitle.SetTextAlignment(TextAlignment.CENTER);
            cellTitle.SetPadding(4);
            cellTitle.SetBackgroundColor(new DeviceRgb(140, 221, 8));
            table.AddCell(cellTitle);

            //PosWet cell
            if (posWetPhaseSettings != null)
            {
                table.AddCell(new Cell(posWetPhaseSettings.UPSPhaseList.Count, 1).Add(new Paragraph("PosWet")).SetVerticalAlignment(VerticalAlignment.MIDDLE));

                foreach (var item in posWetPhaseSettings.UPSPhaseList)
                {
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"{item.Name}")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"{item.SettingValue}")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"({item.MinimumValue}, {item.MaximumValue})")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(GetColoredStatusText(item.Color))));
                }
            }

            //NegWet cell
            if (negWetPhaseSettings != null)
            {
                table.AddCell(new Cell(negWetPhaseSettings.UPSPhaseList.Count, 1).Add(new Paragraph("NegWet")).SetVerticalAlignment(VerticalAlignment.MIDDLE));

                foreach (var item in negWetPhaseSettings.UPSPhaseList)
                {
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"{item.Name}")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"{item.SettingValue}")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"({item.MinimumValue}, {item.MaximumValue})")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(GetColoredStatusText(item.Color))));
                }
            }

            //NegDry cell
            if (negDryPhaseSettings != null)
            {
                table.AddCell(new Cell(negDryPhaseSettings.UPSPhaseList.Count, 1).Add(new Paragraph("NegDry")).SetVerticalAlignment(VerticalAlignment.MIDDLE));

                foreach (var item in negDryPhaseSettings.UPSPhaseList)
                {
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"{item.Name}")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"{item.SettingValue}")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"({item.MinimumValue}, {item.MaximumValue})")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(GetColoredStatusText(item.Color))));
                }
            }
            return table;
        }

        private static Table GenerateDWSTablePDF(DWSSettings dwsSettings)
        {
            float[] columnWidths = { 2, 2, 1, 2, 1 };
            Table table = new Table(UnitValue.CreatePercentArray(columnWidths));
            table.SetWidth(500);
            table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            table.SetTextAlignment(TextAlignment.CENTER);
            table.SetFontSize(6);

            // Title cell
            Cell cellTitle = new Cell(1, 5).Add(new Paragraph("Detection DWS"));
            cellTitle.SetTextAlignment(TextAlignment.CENTER);
            cellTitle.SetPadding(4);
            cellTitle.SetBackgroundColor(new DeviceRgb(140, 221, 8));
            table.AddCell(cellTitle);

            //DWS cell
            if (dwsSettings != null)
            {
                table.AddCell(new Cell(dwsSettings.DWSList.Count, 1).Add(new Paragraph("All Phase")).SetVerticalAlignment(VerticalAlignment.MIDDLE));

                foreach (var item in dwsSettings.DWSList)
                {
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"{item.Name}")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"{item.SettingValue}")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"({item.MinimumValue}, {item.MaximumValue})")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(GetColoredStatusText(item.Color))));
                }
            }
            return table;
        }

        private static Table GenerateDWSSpecificTablePDF(DWSSpecificSettings dwsSpecificSettings)
        {
            float[] columnWidths = { 2, 2, 1, 2, 1 };
            Table table = new Table(UnitValue.CreatePercentArray(columnWidths));
            table.SetWidth(500);
            table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            table.SetTextAlignment(TextAlignment.CENTER);
            table.SetFontSize(6);

            // Title cell
            Cell cellTitle = new Cell(1, 5).Add(new Paragraph("Detection DWS Specific"));
            cellTitle.SetTextAlignment(TextAlignment.CENTER);
            cellTitle.SetPadding(4);
            cellTitle.SetBackgroundColor(new DeviceRgb(140, 221, 8));
            table.AddCell(cellTitle);

            //Prefilter cell
            if (dwsSpecificSettings.Prefilter != null)
            {
                table.AddCell(new Cell(dwsSpecificSettings.Prefilter.MassList_Pos.Count, 1).Add(new Paragraph("Prefilter")).SetVerticalAlignment(VerticalAlignment.MIDDLE));

                foreach (var item in dwsSpecificSettings.Prefilter.MassList_Pos)
                {
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"{item.Name}")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"{item.SettingValue}")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"({item.MinimumValue}, {item.MaximumValue})")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(GetColoredStatusText(item.Color))));
                }
            }

            //AB cell
            if (dwsSpecificSettings.Axial_Bias != null)
            {
                table.AddCell(new Cell(dwsSpecificSettings.Axial_Bias.MassList_Pos.Count, 1).Add(new Paragraph("Axial Bias")).SetVerticalAlignment(VerticalAlignment.MIDDLE));

                foreach (var item in dwsSpecificSettings.Axial_Bias.MassList_Pos)
                {
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"{item.Name}")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"{item.SettingValue}")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"({item.MinimumValue}, {item.MaximumValue})")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(GetColoredStatusText(item.Color))));
                }
            }

            //Lens 5 cell
            if (dwsSpecificSettings.Lens_5 != null)
            {
                table.AddCell(new Cell(dwsSpecificSettings.Lens_5.MassList_Pos.Count, 1).Add(new Paragraph("Lens 5")).SetVerticalAlignment(VerticalAlignment.MIDDLE));

                foreach (var item in dwsSpecificSettings.Lens_5.MassList_Pos)
                {
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"{item.Name}")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"{item.SettingValue}")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"({item.MinimumValue}, {item.MaximumValue})")));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph(GetColoredStatusText(item.Color))));
                }
            }
            return table;
        }

        private static Table GenerateDetectionTablePDF(DetectionSettings detectionSettings)
        {
            float[] columnWidths = { 2, 2, 1, 2, 1 };
            Table table = new Table(UnitValue.CreatePercentArray(columnWidths));
            table.SetWidth(500);
            table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            table.SetTextAlignment(TextAlignment.CENTER);
            table.SetFontSize(6);

            // Title cell
            Cell cellTitle = new Cell(1, 5).Add(new Paragraph("Detection"));
            cellTitle.SetTextAlignment(TextAlignment.CENTER);
            cellTitle.SetPadding(4);
            cellTitle.SetBackgroundColor(new DeviceRgb(140, 221, 8));
            table.AddCell(cellTitle);

            //Detection cell
            table.AddCell(new Cell(3, 1).Add(new Paragraph("All Phase")).SetVerticalAlignment(VerticalAlignment.MIDDLE));

            if (detectionSettings.Detector_Voltage != null)
            {
                table.AddCell(new Cell(1, 1).Add(new Paragraph($"{detectionSettings.Detector_Voltage.Setting.Name}")));
                table.AddCell(new Cell(1, 1).Add(new Paragraph($"{detectionSettings.Detector_Voltage.Setting.SettingValue}")));
                table.AddCell(new Cell(1, 1).Add(new Paragraph($"({detectionSettings.Detector_Voltage.Setting.MinimumValue}, {detectionSettings.Detector_Voltage.Setting.MaximumValue})")));
                table.AddCell(new Cell(1, 1).Add(new Paragraph(GetColoredStatusText(detectionSettings.Detector_Voltage.Setting.Color))));
            }
            if (detectionSettings.Discriminator != null)
            {
                table.AddCell(new Cell(1, 1).Add(new Paragraph($"{detectionSettings.Discriminator.Setting.Name}")));
                table.AddCell(new Cell(1, 1).Add(new Paragraph($"{detectionSettings.Discriminator.Setting.SettingValue}")));
                table.AddCell(new Cell(1, 1).Add(new Paragraph($"({detectionSettings.Discriminator.Setting.MinimumValue}, {detectionSettings.Discriminator.Setting.MaximumValue})")));
                table.AddCell(new Cell(1, 1).Add(new Paragraph(GetColoredStatusText(detectionSettings.Discriminator.Setting.Color))));
            }
            if (detectionSettings.Settle_Time != null)
            {
                table.AddCell(new Cell(1, 1).Add(new Paragraph($"{detectionSettings.Settle_Time.Setting.Name}")));
                table.AddCell(new Cell(1, 1).Add(new Paragraph($"{detectionSettings.Settle_Time.Setting.SettingValue}")));
                table.AddCell(new Cell(1, 1).Add(new Paragraph($"({detectionSettings.Settle_Time.Setting.MinimumValue}, {detectionSettings.Settle_Time.Setting.MaximumValue})")));
                table.AddCell(new Cell(1, 1).Add(new Paragraph(GetColoredStatusText(detectionSettings.Settle_Time.Setting.Color))));
            }
            return table;
        }

        private static Text GetColoredStatusText(int color)
        {
            Text text = new Text(color == 0x80ff80 || color == unchecked((int)0x8080ff80) ? "Pass" : "Fail");
            text.SetFontColor(color == 0x80ff80 || color == unchecked((int)0x8080ff80) ? ColorConstants.GREEN : ColorConstants.RED);
            return text;
        }
    }
}
