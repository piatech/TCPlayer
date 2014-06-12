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
using TCPlayer.API.Views;
using TCPlayer.API;

namespace TCPlayer.Project
{
    class DynViewTimer : IViewTimer, IDisposable
    {
        private bool _isDisposed = false;
        private System.Timers.Timer _timer;
        private IViewHost _pluginHost;
        private bool _isBusy = false;

        private bool _startedByUser = false;

        public bool RunOnlyWhenOnline { get; set; }
        public bool RunOnlyWhenVisible { get; set; }

        public event EventHandler Tick;
        public double Interval
        {
            get
            {
                return _timer.Interval;
            }
            set
            {
                _timer.Interval = value;
            }
        }

        public DynViewTimer(IViewHost Host)
        {
            _pluginHost = Host;

            _timer = new System.Timers.Timer();
            
            // Callback for the timer
            _timer.Elapsed += _timer_Elapsed;

            _pluginHost.OnShow += _pluginHost_OnShow;
            _pluginHost.OnHide += _pluginHost_OnHide;
            _pluginHost.OnClose += _pluginHost_OnClose;
            _pluginHost.OnGoOffline += _pluginHost_OnGoOffline;
            _pluginHost.OnGoOnline += _pluginHost_OnGoOnline;

            RunOnlyWhenOnline = true;
            RunOnlyWhenVisible = true;
            Interval = 1000;
        }

        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Return if busy doing some action
            if(_isBusy)
            {
                return;
            }

            if(Tick != null)
            {
                // Return if offline and can run only if online
                if(RunOnlyWhenOnline && !_pluginHost.Online)
                {
                    return;
                }

                // Return if hidden or closed and can run only if visible
                if (RunOnlyWhenVisible && !_pluginHost.Visible)
                {
                    return;
                }

                _isBusy = true;
                
                try
                {
                    Tick(this, null);
                }
                catch(Exception)
                {

                }

                _isBusy = false;
            }
        }

        void _pluginHost_OnGoOnline(object sender, PluginEventArgs e)
        {
            if (RunOnlyWhenVisible && !_pluginHost.Visible)
            {
                return;
            }

            // Enable the timer only if it was started before
            if(_startedByUser)
            {
                _timer.Enabled = true;
            }
        }

        void _pluginHost_OnGoOffline(object sender, PluginEventArgs e)
        {
            if(RunOnlyWhenOnline)
            {
                _timer.Enabled = false;
            }
        }

        void _pluginHost_OnClose(object sender, ViewEventArgs e)
        {
            _pluginHost_OnHide(sender, e);
        }

        void _pluginHost_OnHide(object sender, ViewEventArgs e)
        {
            if(RunOnlyWhenOnline)
            {
                _timer.Enabled = false;
            }
        }

        void _pluginHost_OnShow(object sender, ViewEventArgs e)
        {
            if (RunOnlyWhenOnline && !_pluginHost.Online)
            {
                return;
            }

            // Enable the timer only if it was started before
            if (_startedByUser)
            {
                _timer.Enabled = true;
            }            
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool Disposing)
        {
            if (!_isDisposed)
            {
                if (Disposing)
                {
                    _timer.Dispose();
                }
            }
        }

        ~DynViewTimer()
        {
            Dispose(false);
        }

        public void Start()
        {
            _timer.Start();
            _startedByUser = true;
        }

        public void Stop()
        {
            _timer.Stop();
            _startedByUser = false;
        }


        public bool IsEnabled
        {
            get { return _startedByUser; }
        }

        public bool IsRunning
        {
            get { return _timer.Enabled; }
        }
    }
}
