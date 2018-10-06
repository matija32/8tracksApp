using System;
using System.Windows.Forms;
using QuartzTypeLib;

namespace EightTracksPlayer.Media.DxMessaging
{
    public sealed class DxMessageHandler : NativeWindow
    {
        private const int WM_APP = 0x8000;
        private const int WM_GRAPHNOTIFY = WM_APP + 1;
        private const int EC_COMPLETE = 0x01;
        private const int WS_CHILD = 0x40000000;
        private const int WS_CLIPCHILDREN = 0x2000000;
        private IMediaEventEx mediaEventEx;

        public DxMessageHandler() : base()
        {
            CreateHandle(new CreateParams());
        }

        public IMediaEventEx MediaEventEx
        {
            get { return mediaEventEx; }
            set
            {
                if (mediaEventEx != null)
                {
                    mediaEventEx.SetNotifyWindow(0, WM_GRAPHNOTIFY, 0);
                }

                mediaEventEx = value;
                mediaEventEx.SetNotifyWindow((int) this.Handle, WM_GRAPHNOTIFY, 0);
            }
        }

        public event EventHandler<DxMessageEventArgs> PlaybackStopped;


        protected override void WndProc(ref Message message)
        {
            Console.WriteLine(message);
            if (message.Msg == WM_GRAPHNOTIFY)
            {
                int lEventCode;
                int lParam1, lParam2;

                while (true)
                {
                    try
                    {
                        MediaEventEx.GetEvent(out lEventCode,
                                              out lParam1,
                                              out lParam2,
                                              0);

                        MediaEventEx.FreeEventParams(lEventCode, lParam1, lParam2);

                        if (lEventCode == EC_COMPLETE)
                        {
                            OnPlaybackStopped(message);
                        }
                    }
                    catch (Exception)
                    {
                        break;
                    }
                }
            }

            base.WndProc(ref message);
        }

        private void OnPlaybackStopped(Message msg)
        {
            EventHandler<DxMessageEventArgs> handler = PlaybackStopped;
            if (handler != null)
            {
                handler(this, new DxMessageEventArgs(msg));
            }
        }
    }
}