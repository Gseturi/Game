using System.Diagnostics;

namespace Game.Client.GameComponents.Classes
{
    public class GameTime
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();

        private long _lastTick = 0;
        private long _elapsedTicks = 0;
        private long _elapsedMilliseconds = 0;
        private long _lastMilliseconds = 0;

        public void Start()
        {
            _stopwatch.Reset();
            _stopwatch.Start();

            _lastTick = 0;
            _lastMilliseconds = 0;
        }

        public void Step()
        {
            _elapsedTicks = _stopwatch.ElapsedTicks - _lastTick;
            _elapsedMilliseconds = _stopwatch.ElapsedMilliseconds - _lastMilliseconds;

            _lastTick = _stopwatch.ElapsedTicks;
            _lastMilliseconds = _stopwatch.ElapsedMilliseconds;
        }

 
        public long TotalTicks => _stopwatch.ElapsedTicks;

     
     
        public long TotalMilliseconds => _stopwatch.ElapsedMilliseconds;

        
        public long ElapsedTicks => _elapsedTicks;

       
        public long ElapsedMilliseconds => _elapsedMilliseconds;

    }
}