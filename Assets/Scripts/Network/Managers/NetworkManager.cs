using System;
using System.Collections;
using Core.Data;
using Core.Networking.Protocols;
using Core.Visualisation;
using Core.World;
using FlatBuffers;
using GUI.ViewModel;
using JetBrains.Annotations;
using UnityEngine;
using Utils.Extensions;
using Utils.Network;

#if DEBUG
using GUI.ViewModel.DebugGUI;
#endif

namespace Core.Networking
{
    public class NetworkManager
    {
        private Simulation _simulation;
        private int _id = -1;

#if DEBUG
        private DebugViewModel _debugViewModel;
#endif

        //TODO: consider "ip builder"
        public const string SERVER_URL = "TEMP_URL";

        public NetworkManager(
            int id,
            [NotNull] Simulation simulation
#if DEBUG
            ,[NotNull] DebugViewModel debugViewModel
#endif
            )
        {
#if DEBUG
            _debugViewModel = debugViewModel;
#endif
            _simulation = simulation;
            _id = id;

            RetrieveData(_id);
            Debug.LogError(_id);
        }

        public void RetrieveData(int guid)
        {
#if DEBUG
            HttpProtocol.RetrieveStringData(
                $"{SERVER_URL}/current_period",
                data => _debugViewModel.SetDebug(data),
                errorMessage =>
                {
#if UNITY_EDITOR
                    Debug.Log(errorMessage);
#endif
                }
            );
#endif

            HttpProtocol.RetrieveData(
                $"{SERVER_URL}/simulation?uid={guid}",
                data => { _simulation.Show(data, () => RetrieveData(_id)); },
                errorMessage =>
                {
#if UNITY_EDITOR
                    Debug.Log(errorMessage);
#endif
                }
            );
        }
    }
}
