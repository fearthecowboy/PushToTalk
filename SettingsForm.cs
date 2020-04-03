using Gma.System.MouseKeyHook;
using Microsoft.Win32;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using PushToTalk.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace PushToTalk
{

    enum State
    {
        None,
        Active,
        Query
    }
    public partial class SettingsForm : Form, IDisposable
    {
        private static string None = "<None>";
        private bool active;
        
        private ManualResetEvent quit = new ManualResetEvent(false);
        private ManualResetEvent trigger = new ManualResetEvent(false);

        // Icon that displays in the tray area, it changes colours based on the state of the microphone
        private System.Windows.Forms.NotifyIcon notifier;

        private IKeyboardMouseEvents m_GlobalHook;
        private State mouse;
        private State key;

        private HashSet<Keys> keys = new HashSet<Keys>();
        private HashSet<Keys> currentKeys = new HashSet<Keys>();

        private HashSet<MouseButtons> buttons = new HashSet<MouseButtons>();
        private HashSet<MouseButtons> currentButtons = new HashSet<MouseButtons>();

        private Task loop;
        private WaveOut wave = new WaveOut();

        private RegistryKey app = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software").CreateSubKey("FearTheCowboy").CreateSubKey("PushToTalk");

        public SettingsForm()
        {
            InitializeComponent();

            CreateTrayIcon();

            m_GlobalHook = Hook.GlobalEvents();

            m_GlobalHook.MouseDownExt += mouseDown;
            m_GlobalHook.KeyDown += keyDown;

            m_GlobalHook.MouseUp += mouseUp;
            m_GlobalHook.KeyUp += keyUp;

            loop = Task.Run(worker);
            keyText.Text = None;
            mouseText.Text = None;

            Task.Delay(1).ContinueWith((p) => Invoke(new Action(load)));
            
        }


        private void save()
        {
            app.SetValue("Key",keyText.Text, RegistryValueKind.String);
            app.SetValue("Buttons",mouseText.Text, RegistryValueKind.String);
            app.SetValue("Delay", (int)delay.Value, RegistryValueKind.DWord);
            app.SetValue("CancelMouseEvents", cancelMouse.Checked ? 1 : 0, RegistryValueKind.DWord);
        }

        private T[] parse<T>( string value )
        {
            if( string.IsNullOrEmpty(value) || value == None)
            {
                return Enumerable.Empty<T>().ToArray();
            }

            return value.Split('+').Select(each => (T)Enum.Parse(typeof(T), each)).ToArray();
        }

        private void load()
        {
            keys.Clear();
            foreach ( var each in parse<Keys>(app.GetValue("Key")?.ToString() ?? None) ) {
                keys.Add(each);
            }
            keyText.Text =keys.Count == 0 ? None:  keys.Select(each => each.ToString()).Aggregate((c, a) => $"{c}+{a}");

            buttons.Clear();
            foreach (var each in parse<MouseButtons>(app.GetValue("Buttons")?.ToString() ?? None))
            {
                buttons.Add(each);
            }
            mouseText.Text = buttons.Count == 0 ? None : buttons.Select(each => each.ToString()).Aggregate((c, a) => $"{c}+{a}");


            delay.Value = Convert.ToInt32(app.GetValue("Delay", 250));
            cancelMouse.Checked = app.GetValue("CancelMouseEvents", 1) == (object)1;

            if( keyText.Text == None && mouseText.Text == None)
            {
                Show();
            } else
            {
                Hide();
                mouse = mouseText.Text == None ? State.None : State.Active;
                key= keyText.Text == None ? State.None : State.Active;
            }
        }

        private IAudioEndpointVolume volumeControl
        {
            get
            {
                IMMDeviceEnumerator deviceEnumerator = null;
                IMMDevice device = null;
                try
                {
                    deviceEnumerator = (IMMDeviceEnumerator)(new MMDeviceEnumerator());
                    deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eCapture, ERole.eMultimedia, out device);

                    Guid IID_IAudioEndpointVolume = typeof(IAudioEndpointVolume).GUID;
                    object o;
                    device.Activate(ref IID_IAudioEndpointVolume, 0, IntPtr.Zero, out o);
                    IAudioEndpointVolume masterVol = (IAudioEndpointVolume)o;

                    return masterVol;
                }
                finally
                {
                    if (device != null) {
                        Marshal.ReleaseComObject(device);
                    };
                    if (deviceEnumerator != null) {
                        Marshal.ReleaseComObject(deviceEnumerator);
                    }
                }
            }
        }

        private bool MicrophoneMuted {
            get {
                IAudioEndpointVolume masterVol = null;
                try
                {
                    bool isMuted;
                    masterVol = volumeControl;
                    if (masterVol == null)
                    {
                        return false;
                    }
                    
                    masterVol.GetMute(out isMuted);
                    return isMuted;
                }
                finally
                {
                    if (masterVol != null)
                    {
                        Marshal.ReleaseComObject(masterVol);
                    }
                }
            }
        
            set {
                IAudioEndpointVolume masterVol = null;
                try
                {
                    masterVol = volumeControl;
                    if (masterVol == null)
                    {
                        return;
                    }
                    masterVol.SetMute(value, Guid.Empty);
                }
                finally
                {
                    if (masterVol != null)
                        Marshal.ReleaseComObject(masterVol);
                }
            }
        }

        private void CreateTrayIcon()
        {
            notifier = new System.Windows.Forms.NotifyIcon();
            notifier.Icon = Properties.Resources.mic_off;
            notifier.Visible = true;

            var trayContext = new System.Windows.Forms.ContextMenuStrip();

            var show = new System.Windows.Forms.ToolStripMenuItem("Settings");
            show.Click += (x, y) =>
            {
                this.Show();
            };
            trayContext.Items.Add(show);

            var exit = new System.Windows.Forms.ToolStripMenuItem("Exit");
            exit.Click += (x, y) =>
            {
                quit.Set();
                this.Close();
            };
            trayContext.Items.Add(exit);

            notifier.Click += Notifier_Click;
            notifier.ContextMenuStrip = trayContext;
        }

        private void Notifier_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void worker()
        {
            var a = new Action(activate);
            var d = new Action(deactivate);
            while (true)
            {
                ManualResetEvent.WaitAny(new WaitHandle[] { trigger, quit });

                if(quit.WaitOne(0) )
                {
                    return;
                }
                trigger.Reset();

                if ((currentKeys.Count > 0 && currentKeys.SetEquals(keys)) || (currentButtons.Count > 0 && currentButtons.SetEquals(buttons)))
                {
                   this.Invoke(a);
                }
                else
                {
                    if (active)
                    {
                        Task.Delay((int)delay.Value).ContinueWith(p => { this.Invoke(d); });
                    }
                }
            }
        }
        private void PlaySound(UnmanagedMemoryStream sound)
        {
            using (var provider = new RawSourceWaveStream(sound, new WaveFormat()))
            {
                wave.Init(provider);
                wave.Play();
            }
        }

        private void activate()
        {
            if (!active)
            {
                PlaySound(Properties.Resources.on);
                active = true;
                notifier.Icon = Resources.mic_on;
                MicrophoneMuted = false;
            }
        }

        private void deactivate()
        {
            active = false;
            notifier.Icon = Resources.mic_off;
            PlaySound(Properties.Resources.off);
            MicrophoneMuted = true;
        }

        private void keyDown(object sender, KeyEventArgs e)
        {
            switch (key)
            {
                case State.Active:
                    if (keys.Count > 0)
                    {
                        currentKeys.Add(e.KeyCode);
                        trigger.Set();

                        // for single-key hotkey, cancel then keypress (so pause, capslock, scroll-lock, etc work )
                        if (keys.Count == 0 && currentKeys.SetEquals(keys) )
                        {
                            e.Handled = true;
                        }
                    }
                    break;

                case State.Query:
                    keys.Add(e.KeyCode);
                    e.Handled = true;
                    keyText.Text = keys.Select(each => each.ToString()).Aggregate((c,a)=> $"{c}+{a}" );
                    break;
            }
        }

        private void keyUp(object sender, KeyEventArgs e)
        {
            switch (key)
            {
                case State.Active:
                    currentKeys.Remove(e.KeyCode);
                    trigger.Set();
                    break;
                case State.Query:
                    key = State.Active;
                    save();
                    break;
            }
        }

        private void mouseDown(object sender, MouseEventExtArgs e)
        {
            // skip button one and two
            if( e.Button == MouseButtons.Left || e.Button == MouseButtons.Right   )
            {
                return;
            }

            switch (mouse)
            {
                case State.Active:
                    currentButtons.Add(e.Button);
                    e.Handled = cancelMouse.Checked && buttons.Contains(e.Button);
                    trigger.Set();
                    break;

                case State.Query:
                    buttons.Add(e.Button);
                    mouseText.Text = buttons.Select(each => each.ToString()).Aggregate((c, a) => $"{c}+{a}");
                    break;
            }
        }


        private void mouseUp(object sender, MouseEventArgs e)
        {
            // skip button one and two
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                return;
            }

            switch (mouse)
            {
                case State.Active:
                    currentButtons.Remove(e.Button);
                    trigger.Set();
                    break;
                case State.Query:
                    mouse = State.Active;
                    save();
                    break;
            }
        }

        private void setKey_Click(object sender, EventArgs e)
        {
            keyText.Text = "Press and hold a key combination";
            key = State.Query;
            keys.Clear();
            currentKeys.Clear();
        }

        private void clearKey_Click(object sender, EventArgs e)
        {
            keyText.Text = None;
            key = State.None;
            keys.Clear();
            currentKeys.Clear();
            save();
        }

        private void setMouse_Click(object sender, EventArgs e)
        {
            mouseText.Text = "Press a mouse button combination";
            mouse = State.Query;
            buttons.Clear();
            currentButtons.Clear();
        }

        private void clearMouse_Click(object sender, EventArgs e)
        {
            mouseText.Text = None;
            mouse = State.None;
            buttons.Clear();
            currentButtons.Clear();
            save();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // quit only when really quitting
            e.Cancel = !quit.WaitOne(0);

            if( e.Cancel)
            {
                // hide the form then
                this.Hide();
            }

        }

        private void delay_ValueChanged(object sender, EventArgs e)
        {
            save();
        }

        private void cancelMouse_CheckedChanged(object sender, EventArgs e)
        {
            save();
        }
    }
}
