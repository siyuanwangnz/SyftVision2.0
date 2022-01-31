using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Public.Global
{
    public class Options
    {
        private const string USER = "root";
        private const string PORT = "22";
        private const string PASSWORD = "Syft2002";
        private const string OPERATOR = "Syft";

        public string User { get; private set; }
        public string Port { get; private set; }
        public string Password { get; private set; }
        public string Operator { get; private set; }

        public Options()
        {
            try
            {
                XElement RootNode = XElement.Load($"./Config/Options_Config.xml");

                User = RootNode.Element("User")?.Value;
                Port = RootNode.Element("Port")?.Value;
                Password = RootNode.Element("Password")?.Value;
                Operator = RootNode.Element("Operator")?.Value;
            }
            catch (Exception ex)
            {
                User = USER;
                Port = PORT;
                Password = PASSWORD;
                Operator = OPERATOR;

                throw ex;
            }

        }

        public void Reset()
        {
            User = USER;
            Port = PORT;
            Password = PASSWORD;
            Operator = OPERATOR;

            XElement RootNode = XElement.Load($"./Config/Options_Config.xml");

            RootNode.Element("User")?.SetValue(USER);
            RootNode.Element("Port")?.SetValue(PORT);
            RootNode.Element("Password")?.SetValue(PASSWORD);
            RootNode.Element("Operator")?.SetValue(OPERATOR);

            RootNode.Save($"./Config/Options_Config.xml");
        }

        public void Set(string User, string Port, string Password, string Operator)
        {
            this.User = User;
            this.Port = Port;
            this.Password = Password;
            this.Operator = Operator;

            XElement RootNode = XElement.Load($"./Config/Options_Config.xml");

            RootNode.Element("User")?.SetValue(this.User);
            RootNode.Element("Port")?.SetValue(this.Port);
            RootNode.Element("Password")?.SetValue(this.Password);
            RootNode.Element("Operator")?.SetValue(this.Operator);

            RootNode.Save($"./Config/Options_Config.xml");
        }
    }
}
