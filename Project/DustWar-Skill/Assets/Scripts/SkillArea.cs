using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 技能指示器类型枚举
/// </summary>
public enum SkillAreaType
{
    OuterCircle = 0,
    OuterCircle_InnerCube = 1,
    OuterCircle_InnerSector = 2,
    OuterCircle_InnerCircle = 3,
}

public class SkillArea : MonoBehaviour {

    /// <summary>
    /// 技能指示器元素（技能指示器组件）枚举
    /// </summary>
    enum SKillAreaElement
    {
        /// <summary>
        /// 外圆
        /// </summary>
        OuterCircle,   
        /// <summary>
        /// 内圆
        /// </summary>
        InnerCircle,   
        /// <summary>
        /// 矩形 
        /// </summary>
        Cube,   
        /// <summary>
        /// 扇形
        /// </summary>
        Sector60,        
        /// <summary>
        /// 扇形
        /// </summary>
        Sector120,       
    }
    /// <summary>
    /// 技能控制类
    /// </summary>
    SkillController skill;
    /// <summary>
    /// 玩家（角色）
    /// </summary>
    public GameObject player;      
    /// <summary>
    /// 设置指示器类型
    /// </summary>
    private SkillAreaType areaType;

    /// <summary>
    /// 技能指示器的位置
    /// </summary>
    internal Vector3 deltaVec;

    /// <summary>
    /// 外圆半径
    /// </summary>
    public float outerRadius = 6f;      
    /// <summary>
    /// 内圆半径
    /// </summary>
    public float innerRadius = 2f;  
    /// <summary>
    /// 矩形宽度 （矩形长度使用的外圆半径）
    /// </summary>
    public float cubeWidth = 2f;       
    /// <summary>
    /// 扇形角度
    /// </summary>
    public int angle = 60;
    /// <summary>
    /// 技能是否启用的标识（是否摁下技能）
    /// </summary>
    public bool isPressed = false;

    /// <summary>
    /// 路径
    /// </summary>
    string path = "Effect/Prefabs/Hero_skillarea/";  
    /// <summary>
    /// 圆形
    /// </summary>
    string circle = "quan_hero";    
    /// <summary>
    /// 矩形
    /// </summary>
    string cube = "chang_hero";   
    /// <summary>
    /// 扇形60度
    /// </summary>
    string sector60 = "shan_hero_60";
    /// <summary>
    /// 扇形120度
    /// </summary>
    string sector120 = "shan_hero_120";    

    /// <summary>
    /// 所有技能指示器对应的路径（这里是除了基本目录之外的部分，即可理解为文件名）
    /// </summary>
    Dictionary<SKillAreaElement, string> allElementPath;
    /// <summary>
    /// 所有技能指示器对应的Transform
    /// </summary>
    Dictionary<SKillAreaElement, Transform> allElementTrans;

    /// <summary>
    /// 程序开始执行的方法(优先级：0)
    /// </summary>
    void Start()
    {
        //实例化技能控制器类
        skill = GetComponent<SkillController>();
        //获取技能指示器类型
        areaType = skill.SkillType;
        //绑定事件
        skill.showSkill += Show;
        skill.moveSkill += Move;
        skill.hideSkill += Hide;
        //初始化技能指示器（添加对应指示器范围类型的Path和Transform）
        InitSkillAreaType();
    }

    /// <summary>
    /// 销毁事件
    /// </summary>
    void OnDestroy()
    {
        skill.showSkill -= Show;
        skill.moveSkill -= Move;
        skill.hideSkill -= Hide;
    }

    /// <summary>
    /// 初始化技能指示器
    /// </summary>
    void InitSkillAreaType()
    {
        //添加对应指示器范围类型的Path
        allElementPath = new Dictionary<SKillAreaElement, string>();
        allElementPath.Add(SKillAreaElement.OuterCircle, circle);
        allElementPath.Add(SKillAreaElement.InnerCircle, circle);
        allElementPath.Add(SKillAreaElement.Cube, cube);
        allElementPath.Add(SKillAreaElement.Sector60, sector60);
        allElementPath.Add(SKillAreaElement.Sector120, sector120);
        //添加对应指示器范围类型的Transform，初始化时为实际生成对象，所以Transform为null
        allElementTrans = new Dictionary<SKillAreaElement, Transform>();
        allElementTrans.Add(SKillAreaElement.OuterCircle, null);
        allElementTrans.Add(SKillAreaElement.InnerCircle, null);
        allElementTrans.Add(SKillAreaElement.Cube, null);
        allElementTrans.Add(SKillAreaElement.Sector60, null);
        allElementTrans.Add(SKillAreaElement.Sector120, null);
    }

    /// <summary>
    /// 显示技能指示器的方法
    /// </summary>
    /// <param name="deltaVec">技能指示器的平面位置</param>
    void Show(Vector2 deltaVec)
    {
        //技能启用标识设置为true
        isPressed = true;
        //通过传入的的技能指示器平面坐标生成三维坐标
        this.deltaVec = new Vector3(deltaVec.x, 0, deltaVec.y);
        //创建技能指示器
        CreateSkillArea();
    }
    /// <summary>
    /// 隐藏技能指示器的方法
    /// </summary>
    void Hide()
    {
        //技能启用标识设置为false
        isPressed = false;
        //隐藏技能指示器
        HideElements();
    }
    /// <summary>
    /// 移动技能指示器方法
    /// </summary>
    /// <param name="deltaVec"></param>
    void Move(Vector2 deltaVec)
    {
        //设置技能指示器位置
        this.deltaVec = new Vector3(deltaVec.x, 0, deltaVec.y);
    }
    /// <summary>
    /// 
    /// </summary>
    void LateUpdate()
    {
        //获取技能指示器类型
        areaType = GetComponent<SkillController>().SkillType;
        //技能启用标识为true才实时更新技能位置
        if(isPressed)
            UpdateElement();
    }

    /// <summary>
    /// 创建技能区域展示
    /// </summary>
    void CreateSkillArea()
    {
        //通过区分不同的指示器元素类型创建技能指示器
        switch (areaType)
        {
            case SkillAreaType.OuterCircle:
                CreateElement(SKillAreaElement.OuterCircle);
                break;
            case SkillAreaType.OuterCircle_InnerCube:
                CreateElement(SKillAreaElement.OuterCircle);
                CreateElement(SKillAreaElement.Cube);
                //HideElement(SKillAreaElement.OuterCircle);
                break;
            case SkillAreaType.OuterCircle_InnerSector:
                CreateElement(SKillAreaElement.OuterCircle);
                switch (angle)
                {
                    case 60:
                        CreateElement(SKillAreaElement.Sector60);
                        break;
                    case 120:
                        CreateElement(SKillAreaElement.Sector120);
                        break;
                    default:
                        break;
                }
                break;
            case SkillAreaType.OuterCircle_InnerCircle:
                CreateElement(SKillAreaElement.OuterCircle);
                CreateElement(SKillAreaElement.InnerCircle);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 创建技能区域展示元素
    /// </summary>
    /// <param name="element"></param>
	void CreateElement(SKillAreaElement element)
    {
        Transform elementTrans = GetElement(element);
        if (elementTrans == null) return;
        allElementTrans[element] = elementTrans;
        switch (element)
        {
            case SKillAreaElement.OuterCircle:
                elementTrans.localScale = new Vector3(outerRadius * 2, 1, outerRadius * 2) / player.transform.localScale.x;
                elementTrans.gameObject.SetActive(true);
                break;
            case SKillAreaElement.InnerCircle:
                elementTrans.localScale = new Vector3(innerRadius * 2, 1, innerRadius * 2) / player.transform.localScale.x;
                break;
            case SKillAreaElement.Cube:
                elementTrans.localScale = new Vector3(cubeWidth, 1, outerRadius) / player.transform.localScale.x;
                break;
            case SKillAreaElement.Sector60:
            case SKillAreaElement.Sector120:
                elementTrans.localScale = new Vector3(outerRadius, 1, outerRadius) / player.transform.localScale.x;
                break;
            default:
                break;
        }
    }

    Transform elementParent;
    /// <summary>
    /// 获取元素的父对象
    /// </summary>
    /// <returns></returns>
    Transform GetParent()
    {
        if (elementParent != null)
        {
            elementParent = player.transform.Find("SkillArea");
        }
         else if (elementParent == null)
        {
            elementParent = new GameObject("SkillArea").transform;
            elementParent.parent = player.transform;
            elementParent.localEulerAngles = Vector3.zero;
            elementParent.localPosition = Vector3.zero;
            elementParent.localScale = Vector3.one;
        }
        return elementParent;
    }

    /// <summary>
    /// 获取元素物体
    /// </summary>
    Transform GetElement(SKillAreaElement element)
    {
        if (player == null) return null;
        string name = element.ToString();
        Transform parent = GetParent();
        Transform elementTrans = parent.Find(name);
        if (elementTrans == null)
        {
            GameObject elementGo = Instantiate(Resources.Load(path + allElementPath[element])) as GameObject;
            elementGo.transform.parent = parent;
            elementGo.gameObject.SetActive(false);
            elementGo.name = name;
            elementTrans = elementGo.transform;
        }
        elementTrans.localEulerAngles = Vector3.zero;
        elementTrans.localPosition = Vector3.zero;
        elementTrans.localScale = Vector3.one;
        return elementTrans;
    }

    /// <summary>
    /// 隐藏所有元素
    /// </summary>
    void HideElements()
    {
        if (player == null) return;
        Transform parent = GetParent();
        for (int i = 0, length = parent.childCount; i < length; i++)
        {
            parent.GetChild(i).gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 隐藏指定元素
    /// </summary>
    /// <param name="element"></param>
    void HideElement(SKillAreaElement element)
    {
        if (player == null) return;
        Transform parent = GetParent();
        Transform elementTrans = parent.Find(element.ToString());
        if (elementTrans != null)
            elementTrans.gameObject.SetActive(false);
    }

    /// <summary>
    /// 每帧更新元素
    /// </summary>
    void UpdateElement()
    {
        switch (areaType)
        {
            case SkillAreaType.OuterCircle:
                break;
            case SkillAreaType.OuterCircle_InnerCube:
                UpdateElementPosition(SKillAreaElement.Cube);
                break;
            case SkillAreaType.OuterCircle_InnerSector:
                switch (angle)
                {
                    case 60:
                        UpdateElementPosition(SKillAreaElement.Sector60);
                        break;
                    case 120:
                        UpdateElementPosition(SKillAreaElement.Sector120);
                        break;
                    default:
                        break;
                }
                break;
            case SkillAreaType.OuterCircle_InnerCircle:
                UpdateElementPosition(SKillAreaElement.InnerCircle);
                break;
            default:
                break;
        }
    }
    void MouseDownAndCleanSkill(SKillAreaElement element)
    {
        switch (element)
        {
            case SKillAreaElement.OuterCircle:
                break;
            case SKillAreaElement.InnerCircle:
                break;
            case SKillAreaElement.Cube:
            case SKillAreaElement.Sector60:
            case SKillAreaElement.Sector120:
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 每帧更新元素位置
    /// </summary>
    /// <param name="element"></param>
    void UpdateElementPosition(SKillAreaElement element)
    {
        if (allElementTrans[element] == null)
            return;
        switch (element)
        {
            case SKillAreaElement.OuterCircle:
                break;
            case SKillAreaElement.InnerCircle:
                allElementTrans[element].transform.position = GetCirclePosition(outerRadius);
                break;
            case SKillAreaElement.Cube:
            case SKillAreaElement.Sector60:
            case SKillAreaElement.Sector120:
                allElementTrans[element].transform.LookAt(GetCubeSectorLookAt());
                break;
            default:
                break;
        }
        if (!allElementTrans[element].gameObject.activeSelf)
            allElementTrans[element].gameObject.SetActive(true);
    }

    /// <summary>
    /// 获取InnerCircle元素位置（改动）
    /// </summary>
    /// <returns></returns>
    Vector3 GetCirclePosition(float dist)
    {
        if (player == null) return Vector3.zero;

        Vector3 targetDir = deltaVec;
        return targetDir + player.transform.position;
    }

    /// <summary>
    /// 获取Cube、Sector元素朝向（改动）
    /// </summary>
    /// <returns></returns>
    Vector3 GetCubeSectorLookAt()
    {
        if (player == null) return Vector3.zero;
        
        Vector3 targetDir = deltaVec;
        return targetDir + player.transform.position;
    }
}
