using BDFramework.ScreenView;
using BDFramework.UFlux;
using MR.Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.GameUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TestRoomScore : MonoBehaviour {

    public Button nextBtn;
    public Button returnBtn;

    public GameObject resultGo;
    public GameObject scoreGo;

    public GameObject[] winGos;
    public GameObject[] loseGos;

    public TMP_Text scoreATxt;
    public TMP_Text scoreBTxt;

    public Transform groupA;
    public Transform groupB;

    public GameObject scoreTemplateA;
    public GameObject scoreTemplateB;


    public void Set(BattleGroundScoreCD dataCD, int selfIndex, UnityAction callBack) {
        nextBtn.onClick.AddListener(OnNext);
        returnBtn.onClick.AddListener(callBack);
        resultGo.SetActive(true);
        scoreGo.SetActive(false);

        var teamAKill = 0;
        var teamANum = 0;
        var teamADie = 0;
        var teamAOutput = 0f;
        var teamATake = 0f;
        var teamBNum = 0;
        var teamBKill = 0;
        var teamBDie = 0;
        var teamBOutput = 0f;
        var teamBTake = 0f;
        foreach (var kv in dataCD.Datas) {
            var idx = kv.Key;
            var data = kv.Value;
            if (idx < 3) {
                teamAKill += data.kill;
                teamADie += data.die;
                teamAOutput += data.output.AsFloat();
                teamATake += data.take.AsFloat();
                if (!TestBattle.Offlines.Contains(idx))
                    teamANum++;
            } else {
                teamBKill += data.kill;
                teamBDie += data.die;
                teamBOutput += data.output.AsFloat();
                teamBTake += data.take.AsFloat();
                if (!TestBattle.Offlines.Contains(idx))
                    teamBNum++;
            }
        }

        foreach (var kv in dataCD.Datas) {
            var idx = kv.Key;
            var data = kv.Value;
            var go = idx < 3 == selfIndex < 3 ? Instantiate(scoreTemplateA, groupA) : Instantiate(scoreTemplateB, groupB);
            if (idx == selfIndex) {
                go.transform.Find("SelfName").GetComponent<TMP_Text>().text = TestBattle.PlayerNameDic[idx];
                go.transform.Find("Name").gameObject.SetActive(false);
                go.transform.Find("BG").gameObject.SetActive(false);
            } else {
                go.transform.Find("Name").GetComponent<TMP_Text>().text = TestBattle.PlayerNameDic[idx];
                go.transform.Find("SelfName").gameObject.SetActive(false);
                go.transform.Find("SelfBG").gameObject.SetActive(false);
            }
            if (idx < 3) {
                SetNumberData(data.kill, teamAKill, go.transform.Find("Kill"));
                SetNumberData(data.die, teamADie, go.transform.Find("Die"));
                SetNumberData(data.output.AsFloat(), teamAOutput, go.transform.Find("Output"));
                SetNumberData(data.take.AsFloat(), teamATake, go.transform.Find("Take"));
            } else {
                SetNumberData(data.kill, teamBKill, go.transform.Find("Kill"));
                SetNumberData(data.die, teamBDie, go.transform.Find("Die"));
                SetNumberData(data.output.AsFloat(), teamBOutput, go.transform.Find("Output"));
                SetNumberData(data.take.AsFloat(), teamBTake, go.transform.Find("Take"));
            }
        }
        if (selfIndex < 3) {
            scoreATxt.text = NumberToSpriteStr(teamAKill);
            scoreBTxt.text = NumberToSpriteStr(teamBKill);
        } else {
            scoreATxt.text = NumberToSpriteStr(teamBKill);
            scoreBTxt.text = NumberToSpriteStr(teamAKill);
        }
        if (selfIndex == -1) {
            foreach (var go in loseGos)
                go.SetActive(false);
        } else {
            if (teamANum == 0 || teamBNum == 0 || teamAKill > teamBKill == selfIndex < 3)
                foreach (var go in loseGos)
                    go.SetActive(false);
            else
                foreach (var go in winGos)
                    go.SetActive(false);
        }
    }

    private void SetNumberData(float num, float sum, Transform parent) {
        parent.Find("Num").GetComponent<TMP_Text>().text = num.ToString("0");
        var a = sum == 0 ? 0 : (num / sum);
        parent.Find("Progress").GetComponent<Image>().fillAmount = a;
        parent.Find("Percent").GetComponent<TMP_Text>().text = a.ToString("0%");
    }

    private string NumberToSpriteStr(int number) {
        var str = number.ToString();
        var result = "";
        foreach (var c in str)
            result += $"<sprite={c}>";
        return result;
    }

    public void OnNext() {
        resultGo.SetActive(false);
        scoreGo.SetActive(true);
    }
}
