using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Entity
{
    public enum Action
    {
        Add, Update, Delete, Updateasset, query, Insert, BulkUpdate, StageUpdate
    }
    public class SyncEntity : IComparable<SyncEntity>
    {
        public string Name { get; set; }
        public string PKSourceID { get; set; }
        public string PKDestinationID { get; set; }
        public string PKSourceFieldName { get; set; }
        public string PKDestinationFieldName { get; set; }
        public string From { get; set; }
        public Action OperationAction { get; set; }
        public List<Field> Fields;
        public string RequestID { get; set; }
        public string ResultComment { get; set; }
        public bool ResultStatus { get; set; }
        public string ResultStage { get; set; }
        public Guid BatchID { get; set; }
        public Nullable<DateTime> ResponseTime { get; set; }
        public string SyncID { get; set; }
        public List<SyncEntity> ChildEntity { get; set; }
        public bool IsChild { get; set; }
        public string GetKey()
        {
            return Name + OperationAction.ToString();
        }
        public string GetIdentifier()
        {
            return PKSourceID;
        }
        public int CompareTo(SyncEntity Other)
        {

            if ((Other.Name == Name) && (Other.OperationAction == OperationAction))
                return 1;
            else
                return 0;
        }
        public string GetFieldVal(string Name)
        {
            foreach (Field Fd in Fields)
            {
                if (Fd.Name == Name)
                    return Fd.Value;
            }
            return "";
        }

        public SyncEntity()
        {
            Fields = new List<Field>();
            ChildEntity = new List<SyncEntity>();
        }
        #region old code need to see this
        //public bool IsValid()
        //{
        //    //if (this.OperationAction == Action.Update || this.OperationAction == Action.StageUpdate || this.OperationAction == Action.StageUpdate)
        //    //{
        //    //    if (string.IsNullOrEmpty(this.PKDestinationID))
        //    //        return false;
        //    //    foreach (Field Fie in Fields)
        //    //    {
        //    //        //if (Fie.Name == "ownerid" && string.IsNullOrEmpty(Fie.Value))//Temp fix for Data manager remove once plugin start sending it
        //    //        //{
        //    //        //  Fie.Value = "00570000001JPxS"; 
        //    //        //}

        //    //        if (string.IsNullOrEmpty(Fie.Value))
        //    //            return false;
        //    //    }
        //    //    return true;
        //    //}
        //    ////else
        //    if (this.OperationAction == Action.Add)
        //    {
        //        foreach (Field Fie in Fields)
        //        {
        //            if (Fie.Name == "ownerid")
        //            {
        //                if (string.IsNullOrEmpty(Fie.Value))
        //                    return false;
        //            }

        //        }
        //        return true;
        //    }
        //    else
        //    {
        //        if (string.IsNullOrEmpty(this.PKDestinationID))
        //            return false;
        //        foreach (Field Fie in Fields)
        //        {
        //           if (string.IsNullOrEmpty(Fie.Value))
        //                return false;
        //        }
        //        return true;
        //    }
        //    //return true;
        //}
        #endregion
        public bool IsValid()
        {
            if (this.OperationAction == Action.Update)
            {
                if (string.IsNullOrEmpty(this.PKDestinationID))
                    return false;
                foreach (Field Fie in Fields)
                {
                    //if (Fie.Name == "ownerid" && string.IsNullOrEmpty(Fie.Value))//Temp fix for Data manager remove once plugin start sending it
                    //{
                    //  Fie.Value = "00570000001JPxS"; 
                    //}

                    if (string.IsNullOrEmpty(Fie.Value))
                        return false;
                }
                return true;
            }
            return true;
        }

        public string ToXml()
        {
            StringBuilder XmlBuilder = new StringBuilder();
            if (IsChild == false)
                XmlBuilder.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?><Input>");
            else
                XmlBuilder.Append("<Child>");
            XmlBuilder.Append("<From>" + From + "</From>");
            XmlBuilder.Append("<Entity name=\"" + Name + "\">");
            XmlBuilder.Append("<Action>" + OperationAction.ToString() + "</Action>");
            XmlBuilder.Append("<PKSourceField name=\"opportunityid\">" + this.PKSourceID + "</PKSourceField><PKDestinationField name=\"new_sfdcrenewalopportunityid\">" + this.PKDestinationID + "</PKDestinationField>");
            XmlBuilder.Append("<Data>");
            foreach (Field Childfield in Fields)
            {
                XmlBuilder.Append(Childfield.ToXml());
            }
            XmlBuilder.Append("</Data>");
            XmlBuilder.Append("<Childs>");
            foreach (SyncEntity Ch in ChildEntity)
            {
                XmlBuilder.Append(Ch.ToXml());
            }
            XmlBuilder.Append("</Childs>");
            XmlBuilder.Append("</Entity>");
            if (IsChild == false)
                XmlBuilder.Append("</Input>");
            else
                XmlBuilder.Append("</Child>");
            return XmlBuilder.ToString();

        }
    }
    public class Field
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public Boolean IsTransform { get; set; }
        // public Boolean IsChildline { get; set; }
        public string ToXml()
        {
            return "<Field name=\"" + Name + "\" IsTransForm=\"" + IsTransform.ToString() + "\">" + Value + "</Field>";
        }
    }
    public class Task
    {
        public SyncEntity Entity { get; set; }
    }


}
