using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public class PlatformHelper
{
    public static readonly string[] Ps3Ids =
    {
        "NPJA00081",
        "NPEA00289",
        "NPUA70251",
        "NPHA80210",
        "NPHA80211",
        "NPUA80543",
        "NPEA90119",
        "NPJA90248"
    };
    // Courtesy of https://www.serialstation.com/titles/?name=sound%20shapes&so=tn&systems=e80a9d68-aef2-4e23-8768-27ac0e5d9f25


    public static readonly string[] PsvIds =
    {
        "PCSA00003",
        "PCSC00024",
        "PCSD00027",
        "PCSD00028",
        "PCSF00076",
        "PCSF00081",
        "PCSA00094",
        "PCSA00104",
        "PCSF00158",
        "PCSC90026"
    };
    // Courtesy of https://www.serialstation.com/titles/?name=sound%20shapes&so=tn&systems=d8f282b9-916f-491d-9ca3-61e76a74fef0

    public static readonly string[] Ps4Ids =
    {
        "CUSA00040",
        "CUSA00073",
        "CUSA00090",
        "CUSA00154",
        "CUSA00175",
        "CUSA00800"
    };
    // Courtesy of https://www.serialstation.com/titles/?name=sound%20shapes&so=tn&systems=97ec53a2-f676-4c89-8172-e653dce5eed1
    
    public static PlatformType GetPlatform(string titleId, bool genuinePsn)
    {
        if (!genuinePsn) return PlatformType.RPCS3;
        if (Ps3Ids.Contains(titleId)) return PlatformType.PS3;
        if (PsvIds.Contains(titleId)) return PlatformType.PSVita;
        if (Ps4Ids.Contains(titleId)) return PlatformType.PS4;

        return PlatformType.Unknown;
    }
}