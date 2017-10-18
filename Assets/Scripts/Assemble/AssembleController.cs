/// <copyright>(c) 2017 WyzLink Inc. All rights reserved.</copyright>
/// <author>xlzou</author>
/// <summary>
/// The part represent the part in the assembly line. It contains all the meta data 
/// about this part, and will be attached to the actual part gameObject
/// </summary>

namespace WyzLink.Assemble                                                            //Assemble命名空间
{
    using System;                                                                     //引用命名空间System（C#）
    using System.Collections.Generic;                                                 //System.Collections.Generic（集合命名控件）
    using System.IO;                                                                  //引用IO（流）命名空间
    using UnityEngine;                                                                //引用unity自带命名空间

    public class AssembleController : MonoBehaviour, INodeLoader                      //安装类，继承行为类（unity中所有生成类都继承行为类），继承INodeLoader接口，该接口提供四个方法，1.GetAllNodes获取所有零件，2.GetAssembleFlowAsset获取零件节点文件，3.ParseAssembleFlowFile格式化零件节点文件。4.SetAssembleFlowAsset设置零件节点文件；AssembleController类继承INodeLoader接口，那么也要实现这四个抽象方法
    {
        [SerializeField]                                    //  [SerializeField]表示将原本不会被序列化的私有变量和保护变量变成可以被序列化的，那么它们在下次读取的值就是你上次赋值的值（也就是在unity中拖拽文件，给assembleFlow赋值，下一次打开就不需要再拖拽相同的文件赋值了）。unity会自动为public类型数据做序列化。
        public TextAsset assembleFlow;

        private DependencyGraph dependencyGraph;              //声明一个DependencyGraph类的对象

        private void Awake()
        {
            dependencyGraph = new DependencyGraph(this, assembleFlow.text);                     //实例化DependencyGraph类的对象
        }

        public DependencyGraph GetDependencyGraph()                                             //提供一个获取DependencyGraph类的对象
        {
            return dependencyGraph;                                                             //返回AssembleController类中声明的DependencyGraph类的对象
        }

        public IEnumerable<T> GetAllNodes<T>(Transform transform = null)                       //获取所有节点的方法，继承自INodeLoader接口
        {
            if (transform == null)
            {
                transform = this.transform;                                                   //如果变量transform为空（必定为空，因为一旦调用GetAllNodes方法，Transform transform = null）， 那么让transform等于挂载了AssembleController类的物体，即被组装的机器模型的transform
            }

            var part = transform.GetComponent<T>();                                 //获取transform的Node组件
            if (part != null && IsPartEnabledComponent(part))                       //如果扫描的零件不为空，且Node脚本为可用
            {
                // We only count if the component is enabled
                yield return part;                                                  //返回这个零件
            }
            else                                                                    //说明扫描的零件可能为空，或者没有挂载Node脚本，那么这个物体很可能是父物体，真正有用的零件是其子物体
            {
                foreach (Transform t in transform)                             //遍历子物体
                {
                    foreach (var p in GetAllNodes<T>(t))                       //对每个子物体继续调用上述方法，直到所有层级物体都被执行到
                    {
                        yield return p;
                    }
                }
            }
        }

        private bool IsPartEnabledComponent<T>(T part)                           //判断零件上面的Node脚本是否可用，如果隐藏，则不可用
        {
            // Kind of hacky, but so far it is the best way to convert a type to T
            MonoBehaviour m = (MonoBehaviour)(object)part;
            return m.enabled;
        }

        public void ParseAssembleFlowFile(string text, Action<int, int> connect)
        {
            var sr = new StringReader(text);
            int lineNumber = 0;
            while (true)
            {
                var line = sr.ReadLine();
                if (line == null)
                {
                    break;
                }

                line.Trim();
                if (line.StartsWith("#"))
                {
                    // Comment, ignore
                }
                else
                {
                    var index = line.IndexOf("->");
                    if (index == -1)
                    {
                        Debug.LogError("The text parsing failed on line: " + line + "@" + lineNumber);
                    }
                    var t0 = line.Substring(0, index);
                    var t1 = line.Substring(index + 2, line.Length - index - 2);
                    int a0;
                    int a1;
                    if (Int32.TryParse(t0, out a0) && Int32.TryParse(t1, out a1))
                    {
                        connect(a0, a1);
                    }
                    else
                    {
                        Debug.LogError("Failed to parse the format of line: " + line + "@" + lineNumber);
                    }
                }
                lineNumber++;
            }
        }

        public TextAsset GetAssembleFlowAsset()
        {
            return this.assembleFlow;
        }

        public void SetAssembleFlowAsset(TextAsset asset)
        {
            this.assembleFlow = asset;
        }
    }
}