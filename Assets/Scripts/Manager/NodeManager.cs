/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>zq</author>
/// <summary>
/// 单个零件的处理逻辑，如高亮，被点击等
/// </summary>
namespace WyzLink.Manager
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class NodeManager : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private Coroutine MaterialCoroutine;
        private Material OriginalMaterial;

        // Use this for initialization
        void Start()
        {
            if (null != gameObject.GetComponent<MeshRenderer>())
            {
                OriginalMaterial = gameObject.GetComponent<MeshRenderer>().sharedMaterial;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnPointerClick(PointerEventData eventData)
        {

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (null != gameObject.GetComponent<MeshRenderer>())
            {
                TransformIntoMaterial(GlobalVar.HighLightMate);
            }

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (null != gameObject.GetComponent<MeshRenderer>())
            {
                TransformIntoMaterial(OriginalMaterial);
            }

        }

        private void TransformIntoMaterial(Material targetMat)
        {
            if (this.MaterialCoroutine != null)
            {
                StopCoroutine(this.MaterialCoroutine);
            }
            this.MaterialCoroutine = StartCoroutine(TransformMaterialCoroutine(targetMat));
        }

        private IEnumerator TransformMaterialCoroutine(Material targetMat)
        {
            this.GetComponent<MeshRenderer>().material = targetMat;
            yield return 0;
        }
    }
}