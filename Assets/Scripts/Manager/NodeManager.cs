/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 单个零件的处理逻辑，如高亮，被点击等
/// </summary>
namespace WyzLink.Manager
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class NodeManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {

        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            //if (null != gameObject.GetComponent<MeshFilter>())
            //{
            //    gameObject.GetComponent<MeshRenderer>().sharedMaterial = GlobalVar.HighLightMate;
            //}
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            //if (null != gameObject.GetComponent<MeshFilter>())
            //{
            //    gameObject.GetComponent<MeshRenderer>().sharedMaterial = GlobalVar.NomalMate;
            //}
        }
    }
}