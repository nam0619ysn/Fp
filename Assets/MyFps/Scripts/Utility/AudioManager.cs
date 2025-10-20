using UnityEngine;

namespace MyFps
{
    //사운드를 관리하는 싱글톤 클래스
    public class AudioManager : Singleton<AudioManager>
    {
        #region Variables
        //사운드 데이터 목록
        public Sound[] sounds;

        //배경음 이름
        private string bgmSound = "";
        #endregion

        #region Unity Event Method
        protected override void Awake()
        {
            base.Awake();

            //사운드 데이터 셋팅
            foreach(var s in sounds)
            {
                //AudioSource 컴포넌트 추가 생성
                s.source = this.gameObject.AddComponent<AudioSource>();

                //AudioSource 컴포넌트 데이터 셋팅
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;

                s.source.playOnAwake = false;
            }
        }
        #endregion

        #region Custom Method
        //사운드 플레이
        public void Play(string name)
        {
            Sound sound = null;
            //사운드 목록에서 같은 이름의 사운드 찾기
            foreach (var s in sounds)
            {
                if(s.name == name)
                {
                    sound = s;
                    break;
                }
            }

            //찾았는지 체크
            if(sound == null)
            {
                Debug.Log("Cannot Find " + name + " Sound");
                return;
            }

            sound.source.Play();
        }

        //사운드 정지
        public void Stop(string name)
        {
            Sound sound = null;

            //사운드 목록에서 같은 이름의 사운드 찾기
            foreach (var s in sounds)
            {
                if (s.name == name)
                {
                    sound = s;
                    break;
                }
            }

            //찾았는지 체크
            if (sound == null)
            {
                Debug.Log("Cannot Find " + name + " Sound");
                return;
            }

            sound.source.Stop();
        }

        //배경음 플레이
        public void PlayBgm(string name)
        {
            //배경음 이름 체크
            if(bgmSound == name)
            {
                return;
            }

            //현재 플레이 되고 있는 배경음 스톱
            Stop(bgmSound);

            //배경음 플레이
            Sound sound = null;

            //사운드 목록에서 같은 이름의 사운드 찾기
            foreach (var s in sounds)
            {
                if (s.name == name)
                {
                    sound = s;
                    //배경음 이름 저장
                    bgmSound = name;
                    break;
                }
            }

            //찾았는지 체크
            if (sound == null)
            {
                Debug.Log("Cannot Find " + name + " Bgm Sound");
                return;
            }

            sound.source.Play();
        }

        public void StopBgm()
        {
            //배경음 정지
            Stop(bgmSound);
        }
        #endregion
    }
}
