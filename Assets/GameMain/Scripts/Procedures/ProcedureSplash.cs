using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.Localization;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework.Procedure;
using GameFramework.Resource;

namespace GameMain
{
    public class ProcedureSplash : ProcedureBase
    {
        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            // TODO: ������Բ���һ�� Splash ����
            // ...

            if (GameEntry.Base.EditorResourceMode)
            {
                // �༭��ģʽ
                Log.Info("Editor resource mode detected.");
                ChangeState<ProcedurePreload>(procedureOwner);
            }
            else if(GameEntry.Resource.ResourceMode == ResourceMode.Package)
            {
                // ����ģʽ
                Log.Info("Package resource mode detected.");
                ChangeState<ProcedureInitResources>(procedureOwner);
            }
            //else
            //{
            //    // �ɸ���ģʽ
            //    Log.Info("Updatable resource mode detected.");
            //    //ChangeState<ProcedureCheckVersion>(procedureOwner);
            //}
        }
    }
}
