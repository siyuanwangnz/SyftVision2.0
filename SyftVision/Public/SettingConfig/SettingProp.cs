﻿using Public.Instrument;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Public.SettingConfig
{
    public class SettingProp
    {
        public SettingProp(string title, string subTitle, List<FilterOff> filterOffList, List<Setting> settingList)
        {
            Tittle = title;
            SubTittle = subTitle;
            FilterOffList = filterOffList.Where(a => a.Wildcard != "").ToList();
            SettingList = settingList.Where(a => !a.IsInvalid()).ToList();
        }
        public SettingProp(XElement rootNode)
        {
            try
            {
                Tittle = rootNode.Attribute("Tittle").Value;
                SubTittle = rootNode.Attribute("SubTittle").Value;

                FilterOffList = new List<FilterOff>();
                foreach (var wildcard in rootNode.Element("FilterOff").Elements("Wildcard"))
                {
                    FilterOff filterOff = new FilterOff();
                    filterOff.Wildcard = wildcard.Value;
                    FilterOffList.Add(filterOff);
                }

                SettingList = new List<Setting>();
                foreach (var setting in rootNode.Elements("Setting"))
                {
                    SettingList.Add(new Setting(setting));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string Tittle { get; private set; }
        public string SubTittle { get; private set; }
        public string Name => $"{Tittle}-{SubTittle}";
        public string File => $"{SubTittle}.xml";
        public bool HasSet { get; private set; } = false;
        public List<FilterOff> FilterOffList { get; private set; }
        public List<Setting> SettingList { get; private set; }
        public XElement XMLGeneration()
        {
            XElement rootNode = new XElement("Settings",
                new XAttribute("Tittle", Tittle),
                new XAttribute("SubTittle", SubTittle));

            if (FilterOffList != null)
            {
                XElement filterOffElement = new XElement("FilterOff");

                foreach (var filterOff in FilterOffList)
                    filterOffElement.Add(new XElement("Wildcard", filterOff.Wildcard));

                rootNode.Add(filterOffElement);
            }

            if (SettingList != null)
            {
                foreach (var setting in SettingList)
                {
                    rootNode.Add(setting.XMLGeneration());
                }
            }

            return rootNode;
        }
        public void SetContentForSettingList(XElement rootNode, ScanFile scanFile)
        {
            List<Setting> newSettingList = Setting.GetSettingList(rootNode, FilterOffList);
            foreach (var setting in SettingList)
            {
                Setting newSetting;
                try
                {
                    newSetting = newSettingList.Single(a => a.Name == setting.Name);
                }
                catch (Exception)
                {
                    continue;
                }
                setting.SetContent(newSetting.ContentList[0], scanFile);
            }
            HasSet = true;
        }
        public void AddContentForSettingList(XElement rootNode, ScanFile scanFile)
        {
            List<Setting> newSettingList = Setting.GetSettingList(rootNode, FilterOffList);
            foreach (var setting in SettingList)
            {
                Setting newSetting;
                try
                {
                    newSetting = newSettingList.Single(a => a.Name == setting.Name);
                }
                catch (Exception)
                {
                    continue;
                }
                setting.AddContent(newSetting.ContentList[0], scanFile);
            }
        }
    }
}
