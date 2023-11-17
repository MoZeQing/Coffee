using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class CatComponent : GameFrameworkComponent
    {
        private int m_Favour = 0;
        private int m_Love = 0;
        private int m_Family = 0;
        //èè״̬���ݣ�����0ΪĬ�ϵ�״̬��Ҳ�ǵ�����������������ʱ��״̬��һ����Ϊ���״̬
        private List<CatStateData> catStateDatas = new List<CatStateData>();
        private CatStateData catState;
        private Dictionary<BehaviorTag, BehaviorData> behaviors = new Dictionary<BehaviorTag, BehaviorData>();

        private void Start()
        {
            catStateDatas.Clear();
            CatStateSO[] catStateSOs = Resources.LoadAll<CatStateSO>("CatStateData");
            foreach (CatStateSO catStateSO in catStateSOs)
            {
                catStateDatas.Add(catStateSO.catStateData);
            }
            UpdateState();
        }
        public int Favour
        {
            get
            {
                return m_Favour;
            }
            private set { }
        }
        public int Family
        {
            get
            {
                return m_Family;
            }
            private set { }
        }
        public int Love
        {
            get
            {
                return m_Love;
            }
            private set { }
        }
        public CatStateData GetCatState()
        {
            return catState;
        }
        public BehaviorData GetBehavior(BehaviorTag behaviorTag)
        {
            return behaviors[behaviorTag];
        }
        public void UpdateState()
        {
            //�����ǰ��Ч��ֱ������
            if (GameEntry.Utils.Check(catState.trigger))
                return;
            catState = catStateDatas[0];
            for (int i=1;i<catStateDatas.Count;i++)
            {
                CatStateData stateData = catStateDatas[i];
                if (GameEntry.Utils.Check(stateData.trigger))
                {
                    catState = stateData;
                    behaviors.Clear();
                    foreach (BehaviorData behavior in catState.behaviors)
                    {
                        behaviors.Add(behavior.behaviorTag, behavior);
                    }
                    return;
                }
            }
        }
    }
    [System.Serializable]
    public class CatStateData
    {
        public ParentTrigger trigger;
        public List<BehaviorData> behaviors;
    }
    [System.Serializable]
    public class CatData
    {
        public int favour = 0;
        public int love = 0;
        public int family = 0;
    }
    [System.Serializable]
    public class BehaviorData
    {
        public BehaviorTag behaviorTag;
        public PlayerData playerData;
        public CatData catData;
        public List<DialogueGraph> dialogues;
    }

    public enum BehaviorTag
    {
        Click,
        Clean,
        Play,
        Talk,
        Bath,
        TV,
        Story,
        Touch,
        Rest,
        Sleep,
        Morning
    }
}
