using UnityEngine;

public class FlightIntent
{
    public ATCController origin;
    public ATCController destination;
    public DroneAgent spawnedAgent;

    public string flightID;
    public string callSign;

    public FlightIntent()
    {
        flightID = System.Guid.NewGuid().ToString();
        callSign = GenerateCallSign();
    }

    private string GenerateCallSign()
    {
        string prefix =
            $"{(char)Random.Range('A', 'Z' + 1)}" +
            $"{(char)Random.Range('A', 'Z' + 1)}" +
            $"{(char)Random.Range('A', 'Z' + 1)}";
        string number = Random.Range(1, 1000).ToString("D3");
        return $"{prefix}-{number}";
    }
}
