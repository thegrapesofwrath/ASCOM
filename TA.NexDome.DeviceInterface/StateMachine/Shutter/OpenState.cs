﻿namespace TA.NexDome.DeviceInterface.StateMachine.Shutter {
    class OpenState : ShutterStateBase {
        /// <inheritdoc />
        public OpenState(ControllerStateMachine machine) : base(machine)
            { }

        /// <inheritdoc />
        public override void OnEnter()
            {
            base.OnEnter();
            Machine.ShutterInReadyState.Set();
            }

        /// <inheritdoc />
        public override void OnExit()
            {
            base.OnExit();
            Machine.ShutterInReadyState.Reset();
            }
        }
    }