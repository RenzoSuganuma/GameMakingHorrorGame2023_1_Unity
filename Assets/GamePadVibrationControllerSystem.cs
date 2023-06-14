using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �Q�[���p�b�h�̐U������N���X
/// </summary>
public class GamePadVibrationControllerSystem : MonoBehaviour
{
    /// <summary>
    /// ���ݎh�����Ă���Q�[���p�b�h
    /// </summary>
    private Gamepad _gamepad;

    /// <summary>
    /// �U���̎��g��
    /// </summary>
    private float _motorMinFrecuency = .1f, _motorMaxFrecuency = .5f;

    /// <summary>
    /// �U�����t���O
    /// </summary>
    public bool _isVivrate = false;

    /// <summary>
    /// �V�[���ǂݍ��ݎ��ɃQ�[���p�b�h�̐U�����~�߂�
    /// </summary>
    [SerializeField] bool _forceStopViverate = false;

    private void Start()
    {
        if(Gamepad.current != null)//null check
            _gamepad = Gamepad.current;

        if (this._forceStopViverate)
        {
            StopGamepadViverate();
        }
    }

    /// <summary>
    /// �u�b�u�b�ƃQ�[���p�b�h��U��������
    /// </summary>
    /// <param name="frames"></param>
    /// <param name="repeat"></param>
    public void GamepadViverateRapid(int frames,int repeat)
    {
        StartCoroutine(VivrateTheGamepadRapidlyNow(frames, repeat));
    }

    /// <summary>
    /// �u�[�[�[�ƃQ�[���p�b�h��U��������
    /// </summary>
    /// <param name="frames"></param>
    public void GamepadViverate(int frames)
    {
        StartCoroutine(VivrateTheGamepadLongNow(frames));
    }

    public void StopGamepadViverate()
    {
        if(this._gamepad != null)
            _gamepad.SetMotorSpeeds(0f, 0f); // �U�����~���܂�
    }

    #region �U���F�u�b�u�b
    /// <summary>
    /// �u�b�u�b �ƐU������
    /// </summary>
    /// <param name="frames"></param>
    /// <param name="repeat"></param>
    /// <returns></returns>
    IEnumerator VivrateTheGamepadRapidlyNow(int frames, int repeat)
    {
        this._isVivrate = true;//�U�����t���O 1

        for (int timeCount = 0; timeCount < repeat; timeCount++)
        {
            for (int cnt = 0; cnt < frames; cnt++)
            {
                _gamepad.SetMotorSpeeds(this._motorMinFrecuency, this._motorMaxFrecuency); // �U���̋�����ݒ肵�܂�
                    yield return null;
            }_gamepad.SetMotorSpeeds(0f, 0f); // �U�����~���܂�

            for (int cnt = 0; cnt < 3; cnt++)
            {
                _gamepad.SetMotorSpeeds(0f, 0f); // �U�����~���܂�
                    yield return null;
            }_gamepad.SetMotorSpeeds(0f, 0f); // �U�����~���܂�
        }

        this._isVivrate = false;//�U�����t���O 0
    }
    #endregion

    #region �U���F�u�[�[�[
    /// <summary>
    /// �u�b�[�[�[�[ �ƐU������
    /// </summary>
    /// <param name="frames"></param>
    /// <returns></returns>
    IEnumerator VivrateTheGamepadLongNow(int frames)
    {
        this._isVivrate = true;//�U�����t���O 1

        for (int cnt = 0; cnt < frames; cnt++)
        {
            _gamepad.SetMotorSpeeds(this._motorMinFrecuency, this._motorMaxFrecuency); // �U���̋�����ݒ肵�܂�
            yield return null;
        }
        _gamepad.SetMotorSpeeds(0f, 0f); // �U�����~���܂�

        for (int cnt = 0; cnt < 5; cnt++)
        {
            _gamepad.SetMotorSpeeds(0f, 0f); // �U�����~���܂�
            yield return null;
        }
        _gamepad.SetMotorSpeeds(0f, 0f); // �U�����~���܂�

        this._isVivrate = false;//�U�����t���O 0
    }
    #endregion
}