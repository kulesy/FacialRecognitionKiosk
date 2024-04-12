using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DsReceptionWebApp.Stores.SpeechStore
{
    public class SpeechState
    {
        public List<string> Message { get; }
        public SpeechState(List<string> message)
        {
            Message = message;
        }
    }
    public class SpeechStore
    {
        private SpeechState _state;
        public SpeechStore()
        {
            _state = new SpeechState(new());
        }
        public SpeechState GetState()
        {
            return _state;
        }

        public void NewMessage(List<string> message)
        {
            _state = new SpeechState(message);
            BroadcastStateChange();
        }

        public void ClearMessage()
        {
            _state = new SpeechState(null);
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
