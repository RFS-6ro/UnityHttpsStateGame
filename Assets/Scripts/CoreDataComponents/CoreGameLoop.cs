using System;
using System.Collections;
using System.Collections.Generic;
using Core.Networking;
using Core.Networking.Protocols;
using UnityEngine;
using UnityEngine.Networking;
using Utils.Coroutines;

namespace Core.Model
{
    //Work In Progress: Custom Yielders State Machine 
    public class CoreGameLoop : MonoBehaviour
    {
        private bool _connectionIsEstablished;
        private IEnumerable<IStateAffectible> _affectibles;

        private int _id;

        private GameLoopState _currentState = GameLoopState.Disabled;
        private GameLoopState CurrentState
        {
            get => _currentState;
            set
            {
                if (_currentState == value)
                {
                    return;
                }

                _currentState = value;
                SetStateToAllAffectibles(_currentState);
            }
        }

        public void SetId(int id)
        {
            _id = id;
        }

        public void SetConnectionState(bool state)
        {
            _connectionIsEstablished = state;
        }

        public void SetAffectibles(IEnumerable<IStateAffectible> affectibles)
        {
            _affectibles = affectibles;
            SetStateToAllAffectibles(_currentState);
        }

        public void StartLoop()
        {
            StartCoroutine(Loop());
        }

        private IEnumerator Loop()
        {
            //TODO: WARNING first simulation may be lagged
            //TODO: show input trajectory
            while (_connectionIsEstablished)
            {
                CurrentState = GameLoopState.SimulationReceiving;
                //read from server
                yield return HttpProtocol.GetResponce(ReadUrl, out UnityWebRequest readwww);
                AssertRequest(readwww);
                //TODO: deserialize simulation
                //TODO: replace with simulation reading
                long period = 20;
                //TODO: replace with simulation reading
                long currentPeriodTimeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();


                CurrentState = GameLoopState.Visualisation;
                //TODO: show timer
                // yield return CoroutineUtils.WaitAny(
                //     new WaitForTimeStamp(currentPeriodTimeStamp + 5), //TODO: possible replacement 6 for simulation length read from http response
                //     new SimulationDisplay(Frames[])
                // );
                yield return new WaitForTimeStamp(currentPeriodTimeStamp + 5);


                CurrentState = GameLoopState.CollectingInput;
                //TODO: set input available
                //TODO: show timer
                yield return new WaitForTimeStamp(currentPeriodTimeStamp + period - 1);


                CurrentState = GameLoopState.SendingInput;
                string input = ""; //TODO: set input unavailable and read in
                //send input on server
                yield return HttpProtocol.GetResponce(GetSendUrl(input), out UnityWebRequest sendwww);
                AssertRequest(sendwww);
                //TODO: show timer
                yield return new WaitForTimeStamp(currentPeriodTimeStamp + period);
            }
        }

        private void AssertRequest(UnityWebRequest www)
        {
            if (www.isNetworkError || www.isHttpError)
            {
                CurrentState = GameLoopState.Disabled;
                SetConnectionState(false);
                StopLoopForce();
            }
        }

        private string ReadUrl => $"{NetworkManager.SERVER_URL}/simulation?uid={_id}";
        private string GetSendUrl(string input) => $"{NetworkManager.SERVER_URL}/input?uid={_id}&direction={input}";

        public void StopLoopForce()
        {
            StopCoroutine(Loop());
        }

        private void SetStateToAllAffectibles(GameLoopState newState)
        {
            if (_affectibles == null)
            {
                return;
            }

            foreach (var affectible in _affectibles)
            {
                affectible.SetState(newState);
            }
        }
    }
}
