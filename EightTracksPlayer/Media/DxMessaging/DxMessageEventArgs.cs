using System;
using System.Windows.Forms;

namespace EightTracksPlayer.Media.DxMessaging
{
    public class DxMessageEventArgs : EventArgs
    {
        public DxMessageEventArgs(Message message)
        {
            this.Message = message;
        }

        public Message Message { get; private set; }
    }
}