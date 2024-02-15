
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class InputController
{
    protected bool m_isAllowed = true;

    public virtual string m_name { get;}

    public InputController m_manager;

    public List<InputController> m_employee = new List<InputController>();
    public Action<InputController> m_notifyParentIsOn, m_notifyParentIsFinished;

    public Dictionary<string, Dictionary<string, bool>> employeeRelationships = new Dictionary<string, Dictionary<string, bool>>();

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
        }
    }

    public void Block()
    {
        if (!m_isAllowed) Debug.Log(m_name + " is already blocked");
        else
        {
            m_isAllowed = false;
        }
    }

    public void AddController(InputController employee)
    {
        m_employee.Add(employee);
        //employee.m_manager = this;
        //employee.m_notifyParentIsOn = NotifyParentIsOn;
        //employee.m_notifyParentIsFinished = NotifyParentIsFinished;
        employeeRelationships.Add(employee.m_name, new Dictionary<string, bool>());
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

    public virtual void NotifyParentIsOn(InputController employee)
    {
        BlockOtherControllers(employee);

        // notify its parent
        m_notifyParentIsOn?.Invoke(this);
    }

    public virtual void NotifyParentIsFinished(InputController employee)
    {
        UnblockOtherControllers(employee);

        // notify its parent
        m_notifyParentIsFinished?.Invoke(this);
    }
}