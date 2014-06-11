// Copyright (c) 2014 Leonid Lezner, PIATECH. All rights reserved. 
// Use of this source code is governed by a MIT license that
// can be found in the LICENSE file.

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
