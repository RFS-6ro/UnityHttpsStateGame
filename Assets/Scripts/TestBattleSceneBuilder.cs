using Core.Model;
using Core.Visualisation;
using UnityEngine;

namespace Core.World
{
    public class TestBattleSceneBuilder : MonoBehaviour
    {
        [SerializeField] private Border _border;
        [SerializeField] private CoreGameLoop _loop;

        void Start()
        {
            _loop.SetConnectionState(true);
            _loop.SetAffectibles(new[] { _border });
            _loop.StartLoop();
        }
    }
}
