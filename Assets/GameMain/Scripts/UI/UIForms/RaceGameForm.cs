using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace GameMain
{
    public class RaceGameForm : BaseForm
    {
        private float score;
        [SerializeField] private float targetScore;
        [SerializeField] Text scoreTitle;
        [SerializeField] Text timeTitle;

        [SerializeField] List<Transform> genNodes;
        [SerializeField] List<int> genNodeCount;
        [SerializeField] List<GameObject> barrierList;
        [SerializeField] float barrierGenInterval;//障碍生成间隔
        [Range(0.01f, 0)]
        [SerializeField] float BGIIncreaseRate;//障碍生成间隔增长率
        private float currentBGI;
        [SerializeField] float barrierSpeed;//障碍速度
        private float currentBS;
        [Range(1, 0)]
        [SerializeField] float BSIncreaseRate;//障碍速度增长率
        private float m_genTimer;
        private bool m_done;
        private List<GameObject> barriers = new List<GameObject>();

        [SerializeField] private float gameTime;
        private float m_gameTimer;

        [SerializeField] GameObject player;
        private int m_playerPosition;

        private Action mAction;
        [SerializeField] private ValueData mValueData;

        [SerializeField] private Button startBtn;
        private bool m_start;

        private bool m_fire;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            mAction = BaseFormData.Action;
            m_fire = false;
            scoreTitle.gameObject.SetActive(false);
            timeTitle.gameObject.SetActive(false);
            startBtn.gameObject.SetActive(true);

            startBtn.onClick.AddListener(OnClickStartBtn);
            m_start = false;
            m_done = false;

            currentBGI = barrierGenInterval;
            currentBS = barrierSpeed;
            m_genTimer = currentBGI;
            m_gameTimer = gameTime * (float)(2 / GameEntry.Cat.StaminaLevel);

            foreach (Transform t in genNodes)
            {
                foreach(Transform child in t)
                {
                    Destroy(child.gameObject);
                }
            }

            m_playerPosition = 1;
            for (int i = 0; i < genNodes.Count; i++)
                genNodeCount.Add(0);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
            startBtn.onClick.RemoveAllListeners();
        }
        private void OnComplete()
        {
            if (m_fire)
                return;
            m_fire = true;
            float power = (gameTime * (float)(2 / GameEntry.Cat.StaminaLevel) - m_gameTimer) / gameTime * (float)(2 / GameEntry.Cat.StaminaLevel);
            ValueData newValueData = new ValueData(mValueData);
            newValueData.stamina = (int)(mValueData.stamina * power);
            newValueData.money = (int)(mValueData.money * power);
            GameEntry.Player.Ap -= newValueData.ap;
            GameEntry.Player.Money += newValueData.money;
            GameEntry.Cat.Stamina += newValueData.stamina;
            GameEntry.UI.OpenUIForm(UIFormId.CompleteForm, OnExit, newValueData);
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (!m_start)
                return;

            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if(m_done)
            {
                m_done = false;
                OnComplete();
                return;
            }

            m_genTimer -= Time.deltaTime;
            m_gameTimer -= Time.deltaTime;
            scoreTitle.text = ((int)score).ToString();
            timeTitle.text = FlopForm.FormatTime((int)m_gameTimer);

            if (!m_done)
                score += (1 + currentBS / 5) * Time.deltaTime;

            if (currentBS <= 30)
                currentBS += Time.deltaTime * BSIncreaseRate;

            if (currentBGI >= 0.05f)
                currentBGI -= Time.deltaTime * BGIIncreaseRate;


            if (m_genTimer <= 0 && !m_done)
            {
                var nodeIndex = GenNodeIndex();

                GenOnSide(nodeIndex);

                m_genTimer = currentBGI;
            }

            MoveVertical();

            if (PlayerCollisionResult() > 0)
            {
                m_done = true;
                foreach (GameObject barrier in barriers)
                {
                    DOTween.To(() => barrier.GetComponent<Rigidbody2D>().velocity, x => barrier.GetComponent<Rigidbody2D>().velocity = x, Vector2.zero, 2f).SetEase(Ease.OutQuad);
                }
                Debug.Log("GameOver");
            }
            else if(m_gameTimer <= 0)
            {
                m_done = true;
            }
        }
       
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Destroy(collision.gameObject);
        }

        private void MoveVertical()
        {
            if(Input.GetKeyDown(KeyCode.W))
            {
                if (m_playerPosition > 0)
                    m_playerPosition--;
            }
            else if(Input.GetKeyDown(KeyCode.S))
            {
                if(m_playerPosition < 2)
                    m_playerPosition++;
            }
            else { }

            player.transform.DOMoveY(genNodes[m_playerPosition].position.y, 0.2f, true);
        }

        private int GenNodeIndex()
        {
            int index;
            do
            {
                index = UnityEngine.Random.Range(0, genNodes.Count);
                genNodeCount[index]++;
            } while (genNodeCount[index] > 2);

            for(int i = 0; i < genNodeCount.Count; i++)
            {
                if (i != index)
                    genNodeCount[i] = 0;
            }

            return index;
        }

        private int PlayerCollisionResult()
        {
            ContactFilter2D filter2D = new ContactFilter2D();
            filter2D.NoFilter();
            List<Collider2D> result = new List<Collider2D>();
            player.GetComponent<BoxCollider2D>()?.OverlapCollider(filter2D, result);
            return result.Count;
        }

        private void OnExit()
        {
            mAction?.Invoke();
            GameEntry.UI.CloseUIForm(this.UIForm);
        }

        private void OnClickStartBtn()
        {
            m_start = true;
            scoreTitle.gameObject.SetActive(true);
            timeTitle.gameObject.SetActive(true);
            startBtn.gameObject.SetActive(false);
        }

        private void GenOnCenter(int index)
        {
            Transform node = genNodes[index];
            GameObject barrier = barrierList[UnityEngine.Random.Range(0, barrierList.Count)];

            GameObject go = Instantiate(barrier, node);
            barriers.Add(go);
            go.GetComponent<Rigidbody2D>().velocity = new Vector3(-currentBS * 100, 0, 0);
        }

        private void GenOnSide(int index)
        {
            List<Transform> nodes = new List<Transform>();
            foreach(Transform node in genNodes)
            {
                if(node != genNodes[index])
                {
                    nodes.Add(node);
                }
            }

            foreach(Transform node in nodes)
            {
                GameObject barrier = barrierList[UnityEngine.Random.Range(0, barrierList.Count)];
                GameObject go = Instantiate(barrier, node);
                barriers.Add(go);
                go.GetComponent<Rigidbody2D>().velocity = new Vector3(-currentBS * 100, 0, 0);
            }
        }
    }
}


