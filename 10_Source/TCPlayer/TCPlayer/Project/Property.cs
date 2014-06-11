using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TCPlayer.Interfaces;

namespace TCPlayer.Project
{
    public class Property : ISerializable
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public XElement Xml
        {
            get
            {
                XElement element = new XElement("Property");
                element.SetAttributeValue("Name", Name);
                element.SetValue(Value);
                return element;
            }
            set
            {
                XElement Element = value;

                if (Element == null || Element.Name != "Property")
                {
                    throw new ProjectSerializationException("Creating Property with wrong XML element");
                }

                XAttribute AttributeName = Element.Attribute("Name");

                if (AttributeName == null || String.IsNullOrEmpty(AttributeName.Value))
                {
                    throw new ProjectSerializationException("Creating Property with no Name attribute");
                }

                Name = AttributeName.Value;

                Value = Element.Value;
            }
        }

        public Property()
        {

        }

        public Property(XElement Xml)
        {
            this.Xml = Xml;
        }

        public Property(string Name, string Value)
        {
            this.Name = Name;
            this.Value = Value;
        }

        public override string ToString()
        {
            return Value;
        }

        public TType GetValue<TType>()
        {
            object retObj;

            switch (typeof(TType).ToString())
            {
                case "System.Boolean":
                    retObj = Utils.ParseBoolean(Value);
                    break;
                case "System.Int32":
                    retObj = int.Parse(Value);
                    break;
                default:
                    retObj = Value;
                    break;
            }

            return (TType)Convert.ChangeType(retObj, typeof(TType));
        }
    }
}
