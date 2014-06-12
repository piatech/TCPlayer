#region Copyright (c) 2014 Leonid Lezner. All rights reserved.
// Copyright (C) 2013-2014 Leonid Lezner
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

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
