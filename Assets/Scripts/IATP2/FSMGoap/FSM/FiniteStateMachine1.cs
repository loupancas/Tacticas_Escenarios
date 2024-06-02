using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSMach {

    public class FiniteStateMachine1 {

        private const int _MAX_TRANSITIONS_PER_FRAME = 3;

        public delegate void StateEvent(IState1 from, IState1 to);

        public event Action OnActive;
        public event Action OnUnActive;

        public IState1 CurrentState { get; private set; }
        private List<IState1> _allStates;

        private Func<IEnumerator, Coroutine> _startCoroutine;

        private bool _isActive;

        public FiniteStateMachine1(IState1 initialState, Func<IEnumerator, Coroutine> startCoroutine){
            CurrentState = initialState.Configure(this);
            _allStates = new List<IState1>{CurrentState};

            _startCoroutine = startCoroutine;
        }

        public IEnumerator Update() {
            while (Active) {
                CurrentState.UpdateLoop();
                
                var nextState = CurrentState.ProcessInput();
                var stateTransitions = 0;
                
                while (nextState != CurrentState && stateTransitions < _MAX_TRANSITIONS_PER_FRAME) {
                    var previousState = CurrentState;
                    var transitionParameters = CurrentState.Exit(nextState);

                    Debug.Log("Exiting state '" + CurrentState.Name + "' to state '" + nextState.Name + "'.");
                    
                    CurrentState = nextState;
                    CurrentState.Enter(previousState, transitionParameters);

                    nextState = CurrentState.ProcessInput();

                    stateTransitions++;
                }
                
                yield return null;
            }
        }

        public FiniteStateMachine1 AddTransition(string transitionName, IState1 from, IState1 to) {
            from.Configure(this);
            to.Configure(this);
            
            if (from.Transitions == null) 
                from.Transitions = new Dictionary<string, IState1>();

            if (!from.Transitions.ContainsKey(transitionName)) {
                from.Transitions.Add(transitionName, to);
                if (!_allStates.Contains(from)) _allStates.Add(from);
                if (!_allStates.Contains(to))   _allStates.Add(to);
            }

            return this;
        }

        public FiniteStateMachine1 Clear() {
            foreach (var state in _allStates) {
                state.Transitions = new Dictionary<string, IState1>();
                state.HasStarted  = false;
            }

            return this;
        }
        
        public bool Active {
            get { return _isActive; }
            set {
                if (_isActive == value) return;
                _isActive = value;
                if (_isActive) {
                    if (!CurrentState.HasStarted) CurrentState.Enter(CurrentState, null);
                    _startCoroutine(Update());
                    OnActive?.Invoke();
                }
                else
                    OnUnActive?.Invoke();
            }
        }


    }
}