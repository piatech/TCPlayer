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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TCPlayer
{
    class Utils
    {
        internal static string GetApplicationBasePath()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// Parsing the string containing a boolean value like "true", "false", "1", "0", "yes", "no"
        /// </summary>
        internal static bool ParseBoolean(string BooleanString)
        {
            BooleanString = BooleanString.ToLower();
            string[] trueValues = new string[] { "1", "true", "yes", "y" };
            foreach (string trueValue in trueValues)
            {
                if (trueValue == BooleanString)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Getting the boolean attribute of a xml element
        /// </summary>
        /// <param name="Element">XElement object</param>
        /// <param name="Name">Attribute's name</param>
        /// <param name="Default">Default boolean value</param>
        /// <returns></returns>
        internal static bool GetXBoolAttribute(XElement Element, string Name, bool Default = false)
        {
            XAttribute Attribute = Element.Attribute(Name);

            if (Attribute == null || String.IsNullOrEmpty(Attribute.Value))
            {
                return Default;
            }
            else
            {
                return Utils.ParseBoolean(Attribute.Value);
            }
        }


        /// <summary>
        /// Getting the string attribute of a xml element
        /// </summary>
        /// <param name="Element">XElement object</param>
        /// <param name="Name">Attribute's name</param>
        /// <param name="Default">Default string value</param>
        /// <returns></returns>
        internal static string GetXStringAttribute(XElement Element, string Name, string Default = "")
        {
            XAttribute Attribute = Element.Attribute(Name);

            if (Attribute == null || String.IsNullOrEmpty(Attribute.Value))
            {
                return Default;
            }
            else
            {
                return Attribute.Value;
            }
        }

        internal static void CallWithTimeOut(Action callAction, Action failAction, int timeoutInMilliseconds)
        {
            Task tempTask = Task.Factory.StartNew(() => callAction());

            tempTask.Wait(timeoutInMilliseconds);

            if(!tempTask.IsCompleted)
            {
                failAction();
            }
        }
    }
}
