using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public class CatComponent : GameFrameworkComponent
    {
        //èè״̬���ݣ�����0ΪĬ�ϵ�״̬��Ҳ�ǵ�����������������ʱ��״̬��һ����Ϊ���״̬
        private List<CatStateData> catStateDatas = new List<CatStateData>();
        private CatStateData catState;
        private Dictionary<BehaviorTag, BehaviorData> behaviors = new Dictionary<BehaviorTag, BehaviorData>();
        private CatData mCatData=new CatData();

        private void Start()
        {
            catStateDatas.Clear();
            CatStateSO[] catStateSOs = Resources.LoadAll<CatStateSO>("CatStateData");
            foreach (CatStateSO catStateSO in catStateSOs)
            {
                catStateDatas.Add(catStateSO.catStateData);
            }
        }
        public CatData GetSaveData()
        { 
            return mCatData;
        }
        public void LoadData(CatData charData)
        {
            mCatData = charData;
        }
        public int Closet
        {
            get
            {
                return Mathf.Clamp(mCatData.closet, 1002, 1006);
            }
            set
            {
                mCatData.closet = value;
                GameEntry.Event.FireNow(this, CatDataEventArgs.Create(mCatData));
            }
        }
        public int Wisdom
        {
            get
            {
                return mCatData.wisdom;
            }
            set
            {
                mCatData.wisdom = value;
                GameEntry.Utils.AddValue(TriggerTag.Wisdom, mCatData.wisdom.ToString());
                GameEntry.Event.FireNow(this, CatDataEventArgs.Create(mCatData));
            }
        }
        public int Charm
        {
            get
            {
                return mCatData.charm;
            }
            set
            {
                mCatData.charm = value;
                GameEntry.Utils.AddValue(TriggerTag.Charm, mCatData.charm.ToString());
                GameEntry.Event.FireNow(this, CatDataEventArgs.Create(mCatData));
            }
        }
        public int Stamina
        {
            get
            {
                return mCatData.stamina;
            }
            set
            {
                mCatData.stamina = value;
                GameEntry.Utils.AddValue(TriggerTag.Stamina, mCatData.stamina.ToString());
                GameEntry.Event.FireNow(this, CatDataEventArgs.Create(mCatData));
            }
        }
        public int Favor
        {
            get
            {
                return mCatData.favor;
            }
            set
            {
                mCatData.favor = value;
                GameEntry.Utils.AddValue(TriggerTag.Favor, mCatData.favor.ToString());
                GameEntry.Event.FireNow(this, CatDataEventArgs.Create(mCatData));
            }
        }
        public int CharmLevel
        {
            get
            {
                return Mathf.Min(mCatData.charm / 40 + 1, 4);
            }
        }
        public int WisdomLevel
        {
            get
            {
                return Mathf.Min(mCatData.wisdom / 40 + 1, 4);
            }
        }
        public int StaminaLevel
        {
            get
            {
                return Mathf.Min(mCatData.stamina / 40 + 1, 4);
            }
        }
        public CatStateData GetCatState()
        {
            UpdateState();
            return catState;
        }
        public BehaviorData GetBehavior(BehaviorTag behaviorTag)
        {
            UpdateState();
            return behaviors[behaviorTag];
        }
        public void UpdateState()
        {
            //�����ǰ��Ч��ֱ������
            if (catState!=null&&GameEntry.Utils.Check(catState.trigger))
                return;
            for (int i=0;i<catStateDatas.Count;i++)
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
                    continue;
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
    public class BehaviorData
    {
        public BehaviorTag behaviorTag;
        public string behaviorName;
        public ValueData valueData;
        public List<DialogueGraph> dialogues;
    }
    [System.Serializable]
    public class ValueData
    {
        public int energy;//���ѵ�����
        public int ap;//���ѵ�AP
        public int money;//���ѵĽ�Ǯ
        public int favor;//�øж�
        public int stamina;//����
        public int wisdom;//�ǻ�
        public int charm;//����

        public ValueData(ValueData valueData)
        {
            energy = valueData.energy;
            ap=valueData.ap;//���ѵ�AP
            money=valueData.money;//���ѵĽ�Ǯ
            favor=valueData.favor;//�øж�
            stamina=valueData.stamina;//����
            wisdom=valueData.wisdom;//�ǻ�
            charm=valueData.charm;//����
    }
        public Dictionary<ValueTag, int> GetValueTag(Dictionary<ValueTag, int> dic)
        {
            dic.Add(ValueTag.Stamina, stamina);
            dic.Add(ValueTag.Wisdom, wisdom);
            dic.Add(ValueTag.Charm, charm);
            return dic;
        }
    }
    [System.Serializable]
    public class CatData
    {
        public int closet;
        public int favor;//�øж�
        public int stamina;//����
        public int wisdom;//�ǻ�
        public int charm;//����
        public int StaminaLevel
        {
            get
            {
                return Mathf.Min(stamina / 40 + 1, 4);
            }
        }//����
        public int WisdomLevel
        {
            get
            {
                return Mathf.Min(wisdom / 40 + 1, 4);
            }
        }//�ǻ�
        public int CharmLevel
        {
            get
            {
                return Mathf.Min(charm / 40 + 1, 4);
            }
        }//����

        public CatData() { }
    }
    public enum BehaviorTag
    {
        Click,
        Talk,
        TV,
        Touch,
        Sleep,
        Morning,
        Teach,
        Sport,
        Read,
        Augur,
        Out
    }
}
