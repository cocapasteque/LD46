using Doozy.Engine.UI.Nodes;
using Newtonsoft.Json;
using Proyecto26;
using RSG;
using UnityEngine;

public class LeaderboardController : Singleton<LeaderboardController>
{
    public string lboardUrl = "https://lboard.cocapasteque.tech";

    void Start()
    {
    }

    public IPromise<ResponseHelper> FetchLeaderboard(string level)
    {
        Debug.Log("Fetching leaderboard");
        RequestHelper request = new RequestHelper();
        request.Uri = $"{lboardUrl}/leaderboard/{level}";

        var promise = RestClient.Get(request);
        return promise;
    }

    public void PostScore(LeaderboardMeta data, string level)
    {
        var entry = new LeaderboardEntry()
        {
            Key = data.Alias,
            Metadata = JsonConvert.SerializeObject(data)
        };
        PostEntry(entry, data.Score, level);
    }
    
    public void PostEntry(LeaderboardEntry entry, double score, string board)
    {
        Debug.Log("Posting entry " + entry);
        RequestHelper request = new RequestHelper();
        var reqBody = new LeaderboardEntryRequest()
        {
            Entry = entry,
            Score = score
        };
        
        request.Uri = $"{lboardUrl}/leaderboard/{board}";
        request.BodyString = JsonConvert.SerializeObject(reqBody);
        
        var promise = RestClient.Post(request).Then(response => { Debug.Log(response.Text); });
    }
}

public class LeaderboardEntry
{
    [JsonProperty("key")] public string Key { get; set; }

    [JsonProperty("meta")] public string Metadata { get; set; }

    public override string ToString()
    {
        return $"[key={Key}, metadata={Metadata}]";
    }
}

public class LeaderboardEntryRequest
{
    [JsonProperty("entry")] public LeaderboardEntry Entry { get; set; }

    [JsonProperty("score")] public double Score { get; set; }
}

public class LeaderboardMeta
{
    [JsonProperty("alias")] public string Alias { get; set; }
    [JsonProperty("score")] public double Score { get; set; }
    [JsonProperty("tries")] public int Tries { get; set; }
    [JsonProperty("time")] public string Time { get; set; }
    [JsonProperty("fanUsed")] public int FanUsed { get; set; }
    [JsonProperty("totalFan")] public int TotalFan { get; set; }
    
    public int Index { get; set; }
}