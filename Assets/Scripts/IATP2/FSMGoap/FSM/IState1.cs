using System;
using System.Collections.Generic;

namespace FSMach {

    public interface IState1 {

        event Action     OnNeedsReplan;

        event StateEvent OnEnter;
        event StateEvent OnExit;

        FiniteStateMachine1 FSM        { get; }
        string             Name       { get; }
        bool               HasStarted { get; set; }
        
        Dictionary<string, IState1>     Transitions        { get; set; }

        IState1 Configure(FiniteStateMachine1 fsm);

        void                       Enter(IState1 from, Dictionary<string, object> transitionParameters);
        void                       UpdateLoop();
        Dictionary<string, object> Exit(IState1 to);

        IState1 ProcessInput();

    }

    public delegate void StateEvent(IState1 from, IState1 to);
}