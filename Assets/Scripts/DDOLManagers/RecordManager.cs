using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Game
{
    public class RecordEventArgs
    {
        public int Position;
        public PlayerResult Result;
    }

    public class RecordManager : MonoBehaviour
    {
        public event EventHandler<RecordEventArgs> NewRecord;
        public event EventHandler RecordAdded, WillNotRecord;
        public Records Records;
        public CreatePlayerRecordUI PlayerRecordUI;

        void Awake()
        {
            if (File.Exists("records.json"))
                Records = JsonUtility.FromJson<Records>(File.ReadAllText("records.json"));
            else
            {
                Records = new Records();
                Records.List = new List<PlayerRecord>(10);
                File.WriteAllText("records.json", JsonUtility.ToJson(Records));
            }
        }

        void Start()
        {
            UIManager.Instance.EndGameClicked += OnEndGameClicked;
            PlayerRecordUI.NewRecordCreated += OnNewRecordCreated;
        }

        void OnNewRecordCreated(object sender, PlayerRecord e)
        {
            if (e.Position > Records.List.Count && e.Position < 10)
                Records.List.Add(e);
            else
                Records.List.Insert(e.Position, e);

            Records.List.ForEach(record => record.Position = Records.List.IndexOf(record) + 1);
            Records.List.RemoveAll(record => record.Position > 10);

            File.WriteAllText("records.json", JsonUtility.ToJson(Records));
            RecordAdded?.Invoke(null, null);
        }

        void OnEndGameClicked(object sender, EventArgs e)
        {
            var bestResult = GameManager.Instance.PlayerResults[0];

            GameManager.Instance.PlayerResults.ForEach(result =>
            {
                if (result.Score > bestResult.Score)
                    bestResult = result;
            });

            if (bestResult.Score > 0)
                for (int i = 0; i < Records.List.Count; i++)
                    if (bestResult.Score > Records.List[i].Score)
                    {
                        NewRecord?.Invoke(null, new RecordEventArgs() { Position = i, Result = bestResult });
                        return;
                    }

            if (Records.List.Count < 10)
            {
                NewRecord?.Invoke(null, new RecordEventArgs() { Position = Records.List.Count == 0 ? 1 : Records.List.Count - 1, Result = bestResult });
                return;
            }

            WillNotRecord?.Invoke(null, null);
        }
    }
}
