using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButtonDrawer : MonoBehaviour
{
    [SerializeField] Transform m_gridRoot;
    [SerializeField] SkillButton m_skillBtnPrefab;
    private Dictionary<SkillType, int> m_skillCollecteds;
    public void DrawSkillButton()
    {
        Helper.ClearChilds (m_gridRoot);
        m_skillCollecteds= FindObjectOfType<SkillManager>().SkillCollecteds;
        if (m_skillCollecteds == null || m_skillCollecteds.Count <= 0) return;
        foreach (var skillCollected in m_skillCollecteds)
        {
            var skillButtonClone = Instantiate(m_skillBtnPrefab);
            Helper.AssignToRoot(m_gridRoot, skillButtonClone.transform, Vector3.zero,Vector3.one);
            skillButtonClone.Initialize(skillCollected.Key);
        }
    }
}
