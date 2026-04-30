using UnityEngine;

[CreateAssetMenu(fileName = "GuestProfile", menuName = "Scriptable Objects/GuestProfile")]
public class GuestProfile : ScriptableObject
{
    public PersonalInfo personalInfo;
    public ScanResult scanResult;
    
    public bool isFunKiller;
    public bool hasValidTicket;
}

[System.Serializable]
public struct PersonalInfo{
    public string name;
    public Sprite portrait;
    public string reasonForVisit;
    
    public PersonalInfo(string name, Sprite portrait, string reasonForVisit){
        this.name = name;
        this.portrait = portrait;
        this.reasonForVisit = reasonForVisit;
    }
}

[System.Serializable]
public struct ScanResult
{
    public bool sick;
    public bool lowCortisol;
    public bool highDopamine;
    
    public ScanResult(bool sick, bool lowCortisol, bool highDopamine)
    {
        this.sick = sick;
        this.lowCortisol = lowCortisol;
        this.highDopamine = highDopamine;
    }
}