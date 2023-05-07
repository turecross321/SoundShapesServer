using System.Diagnostics.CodeAnalysis;
using NPTicket;
using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

[SuppressMessage("ReSharper", "IdentifierTypo")]
[SuppressMessage("ReSharper", "StringLiteralTypo")]
public static class PlatformHelper
{
    public static PlatformType? GetPlatformType(Ticket ticket)
    {
        bool rpcn = ticket.IssuerId switch
        {
            0x100 => false, // psn
            0x33333333 => true, // rpcn
            _ => throw new ArgumentOutOfRangeException()
        };

        return GetPlatformFromTitleId(ticket.TitleId, rpcn);
    }

    private static PlatformType? GetPlatformFromTitleId(string titleId, bool rpcn)
    {
        if (rpcn) return PlatformType.Rpcs3;
        if (PsvIds.Contains(titleId)) return PlatformType.PsVita;
        if (Ps3Ids.Contains(titleId)) return PlatformType.Ps3;
        if (Ps4Ids.Contains(titleId)) return PlatformType.Ps4;

        return null;
    }

    private static readonly string[] Ps3Ids =
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


    private static readonly string[] PsvIds =
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

    private static readonly string[] Ps4Ids =
    {
        "CUSA00040",
        "CUSA00073",
        "CUSA00090",
        "CUSA00154",
        "CUSA00175",
        "CUSA00800"
    };
    // Courtesy of https://www.serialstation.com/titles/?name=sound%20shapes&so=tn&systems=97ec53a2-f676-4c89-8172-e653dce5eed1
}