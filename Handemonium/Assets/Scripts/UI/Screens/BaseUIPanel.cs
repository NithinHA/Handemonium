using UnityEngine;

namespace RPSLS.UI
{
    public abstract class BaseUIPanel : MonoBehaviour
    {
        [SerializeField] protected GameObject m_Container;

        public virtual void Show()
        {
            m_Container.SetActive(true);
        }

        public virtual void Hide()
        {
            m_Container.SetActive(false);
        }
    }
}