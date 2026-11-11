using System;
using System.Collections;
using Game.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Fantasya
{
    /// <summary>
    /// 키보드와 마이크 입력을 분리하고 명시적으로 시작한 녹음만 로컬에서 문장으로 변환한다.
    /// </summary>
    public sealed class AgentVoiceInput : MonoBehaviour
    {
        [SerializeField] private AgentConversationPanel _panel;
        [SerializeField] private AgentCompanionLink _link;
        [SerializeField] private GameObject _textGroup;
        [SerializeField] private GameObject _voiceGroup;
        [SerializeField] private Text _keyboardHint;
        [SerializeField] private Text _status;
        [SerializeField] private InputField _review;
        [SerializeField] private Button _record;
        [SerializeField] private Button _stop;
        [SerializeField] private Button _textMode;
        [SerializeField] private Button _voiceMode;
        [SerializeField] private Button _confirm;
        private AudioClip _clip;
        private Coroutine _recognition;
        private float _recordStarted;
        private string _companionId;
        private int _requestVersion;
        public bool IsVoiceMode { get; private set; }
        public bool IsRecording => _clip != null;
        public InputField ReviewInput => _review;

        private void Start() => SelectTextMode();

        private void Update()
        {
            _confirm.interactable = _clip == null && _recognition == null && !_panel.IsSending && !string.IsNullOrWhiteSpace(_review.text);
            if (_clip == null)
            {
                return;
            }
            float elapsed = Time.realtimeSinceStartup - _recordStarted;
            _status.text = "녹음 중 " + elapsed.ToString("F1") + "/30초 · 인식 후 문장을 확인해 주세요";
            if (elapsed > 3f && Microphone.GetPosition(null) == 0 && Microphone.IsRecording(null))
            {
                Cancel();
                _status.text = "마이크 입력이 없습니다. 장치와 Windows 마이크 권한을 확인해 주세요.";
            }
            else if (elapsed >= 29.9f || !Microphone.IsRecording(null))
            {
                StopAndRecognize();
            }
        }

        private void OnDisable() => Cancel();

        /// <summary>
        /// 두 입력 그룹을 전환하되 모드 전환 시 녹음과 미완료 음성 요청을 취소한다.
        /// </summary>
        private void SelectMode(bool voice)
        {
            Cancel();
            IsVoiceMode = voice;
            _textGroup.SetActive(!voice);
            _voiceGroup.SetActive(voice);
            _keyboardHint.gameObject.SetActive(!voice);
            _textMode.interactable = voice;
            _voiceMode.interactable = !voice;
            _status.text = "녹음 시작 → 인식 → 문장 확인 → 보내기";
        }

        /// <summary>
        /// 마이크를 종료하고 별도 키보드 초안을 사용하는 채팅 입력 모드로 전환한다.
        /// </summary>
        public void SelectTextMode() => SelectMode(false);

        /// <summary>
        /// 녹음을 자동 시작하지 않고 음성 인식·확인용 입력 모드로 전환한다.
        /// </summary>
        public void SelectVoiceMode() => SelectMode(true);

        /// <summary>
        /// 사용자가 누른 시작 버튼에서만 기본 마이크를 열며 최대 30초를 메모리에 녹음한다.
        /// </summary>
        public void BeginRecording()
        {
            if (!IsVoiceMode || _recognition != null || _clip != null || _panel.Selected == null)
            {
                return;
            }
            if (Microphone.devices.Length == 0)
            {
                _status.text = "연결된 마이크가 없습니다.";
                return;
            }
            _companionId = _panel.Selected.CompanionId;
            _review.text = string.Empty;
            try
            {
                _clip = Microphone.Start(null, false, 30, 16000);
                if (_clip == null)
                {
                    throw new InvalidOperationException();
                }
                _recordStarted = Time.realtimeSinceStartup;
                _record.interactable = false;
                _stop.interactable = true;
            }
            catch (Exception)
            {
                _status.text = "녹음을 시작하지 못했습니다. 마이크 사용 권한을 확인해 주세요.";
            }
        }

        /// <summary>
        /// 녹음을 먼저 종료한 뒤 현재 동료에 묶인 문장 인식을 시작하고 자동 전송하지 않는다.
        /// </summary>
        public void StopAndRecognize()
        {
            if (_clip == null)
            {
                return;
            }
            int samples = Microphone.GetPosition(null);
            if (Time.realtimeSinceStartup - _recordStarted >= 29.9f)
            {
                samples = _clip.samples;
            }
            Microphone.End(null);
            AudioClip clip = _clip;
            _clip = null;
            _stop.interactable = false;
            RecognizeRecording(clip, samples);
            Destroy(clip);
        }

        /// <summary>
        /// 확정된 녹음 샘플만 인식 요청으로 넘기며 결과는 전송하지 않고 별도 검토 입력에 넣는다.
        /// </summary>
        public void RecognizeRecording(AudioClip clip, int samples)
        {
            if (!IsVoiceMode || _recognition != null || _panel.Selected == null)
            {
                return;
            }
            _companionId = _panel.Selected.CompanionId;
            try
            {
                if (samples < 4000)
                {
                    throw new FormatException();
                }
                byte[] wave = AudioWaveEncoder.EncodePcm16Mono(clip, samples);
                _status.text = "로컬 음성 인식 중… 인식된 문장은 전송 전 확인할 수 있어요.";
                _recognition = StartCoroutine(Recognize(wave, ++_requestVersion));
            }
            catch (Exception)
            {
                _record.interactable = true;
                _status.text = "목소리가 너무 짧거나 녹음 형식이 잘못됐습니다.";
            }
        }

        private IEnumerator Recognize(byte[] wave, int version)
        {
            yield return null;
            yield return _link.Transcribe(wave, (success, text, milliseconds) =>
            {
                if (version != _requestVersion || !IsVoiceMode || _panel.Selected?.CompanionId != _companionId)
                {
                    return;
                }
                if (success)
                {
                    _review.text = text;
                    _status.text = "인식 " + milliseconds.ToString("F0") + "ms · 문장을 확인하고 보내기를 눌러 주세요.";
                }
                else
                {
                    _status.text = text;
                }
            });
            _recognition = null;
            _record.interactable = true;
        }

        /// <summary>
        /// 동료 전환·모드 종료·비활성화 시 녹음과 요청을 정리하고 이전 동료의 문장을 폐기한다.
        /// </summary>
        public void Cancel()
        {
            _requestVersion++;
            if (_clip != null)
            {
                Microphone.End(null);
                Destroy(_clip);
                _clip = null;
            }
            if (_recognition != null)
            {
                StopCoroutine(_recognition);
                _recognition = null;
            }
            _link.CancelSpeech();
            _review.text = string.Empty;
            _record.interactable = true;
            _stop.interactable = false;
            _status.text = "녹음하지 않는 중 · 녹음 시작 버튼을 눌러 주세요.";
        }

    }
}
