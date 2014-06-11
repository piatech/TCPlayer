// Copyright (c) 2014 Leonid Lezner, PIATECH. All rights reserved. 
// Use of this source code is governed by a MIT license that
// can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPlayer
{
    class TextBoxStreamWriter : TextWriter
    {
        public TextBox OutputTextBox { get; set; }

        System.Timers.Timer _timer = new System.Timers.Timer();

        string _buffer = "";

        public TextBoxStreamWriter()
        {
            OutputTextBox = null;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Interval = 500;
            _timer.Start();
        }

        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (OutputTextBox != null && _buffer.Length > 0)
            {
                OutputTextBox.BeginInvoke(new Action(() =>
                {
                    OutputTextBox.AppendText(_buffer);
                    _buffer = "";
                }));

                
            }
        }

        public override void Write(char value)
        {
            base.Write(value);
            _buffer += value.ToString();
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
