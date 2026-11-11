using System;
using UnityEngine;

namespace Fantasya
{
    /// <summary>
    /// 플레이어가 승인한 지속 행동 약속을 대화와 게임 실행 사이에 전달한다.
    /// </summary>
    [Serializable]
    public sealed class CompanionPromise
    {
        public string mode = "auto";
        public bool allowAttack = true;
        public string text = "자유롭게 판단하며 플레이어를 도와주세요.";
        public string updatedAt;
    }

    /// <summary>
    /// 발언 또는 실제 게임 사건의 출처와 시각을 함께 보관한다.
    /// </summary>
    [Serializable]
    public sealed class CompanionMessage
    {
        public string id;
        public string at;
        public string role;
        public string text;
        public string source;
    }

    /// <summary>
    /// 명시적으로 기억한 사실을 식별자와 함께 전달한다.
    /// </summary>
    [Serializable]
    public sealed class CompanionMemory
    {
        public string id;
        public string text;
        public string source;
        public string at;
    }

    /// <summary>
    /// 외부 에이전트의 유한한 행동 요청과 게임 세션 정보를 전달한다.
    /// </summary>
    [Serializable]
    public sealed class CompanionCommand
    {
        public string id;
        public string action;
        public int targetId;
        public float seconds;
        public string reason;
        public string sessionId;
        public long issuedAt;
        public string owner;
    }

    /// <summary>
    /// 로컬 서비스에서 읽은 동료별 성격·대화·기억·약속·제어권을 묶는다.
    /// </summary>
    [Serializable]
    public sealed class CompanionState
    {
        public string id;
        public string name;
        public string personality;
        public CompanionPromise promise;
        public CompanionMessage[] messages;
        public CompanionMemory[] memories;
        public bool controllerActive;
        public string controllerName;
        public bool awaitingExternalReply;
        public CompanionCommand[] commands;
    }

    /// <summary>
    /// 현재 공격 가능한 적의 실제 식별자와 상태를 외부 에이전트에 제공한다.
    /// </summary>
    [Serializable]
    public sealed class CompanionTarget
    {
        public int id;
        public string name;
        public int health;
        public float distance;
    }

    /// <summary>
    /// Unity가 실제로 관측한 동료의 상태와 적용 결과를 전달한다.
    /// </summary>
    [Serializable]
    public sealed class CompanionSnapshot
    {
        public string id;
        public string name;
        public string personality;
        public bool alive;
        public int health;
        public int maxHealth;
        public string action;
        public string reason;
        public string receipt;
        public Vector3 position;
        public Vector3 leaderPosition;
        public bool leaderAlive;
        public int leaderHealth;
        public CompanionTarget[] targets;
    }
}
