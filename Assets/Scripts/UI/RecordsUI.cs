using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

namespace Game
{
    public class RecordsUI : MonoBehaviour
    {
        public event EventHandler CloseButtonCliked;
        public RecordManager RecordManager;
        public GameObject PlayerRecordUIPrefab;
        public Transform RecordGroup;
        public Button CloseButton;

        void Awake()
        {
            CloseButton.onClick.AddListener(() => CloseButtonCliked?.Invoke(null, null));
        }

        void OnEnable()
        {
            RecordManager.Records.List.ForEach(record =>
            {
                var newRecordUI = Instantiate(PlayerRecordUIPrefab, RecordGroup).GetComponent<PlayerRecordUI>();

                newRecordUI.ScoreText.text = record.Score.ToString();
                newRecordUI.DateText.text = record.Date.ToString();
                newRecordUI.PlayerNameText.text = record.Name.ToString();
                newRecordUI.PositionText.text = record.Position.ToString();
            });
        }

        void OnDisable()
        {
            for (int i = 0; i < RecordGroup.childCount; i++)           
                Destroy(RecordGroup.GetChild(i).gameObject);     
        }
    }
}
