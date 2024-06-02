using System;
using System.Collections.Generic;
using UnityEngine;

namespace FSMach {

    public abstract class MonoBaseState : MonoBehaviour, IState1 {

        public event Action     OnNeedsReplan;
        public event StateEvent OnEnter;
        public event StateEvent OnExit;
        
        public virtual string Name => GetType().Name;

        public virtual bool HasStarted { get; set; }

        public FiniteStateMachine1 FSM => _fsm;

        public virtual Dictionary<string, IState1> Transitions { get; set; } = new Dictionary<string, IState1>();

        private FiniteStateMachine1 _fsm;


        public IState1 Configure(FiniteStateMachine1 fsm) {
            _fsm            =  fsm;
            _fsm.OnActive   += OnActive;
            _fsm.OnUnActive += OnUnActive;
            return this;
        }

        public virtual void Enter(IState1 from, Dictionary<string, object> transitionParameters = null) {
            OnEnter?.Invoke(from, this);
            HasStarted = true;
        }

        public virtual Dictionary<string, object> Exit(IState1 to) {
            OnExit?.Invoke(this, to);
            HasStarted = false;
            return null;
        }

        public abstract void UpdateLoop();

        protected virtual void OnActive() {}

        protected virtual void OnUnActive() {}

        public abstract IState1 ProcessInput();

       
    }
}