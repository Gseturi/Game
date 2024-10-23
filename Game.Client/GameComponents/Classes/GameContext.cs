using System;
using System.ComponentModel.DataAnnotations;

namespace Game.Client.GameComponents.Classes
{
    public abstract class GameContext
    {
        bool IsFirst = true;
        public async ValueTask Step()
        {
            if (IsFirst)
            {
                this.GameTime.Start();
                IsFirst = false;
            }

            this.GameTime.Step();

            await Update();
            await Render();
        }
        protected abstract ValueTask Update();
        protected abstract ValueTask Render();

        public GameTime GameTime { get; } = new GameTime();
        public Display Display { get; } = new Display();


    }
}
