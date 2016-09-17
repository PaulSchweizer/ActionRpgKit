using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionRpgKit.Character
{
    public interface IState
    {
        /// <summary>
        /// Called when entering the State.</summary>
        void EnterState();

        /// <summary>
        /// Called every frame when the State is active.</summary>
        void UpdateState();

        /// <summary>
        /// Called right before changing to the next State.</summary>
        void ExitState();
    }

    public class IdleState : IState
    {
        public void EnterState()
        {
            throw new NotImplementedException();
        }

        public void ExitState()
        {
            throw new NotImplementedException();
        }

        public void UpdateState()
        {
            throw new NotImplementedException();
        }
    }
}
