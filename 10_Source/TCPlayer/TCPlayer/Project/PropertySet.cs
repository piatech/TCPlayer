using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TCPlayer.Interfaces;

namespace TCPlayer.Project
{
    public class PropertySet : ISerializable
    {
        private Dictionary<string, Property> _properties = new Dictionary<string, Property>();

        public event EventHandler Changed;
        
        public XElement Xml
        {
            get
            {
                if (_properties.Count < 1)
                {
                    return null;
                }

                XElement element = new XElement("Properties");
                
                foreach (Property property in this)
                {
                    element.Add(property.Xml);
                }
                return element;
            }
            set
            {
                XElement Element = value;

                if (Element == null || Element.Name != "Properties")
                {
                    throw new ProjectSerializationException("Creating Property Set with wrong XML element");
                }

                var PropertyElements = Element.Elements("Property");

                foreach (XElement PropElement in PropertyElements)
                {
                    Property property = new Property(PropElement);
                    _properties.Add(property.Name, property);
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            foreach (KeyValuePair<String, Property> entry in _properties)
            {
                yield return entry.Value;
            }
        }

        public Property this[string Name, string DefaultValue = ""]
        {
            get
            {
                try
                {
                    return _properties[Name];
                }
                catch (KeyNotFoundException)
                {
                    Property newProperty = new Property();
                    newProperty.Name = Name;
                    newProperty.Value = DefaultValue;
                    
                    _properties.Add(Name, newProperty);

                    if(Changed != null)
                    {
                        Changed(newProperty, null);
                    }
                    
                    return newProperty;
                }
            }
            set
            {
                try
                {
                    string oldValue = _properties[Name].Value;

                    _properties[Name] = value;
                    
                    if (Changed != null && _properties[Name].Value != oldValue)
                    {
                        Changed(_properties[Name], null);
                    }
                }
                catch (KeyNotFoundException)
                {
                    _properties.Add(Name, value);

                    if (Changed != null)
                    {
                        Changed(_properties[Name], null);
                    }
                }
            }
        }
                
        public PropertySet() { }

        public PropertySet(XElement Xml)
        {
            this.Xml = Xml;
        }

        public static PropertySet FromParentXmlElement(XElement Parent)
        {
            var PropertiesElement = Parent.Element("Properties");

            if (PropertiesElement != null)
            {
                return new PropertySet(PropertiesElement);
            }
            else
            {
                return new PropertySet();
            }
        }
    }
}
