using System;

namespace Tic_Tac_Toe
{
    public class GomokuEventArgs : EventArgs
    {
        public string Message { get; set; }
        public int Status { get; set; }

        public GomokuEventArgs(string message, int status)
        {
            Message = message;
            Status = status;

        }


    }
}
