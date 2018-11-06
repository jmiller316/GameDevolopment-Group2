/* ================================================================
   ---------------------------------------------------
   Project   :    Unreal FPS
   Publisher :    Infinite Dawn
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2018 All rights reserved.
   ================================================================ */

namespace UnrealFPS.Utility
{
    public class FiniteStateMachine
    {
        public delegate void ActiveState();

        private ActiveState activeState;

        public void SetState(ActiveState newActiveState)
        {
            activeState = newActiveState;
        }

        public void UpdateState()
        {
            if (activeState != null)
                activeState();
        }

        public ActiveState GetActiveState()
        {
            return activeState;
        }

        public bool StateEquals(ActiveState state)
        {
            if (activeState == state)
                return true;
            else
                return false;
        }
    }
}