using UnityEngine;

namespace MMCFeedbacks.Singleton
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance != null) return instance;
                var type = typeof(T);

                instance = (T)FindObjectOfType(type);
                if (instance != null) return instance;
                var typeName = type.ToString();

                var gameObject = new GameObject(typeName, type);
                instance = gameObject.GetComponent<T>();

                if (instance == null) Debug.LogError("Problem during the creation of " + typeName, gameObject);


                return instance;
            }
        }

        protected virtual void Awake()
        {
            // 他のゲームオブジェクトにアタッチされているか調べる
            // アタッチされている場合は破棄する。
            Initialize();
        }

        protected virtual void OnInitialize()
        {
        }

        private bool Initialize()
        {
            if (instance == null)
            {
                instance = this as T;
                OnInitialize();
                return true;
            }

            if (Instance == this) return true;

            Destroy(this);
            return false;
        }
    }
}