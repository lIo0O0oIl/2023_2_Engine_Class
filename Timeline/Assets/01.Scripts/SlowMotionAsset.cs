using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SlowMotionAsset : PlayableAsset
{
    public float timeValue = 1f;
    // 드래그 해서 트랙에 올리는 순간 실행
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SlowMotion>.Create(graph);        // 막대 그래프를 생성해서 넣어주는 역할

        var behaviour = playable.GetBehaviour();

        behaviour.timeValue = timeValue;
        return playable;
    }
}
