using System.Collections.Generic;
using UnityEngine;

public abstract class InputController
{
    protected bool m_isAllowed = true;

    public abstract string m_name { get;}

    public InputController m_manager;

    public List<InputController> m_employee = new List<InputController>();

    public Dictionary<string, Dictionary<string, bool>> employeeRelationships = new Dictionary<string, Dictionary<string, bool>>();

    public Dictionary<string, bool> m_employeeStatus = new Dictionary<string, bool>();
    private int m_numberOfEmployeeOn = 0;

    protected InputController(InputController manager)
    {
        m_manager = manager;
    }

    public void Unblock()
    {
        if (m_isAllowed) Debug.Log(m_name + " is already unblocked");
        else
        {
            m_isAllowed = true;

            //unblock all my employees
            foreach (InputController employee in m_employee)
            {
                employee.Unblock();
            }
        }
    }

    public void Block()
    {
        if (!m_isAllowed) Debug.Log(m_name + " is already blocked");
        else
        {
            m_isAllowed = false;

            //block all my employees
            foreach (InputController employee in m_employee)
            {
                employee.Block();
            }
        }
    }

    public void AddController(InputController employee)
    {
        m_employee.Add(employee);
        employee.m_manager = this;

        employeeRelationships.Add(employee.m_name, new Dictionary<string, bool>());
        m_employeeStatus.Add(employee.m_name, false);
    }

    public void AddControllerRelationship(string firstEmployee, string secondEmployee, bool value)
    {
        if (!employeeRelationships.ContainsKey(firstEmployee)) Debug.LogError("Employee " + firstEmployee + " does not exist");
        if (!employeeRelationships.ContainsKey(secondEmployee)) Debug.LogError("Employee " + secondEmployee + " does not exist");
        if (employeeRelationships[firstEmployee].ContainsKey(secondEmployee)) Debug.LogError("Relationship between " + firstEmployee + " and " + secondEmployee + " already exists");
        employeeRelationships[firstEmployee].Add(secondEmployee, value);
        employeeRelationships[secondEmployee].Add(firstEmployee, value);
    }

    public void BlockOtherControllers(InputController initiater)
    {
        string initiaterName = initiater.m_name;
        foreach (InputController employee in m_employee)
        {
            if (employee == initiater) continue;

            if (!employeeRelationships[initiaterName][employee.m_name])
            {
                employee.Block();
            }
            
        }
    }

    public void UnblockOtherControllers(InputController initiater)
    {
        string initiaterName = initiater.m_name;
        foreach (InputController employee in m_employee)
        {
            if (employee == initiater) continue;

            // if everything goes as planned, only certain amount of employee will need to be turned on
            if (!employeeRelationships[initiaterName][employee.m_name])
            {
                employee.Unblock();
            }
        }
    }

    public void NotifyMyParentIsOn()
    {
        m_manager?.NotifyParentFromChildrenIsOn(this);
    }

    public void NotifyMyParentIsFinished()
    {
        m_manager?.NotifyParentFromChildrenIsFinished(this);
    }

    public virtual void NotifyParentFromChildrenIsOn(InputController employee)
    {
        BlockOtherControllers(employee);

        if (!m_employeeStatus.ContainsKey(employee.m_name))
        {
            Debug.LogError("Can not find employee " + employee.m_name + " controller");
            return;
        }
        
        // notify its parent
        
        // if this is the first employee to be turned on, notify my parent
        if (m_numberOfEmployeeOn == 0)
        {
            NotifyMyParentIsOn();
        }

        if (!m_employeeStatus[employee.m_name])
        {
            m_employeeStatus[employee.m_name] = true;
            m_numberOfEmployeeOn++;
        }

    }

    public virtual void NotifyParentFromChildrenIsFinished(InputController employee)
    {
        UnblockOtherControllers(employee);
        
        if (!m_employeeStatus.ContainsKey(employee.m_name))
        {
            Debug.LogError("Can not find employee " + employee.m_name + " controller");
            return;
        }

        if (!m_employeeStatus[employee.m_name])
        {
            Debug.LogError("Employee " + employee.m_name + " is already finished");
            return;
        }

        m_numberOfEmployeeOn--;
        m_employeeStatus[employee.m_name] = false;

        if (m_numberOfEmployeeOn == 0)
        {
            NotifyMyParentIsFinished();
        }

    }
}