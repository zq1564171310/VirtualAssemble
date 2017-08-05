/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 扫描添加零件到集合
/// </summary>
namespace WyzLink.Manager
{
    using UnityEngine;
    using WyzLink.Assemble;
    using WyzLink.Parts;
    using WyzLink.Test;

    public class AddPartsManager : MonoBehaviour
    {
        private Transform[] PartsTransform;

        private GameObject RootPartGameObject;
        // Use this for initialization
        void Start()
        {
            //获取根节点的物体
            foreach (GameObject rootObj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (null != rootObj.GetComponent<AssembleController>())
                {
                    RootPartGameObject = rootObj;
                }
            }

            //查找机器的子物体
            if (null != RootPartGameObject)
            {
                PartsTransform = RootPartGameObject.GetComponentsInChildren<Transform>();
            }

            if (null != PartsTransform)
            {
                Parts parts;
                foreach (Transform child in PartsTransform)
                {
                    if (null != child.GetComponent<MeshFilter>())
                    {
                        child.gameObject.AddComponent<Parts>();
                        parts = child.GetComponent<Parts>();
                        parts.EndPos = child.transform.position;
                        parts.LocalSize = GlobalVar._GetModelSize.GetPartModelRealSize(child.gameObject);
                        parts.Name = child.name;
                        parts.PartsGameObject = child.gameObject;
                        GlobalVar._PartsManager.PartsList.Add(parts);
                    }
                }

                float ScalingNum = 1;
                //for (int i = 0; i < GlobalVar._PartsManager.PartsList.Count; i++)
                //{
                //    ScalingNum = GlobalVar._GetModelSize.Scaling(GlobalVar._PartsManager.PartsList[i].PartsGameObject, ModelType.Part);
                //    GlobalVar._PartsManager.PartsList[i].PartsGameObject.transform.localScale = new Vector3(GlobalVar._PartsManager.PartsList[i].PartsGameObject.transform.localScale.x / ScalingNum, GlobalVar._PartsManager.PartsList[i].PartsGameObject.transform.localScale.y / ScalingNum, GlobalVar._PartsManager.PartsList[i].PartsGameObject.transform.localScale.z / ScalingNum);
                //}
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
