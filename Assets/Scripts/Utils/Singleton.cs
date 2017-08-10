/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The part represent the part in the assembly line. It contains all the meta data 
/// about this part, and will be attached to the actual part gameObject
/// </summary>

namespace WyzLink.Utils
{
    using UnityEngine;                                                           //引用unity核心dll

    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour           //单例
    {
        private static T _instance;                                            //声明一个静态的泛型T的实例

        private static object _lock = new object();                            //声明一个静态变量，用于锁

        public static T Instance                                               //申明一个静态方法，返回泛型T的实例
        {
            get
            {
                if (applicationIsQuitting)                                     //如果程序正在退出
                {
                    Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                        "' already destroyed on application quit." +
                        " Won't create again - returning null.");              //打印相关信息
                    return null;                                               //返回空
                }

                lock (_lock)                                                   //枷锁
                {
                    if (_instance == null)                                     //如果该实例为空
                    {
                        _instance = (T)FindObjectOfType(typeof(T));             //实例化这个泛型对象

                        if (FindObjectsOfType(typeof(T)).Length > 1)            //如果返回的结果个数大于1
                        {
                            Debug.LogError("[Singleton] Something went really wrong " +
                                " - there should never be more than 1 singleton!" +
                                " Reopening the scene might fix it.");            //打印错误，说明已经不是单例了
                            return _instance;
                        }

                        if (_instance == null)                                                          //单例为空
                        {
                            GameObject singleton = new GameObject();                                    //new一个单例对象
                            _instance = singleton.AddComponent<T>();
                            singleton.name = "(singleton) " + typeof(T).ToString();                     //单例对象的名字

                            DontDestroyOnLoad(singleton);                                               //不销毁单例实例对象

                            Debug.Log("[Singleton] An instance of " + typeof(T) +                        //打印信息
                                " is needed in the scene, so '" + singleton +
                                "' was created with DontDestroyOnLoad.");
                        }
                        else
                        {
                            Debug.Log("[Singleton] Using instance already created: " +                  //单例正在使用实例
                                _instance.gameObject.name);
                        }
                    }

                    return _instance;
                }
            }
        }

        private static bool applicationIsQuitting = false;                                   ///程序是否合适
        /// <summary>
        /// When Unity quits, it destroys objects in a random order.
        /// In principle, a Singleton is only destroyed when application quits.
        /// If any script calls Instance after it have been destroyed, 
        ///   it will create a buggy ghost object that will stay on the Editor scene
        ///   even after stopping playing the Application. Really bad!
        /// So, this was made to be sure we're not creating that buggy ghost object.
        /// </summary>
        public void OnDestroy()
        {
            applicationIsQuitting = true;
        }
    }
}
