using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//추상클래스 기반 아이템 Class들
public abstract class Item
{
    //일단 모든 수치는 50으로 통일해둔다
    protected string m_s_name;//아이템 이름
    protected int m_i_attackPower;//기본 공격력
    protected int m_i_addHP;//hp증가치
    protected int m_i_addPower;//무기 강화시 공격력 증가치

    
    public abstract void Info(); //아이템 설명

    //공통상속 함수
    public abstract string Name { get; set; } //이름 셋팅

    //모든 자식이 공통적으로 쓰지는 않는 함수는 이렇게 쓰는게 맞나??
    //new 선언시 초록줄이 사라지긴한데 다운캐스팅이 되는게 맞나??
    //virtual 선언 -> 자식이 오버라이딩시 자식껄로 재정의된대서 버츄얼로 선언..이거시 맞나..ㅠ
    public virtual int BasePower { get; set; }//기본공격력 셋팅
    public virtual int AddHP { get; set; }//힐량 셋팅
    public virtual int AddPower { get; set; }//강화량 셋팅
}
//나무가지
public class Branch: Item
{ 
   public override string Name
    {
        get { return this.m_s_name; }
        set { this.m_s_name = "나무가지"; }
    }
    public override void Info()
    {
        Debug.Log("나중에 이 함수안에는 UI 아이템 설명넣기!");
    }
}

//죽창
public class Spear : Item
{
    public override string Name
    {
        get { return this.m_s_name; }
        set { this.m_s_name = "죽창"; }
    }
    public override int BasePower
    {
        get { return this.m_i_attackPower; }
        set { this.m_i_attackPower = 50; }
    }
    public override void Info()
    {
        Debug.Log("나중에 이 함수안에는 UI 아이템 설명넣기!");        
    }
}
//물병 HP++
public class Water : Item
{
    public override string Name
    {
        get { return this.m_s_name; }
        set { this.m_s_name = "물병"; }
    }
    public override int AddHP
    {
        get { return this.m_i_addHP; }
        set { this.m_i_addHP = 50; }
    }
    public override void Info()
    {
        Debug.Log("나중에 이 함수안에는 UI 아이템 설명넣기!");
    }
}
//조개 HP++
public class Shell : Item
{
    public override string Name
    {
        get { return this.m_s_name; }
        set { this.m_s_name = "조개"; }
    }
    public override int AddHP
    {
        get { return this.m_i_addHP; }
        set { this.m_i_addHP = 50; }
    }
    public override void Info()
    {
        Debug.Log("나중에 이 함수안에는 UI 아이템 설명넣기!");
    }
}
//무기 강화 아이템
public class Enhance : Item
{
    public override string Name
    {
        get { return this.m_s_name; }
        set { this.m_s_name = "무기강화아이템"; }
    }
    public override int AddPower
    {
        get { return this.m_i_addPower; }
        set { this.m_i_addPower = 50; }
    }
    public override void Info()
    {
        Debug.Log("나중에 이 함수안에는 UI 아이템 설명넣기!");
    }
}