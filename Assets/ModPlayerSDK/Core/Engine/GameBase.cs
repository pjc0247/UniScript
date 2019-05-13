using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameBaseNotifiable : MonoBehaviourPunCallbacks
{
    public static GameBaseNotifiable instance;

    public virtual void Awake()
    {
        instance = this;
    }

    public void NotifyInteraction(PPlayer sender, InteractionData data)
    {
        OnInteraction(sender, data);
    }
    protected virtual void OnInteraction(PPlayer sender, InteractionData data)
    {
    }

    public void NotifyPlayerAction(PPlayer sender, int actionId)
    {
        Debug.Log("[NotifyPlayerAction] " + actionId);

        if (actionId == 1) OnAction1(sender);
        else if (actionId == 2) OnAction2(sender);
    }
    protected virtual void OnAction1(PPlayer sender)
    {
    }
    protected virtual void OnAction2(PPlayer sender)
    {
    }

    public void RpcAll(string name, params object[] args)
    {
        photonView.RPC(name, RpcTarget.All, args);
    }
}
public class GameBase<T> : GameBaseNotifiable
    where T : new()
{
    public bool is3rdPersonView = false;
    public int thirdPersonAngleXMinOverride = -25;
    public int thirdPersonAngleXMaxOverride = 30;

    protected int initialPlayers { get; private set; }
    protected bool failed { get; private set; }
    protected bool isPlaying { get; set; }

    private void Start()
    {
        PPlayer.LocalPlayer.thirdPersonAngleXMax = thirdPersonAngleXMaxOverride;
        PPlayer.LocalPlayer.thirdPersonAngleXMin = thirdPersonAngleXMinOverride;

        if (PhotonNetwork.IsMasterClient == false)
            return;

        Room.is3rdPersonCam = is3rdPersonView;
        Room.ResetGameContext<T>();
        ReviveAllPlayers();
        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        initialPlayers = Room.players.Count;
        isPlaying = true;

        Room.isControllable = false;
        yield return StartCoroutine(PlayNarration());
        Room.isControllable = true;
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(ProcessGame());
    }
    protected virtual IEnumerator PlayNarration()
    {
        yield return null;
    }
    protected virtual IEnumerator ProcessGame()
    {
        yield return null;
    }

    protected T GetContext()
    {
        return Room.GetGameContext<T>();
    }
    protected void UpdateContext(T ctx)
    {
        Room.UpdateGameContext<T>(ctx);
    }

    protected void ReviveAllPlayers()
    {
        foreach (var p in Room.players)
            p.Value.Revive();
    }
    protected void RepositionAllPlayers()
    {
        RepositionAllPlayers(transform);
    }
    protected void RepositionAllPlayers(Transform parent)
    {
        var count = 0;
        foreach (var p in Room.players)
        {
            var tf = parent.Find($"spawn_{count}");
            if (tf != null)
                p.Value.LocalTeleport(tf.position);
            count++;
        }
    }
    protected void LookAtCameraAllPlayers()
    {
        foreach (var p in Room.players)
            p.Value.transform.LookAt(Camera.main.transform);
    }

    protected void AwardAll(int n)
    {
        foreach (var p in Room.players)
            Room.context.AwardCoin(p.Key, n);
        Room.UpdateContext();
    }
    protected void Award(string pid, int n)
    {
        Room.context.AwardCoin(pid, n);
        Room.UpdateContext();
    }
    protected void AddPoint(string pid, int n)
    {
        Room.context.AddPoint(pid, n);
        Room.UpdateContext();
    }

    protected void MarkAsDead(string playerId)
    {
        if (Room.players.ContainsKey(playerId))
            Room.players[playerId].Die();
    }
}
