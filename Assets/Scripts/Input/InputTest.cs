using UnityEngine;

public class InputTest : MonoBehaviour
{
    private void Start()
    {
        Test();
    }
    public void Test()
    {
        // This is a test
        InputController manager = new A(null);
        C1 C1 = manager.m_employee[0].m_employee[0] as C1;
        C2 C2 = manager.m_employee[1].m_employee[0] as C2;
        C3 C3 = manager.m_employee[1].m_employee[1] as C3;

        //C1.inputTest();
        //C2.inputTest();
        //C2.InputFinished();
        //C3.inputTest();
        //C3.InputFinished();
        //C1.InputFinished();

        C2.inputTest();
        C3.inputTest();
        C1.inputTest();
        C3.InputFinished();
        C1.inputTest();
        C2.InputFinished();
        C1.inputTest();
    }
}

public class  A: InputController
{
    public A(InputController manager): base(manager)
    {
        AddController(new B1(this));
        AddController(new B2(this));

        AddControllerRelationship("B1", "B2", false);
    }

    public override string m_name => "A";


}

public class  B1: InputController
{
    public B1(InputController manager): base(manager)
    {
        AddController(new C1(this));
    }

    public override string m_name => "B1";
}

public class  B2: InputController
{
    public B2(InputController manager): base(manager)
    {
        AddController(new C2(this));
        AddController(new C3(this));

        AddControllerRelationship("C2", "C3", true);
    }

    public override string m_name => "B2";
}

public class  C1: InputController
{
    public C1(InputController manager): base(manager)
    {
        
    }

    public void inputTest()
    {
        if (!m_isAllowed) return;
        NotifyMyParentIsOn();
        Debug.Log("C1");
        
    }

    public void InputFinished()
    {
        if (!m_isAllowed) return;
        Debug.Log("C1 finished");
        NotifyMyParentIsFinished();
    }

    public override string m_name => "C1";
}


public class  C2: InputController
{
    public C2(InputController manager): base(manager)
    {
        
    }

    public void inputTest()
    {
        if (!m_isAllowed) return;
        NotifyMyParentIsOn();
        Debug.Log("C2");
    }

    public void InputFinished()
    {
        if (!m_isAllowed) return;
        Debug.Log("C2 finished");
        NotifyMyParentIsFinished();
    }

    public override string m_name => "C2";
}

public class  C3: InputController
{
    public C3(InputController manager): base(manager)
    {

    }

    public void inputTest()
    {
        if (!m_isAllowed) return;
        NotifyMyParentIsOn();
        Debug.Log("C3");

    }

    public void InputFinished()
    {
        if (!m_isAllowed) return;
        Debug.Log("C3 finished");
        NotifyMyParentIsFinished();
    }

    public override string m_name => "C3";
}