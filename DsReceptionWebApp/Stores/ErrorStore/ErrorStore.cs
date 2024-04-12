using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DsReceptionWebApp.Stores.SpeechStore
{
    public class ErrorState
    {
        public string Message { get; }
        public ErrorState(string message)
        {
            Message = message;
        }
    }
    public class ErrorStore
    {
        private ErrorState _state;
        public ErrorStore()
        {
            _state = new ErrorState(new(""));
        }
        public ErrorState GetState()
        {
            return _state;
        }

        public void NewMessage(string message)
        {
            _state = new ErrorState(message);
            BroadcastStateChange();
        }

        public void ClearMessage()
        {
            _state = new ErrorState(null);
            BroadcastStateChange();
        }

        //////////////////

        private Action _listeners;
        public void AddStateChangeListeners(Action listener)
        {
            _listeners += listener;
        }
        public void RemoveStateChangeListeners(Action listener)
        {
            _listeners -= listener;
        }

        public void BroadcastStateChange()
        {
            _listeners.Invoke();
        }
    }
}
