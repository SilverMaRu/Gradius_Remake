using UnityEngine;

namespace Assets.Scripts.Class.BaseClass
{
    public enum Team
    {
        White = -1,
        Gray = 0,
        Black = 1
    }
    public class Something : MonoBehaviour
    {
        public Team team = Team.Gray;
        public int atk;

        public ObjectPool objectPool { get; set; }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            Init();
        }

        protected virtual void Update() { }

        protected virtual void Init()
        {
            InitCurrentAttr();
            InitGameObjects();
            InitComponents();
        }

        protected virtual void InitCurrentAttr() { }

        protected virtual void InitGameObjects() { }

        protected virtual void InitComponents() { }
    }
}
