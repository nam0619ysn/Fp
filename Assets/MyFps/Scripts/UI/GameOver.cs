using UnityEngine;

namespace MyFps
{
    //���ӿ��� ó��: �޴�����, �ٽ��ϱ�
    public class GameOver : MonoBehaviour
    {
        #region Variables
        public SceneFader fader;
        [SerializeField]
        private string loadToScene = "PalyScene";
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //Ŀ�� ����
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            //FadeIn ȿ��
            fader.FadeStart(0f);
        }
        #endregion

        #region Custom Method
        public void Retry()
        {
            fader.FadeTo(loadToScene);
        }

        public void Menu()
        {
            Debug.Log("Goto Menu!!!");
        }
        #endregion
    }
}
