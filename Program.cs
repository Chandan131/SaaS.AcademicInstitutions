using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessManager;
using Entity;
using System.Xml;
using System.Xml.XPath;

namespace TestThreading
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessManager.ProcessManager.GetCurrentInstance();
            string datas = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Input><From>Source CRM</From><Entity name=\"opportunity\"><Action>Update</Action><PKSourceField name=\"opportunityid\">4a7b0cf9-dc0b-e111-aac0-001cc4a9ce7c</PKSourceField><PKDestinationField name=\"new_sfdcsalesopportunityid\">0065000000IsUqAAAV</PKDestinationField><Data><Field name=\"new_clientquotenumber\">QuoteNo2</Field><Field name=\"IsTransForm\">false</Field><Field name=\"OwnerID\"></Field><Field name=\"StageName\">Contacted</Field></Data></Entity></Input>";
            XMLParsing ParseXml = new XMLParsing(datas);
            SyncEntity Entity = ParseXml.Entity;
            DoTask(ParseXml.Entity);
           
        }

        private static void DoTask(SyncEntity Task)
        {
            ProcessManager.ProcessManager.GetCurrentInstance().AddTask(Task);
        }
    }
    public enum Action
    {
        Add, Update, Delete, Updateasset, query, Insert, BulkUpdate, StageUpdate
    }
    public class XMLParsing
    {
        public SyncEntity Entity;
        public XMLParsing(string Xml)
        {

            XmlDocument Doc = new XmlDocument();
            Doc.LoadXml(Xml);
            Entity = new SyncEntity();
            XPathNavigator Nav = Doc.CreateNavigator();
            GetEntityAttribute(Nav, Entity);
            GetFields(Nav, Entity);
            GetChild(Nav, Entity);
            //return Entity;
        }
        public void GetChild(XPathNavigator Nav, SyncEntity Entity)
        {

            XPathExpression Expr;
            Expr = Nav.Compile("Input/Entity/Childs/Child");
            XPathNodeIterator Fiterator = Nav.Select(Expr);
            while (Fiterator.MoveNext())
            {
                SyncEntity ScnChild = new SyncEntity();
                GetChildEntityAttribute(Fiterator.Current, ScnChild);
                Entity.ChildEntity.Add(ScnChild);

            }
        }
        private void GetFields(XPathNavigator Nav, SyncEntity Entity)
        {

            XPathExpression Expr;
            Expr = Nav.Compile("Input/Entity/Data/Field");
            XPathNodeIterator Fiterator = Nav.Select(Expr);
            while (Fiterator.MoveNext())
            {
                XPathNavigator CurrentField = Fiterator.Current.Clone();
                Field TempField = new Field();
                TempField.Value = CurrentField.Value;
                TempField.Name = CurrentField.GetAttribute("name", "");
               // TempField.IsTransform = Convert.ToBoolean(CurrentField.GetAttribute("IsTransForm", ""));
                //tempField.IsChildline = Convert.ToBoolean(currentField.GetAttribute("IsChildLine", ""));
                Entity.Fields.Add(TempField);

            }

        }
        private void GetChildFields(XPathNavigator Nav, SyncEntity Entity)
        {
            Nav.MoveToParent();
            XPathExpression XPathExpr;
            XPathExpr = Nav.Compile("Data/Field");
            XPathNodeIterator Fiterator = Nav.Select(XPathExpr);
            while (Fiterator.MoveNext())
            {
                XPathNavigator CurrentField = Fiterator.Current.Clone();
                Field TempField = new Field();
                TempField.Value = CurrentField.Value;
                TempField.Name = CurrentField.GetAttribute("name", "");
                //tempField.IsChildline = Convert.ToBoolean(currentField.GetAttribute("IsChildLine", ""));
                Entity.Fields.Add(TempField);

            }

        }
        private void GetChildEntityAttribute(XPathNavigator Nav, SyncEntity Entity)
        {

            XPathNodeIterator XpathFrom = Nav.Select(Nav.Compile("From"));
            if (XpathFrom.Current.HasChildren)
            {
                Entity.From = XpathFrom.Current.SelectSingleNode("From").Value;
                //From
                XPathNodeIterator XpathEntity = Nav.Select("Entity");
                XpathEntity.MoveNext();

                Entity.Name = XpathEntity.Current.GetAttribute("name", "");

                XpathEntity.Current.MoveToChild("Action", "");
                switch (XpathEntity.Current.Value.ToLower())
                {
                    case "insert":
                        Entity.OperationAction = Entity.OperationAction;
                        break;
                   
                }

                XpathEntity.Current.MoveToParent();
                XpathEntity.Current.MoveToChild("PKSourceField", "");
                Entity.PKSourceID = XpathEntity.Current.Value;
                Entity.PKSourceFieldName = XpathEntity.Current.GetAttribute("name", "");
                XpathEntity.Current.MoveToParent();
                XpathEntity.Current.MoveToChild("PKDestinationField", "");
                Entity.PKDestinationFieldName = XpathEntity.Current.GetAttribute("name", "");
                Entity.PKDestinationID = XpathEntity.Current.Value;
                GetChildFields(XpathEntity.Current, Entity);
            }
        }
        private void GetEntityAttribute(XPathNavigator Nav, SyncEntity Entity)
        {

            XPathNodeIterator XpathFrom = Nav.Select(Nav.Compile("Input"));
            XpathFrom.MoveNext();
            Entity.From = XpathFrom.Current.SelectSingleNode("From").Value;

            XPathNodeIterator XpathEntity = Nav.Select("Input/Entity");
            XpathEntity.MoveNext();

            Entity.Name = XpathEntity.Current.GetAttribute("name", "");
            XpathEntity.Current.MoveToChild("Action", "");
            switch (XpathEntity.Current.Value.ToLower())
            {
                case "insert":

                    Entity.OperationAction = Entity.OperationAction = Entity.OperationAction;;
                    break;
            }

            XpathEntity.Current.MoveToParent();
            XpathEntity.Current.MoveToChild("PKSourceField", "");
            Entity.PKSourceID = XpathEntity.Current.Value;
            Entity.PKSourceFieldName = XpathEntity.Current.GetAttribute("name", "");
            XpathEntity.Current.MoveToParent();
            XpathEntity.Current.MoveToChild("PKDestinationField", "");
            Entity.PKDestinationFieldName = XpathEntity.Current.GetAttribute("name", "");
            Entity.PKDestinationID = XpathEntity.Current.Value;
        }
    }
}
