namespace Packets;

public interface ITransfer<TData>
    where TData : IData
{
    TData LeaseData();

    internal byte BeginIncomingPacket(TData data)
    {
        var id = data.BeginRead();
#if DEBUG
        DebugIncoming(id);
#endif
        return id;
    }

    internal void EndIncomingPacket(TData data)
    {
        data.ReadEnd();
    }

    internal TData BeginOutgoingPacket(byte id)
    {
#if DEBUG
        DebugOutgoing(id);
#endif
        var data = LeaseData();

        data.BeginWrite(id);

        return data;
    }

    internal void EndOutgoingPacket(TData data)
    {
        var size = data.EndWrite();

        data.WriteSize((short)size);
    }

    internal TData BeginOutgoingNoSizePacket(byte id)
    {
#if DEBUG
        DebugOutgoing(id);
#endif
        var data = LeaseData();

        data.BeginWriteNoSize(id);

        return data;
    }

    internal void EndOutgoingNoSizePacket(TData data)
    {
        data.EndWrite();
    }
#if DEBUG
    private static void DebugIncoming(byte packetId)
    {
        Console.WriteLine($"Incoming: {GetPacketName(packetId)}");
    }

    private static void DebugOutgoing(byte packetId)
    {
        Console.WriteLine($"Outgoing: {GetPacketName(packetId)}");
    }

    private static string GetPacketName(byte packetId)
    {
        switch (packetId)
        {
            // ReSharper disable StringLiteralTypo
            case 0x00: return "0x00 PACKET_LOGIN";
            case 0x01: return "0x01 PACKET_LOGOUT";
            case 0x02: return "0x02 PACKET_REQ_MOVE";
            case 0x03: return "0x03 PACKET_SPEECH";
            case 0x04: return "0x04 PACKET_GODMODE_TOGGLE";
            case 0x05: return "0x05 PACKET_ATTACK";
            case 0x06: return "0x06 PACKET_REQ_OBJUSE";
            case 0x07: return "0x07 PACKET_REQ_GETOBJ";
            case 0x08: return "0x08 PACKET_REQ_DROPOBJ";
            case 0x09: return "0x09 PACKET_REQ_LOOK";
            case 0x0A: return "0x0A PACKET_EDIT";
            case 0x0B: return "0x0B PACKET_EDITAREA";
            case 0x0C: return "0x0C PACKET_TILEDATA";
            case 0x0D: return "0x0D PACKET_NPCDATA";
            case 0x0E: return "0x0E PACKET_TEMPLATEDATA";
            case 0x0F: return "0x0F PACKET_PAPERDOLL";
            case 0x10: return "0x10 PACKET_HUEDATA";
            case 0x11: return "0x11 PACKET_MOBILESTAT";
            case 0x12: return "0x12 PACKET_GODCOMMAND";
            case 0x13: return "0x13 PACKET_REQ_OBJEQUIP";
            case 0x14: return "0x14 PACKET_ELEVCHANGE";
            case 0x15: return "0x15 PACKET_FOLLOW";
            case 0x16: return "0x16 PACKET_REQUEST_SCRIPT_NAMES";
            case 0x17: return "0x17 PACKET_SCRIPT_TREE_CMD";
            case 0x18: return "0x18 PACKET_SCRIPT_ATTACH";
            case 0x19: return "0x19 PACKET_NPCCONVO_DATA";
            case 0x1A: return "0x1A PACKET_MOVE";
            case 0x1B: return "0x1B PACKET_LOGIN_CONFIRM";
            case 0x1C: return "0x1C PACKET_TEXT";
            case 0x1D: return "0x1D PACKET_DESTROY_OBJECT";
            case 0x1E: return "0x1E PACKET_ANIMATE";
            case 0x1F: return "0x1F PACKET_EXPLODE";
            case 0x20: return "0x20 PACKET_ZMOVE";
            case 0x21: return "0x21 PACKET_BLOCKED_MOVE";
            case 0x22: return "0x22 PACKET_OK_MOVE";
            case 0x23: return "0x23 PACKET_OBJMOVE";
            case 0x24: return "0x24 PACKET_OPEN_GUMP";
            case 0x25: return "0x25 PACKET_OBJ_TO_OBJ";
            case 0x26: return "0x26 PACKET_OLD_CLIENT";
            case 0x27: return "0x27 PACKET_GETOBJ_FAILED";
            case 0x28: return "0x28 PACKET_DROPOBJ_FAILED";
            case 0x29: return "0x29 PACKET_DROPOBJ_OK";
            case 0x2A: return "0x2A PACKET_BLOOD";
            case 0x2B: return "0x2B PACKET_GODMODE";
            case 0x2C: return "0x2C PACKET_DEATH";
            case 0x2D: return "0x2D PACKET_HEALTH";
            case 0x2E: return "0x2E PACKET_EQUIP_ITEM";
            case 0x2F: return "0x2F PACKET_SWING";
            case 0x30: return "0x30 PACKET_ATTACK_OK";
            case 0x31: return "0x31 PACKET_ATTACK_END";
            case 0x32: return "0x32 PACKET_HACK_MOVER";
            case 0x33: return "0x33 PACKET_GROUP";
            case 0x34: return "0x34 PACKET_CLIENTQUERY";
            case 0x35: return "0x35 PACKET_RESOURCETYPE";
            case 0x36: return "0x36 PACKET_RESOURCETILEDATA";
            case 0x37: return "0x37 PACKET_MOVEOBJECT";
            case 0x38: return "0x38 PACKET_FOLLOWMOVE";
            case 0x39: return "0x39 PACKET_GROUPS";
            case 0x3A: return "0x3A PACKET_SKILLS";
            case 0x3B: return "0x3B PACKET_OFFERACCEPT";
            case 0x3C: return "0x3C PACKET_MULTI_OBJ_TO_OBJ";
            case 0x3D: return "0x3D PACKET_SHIP";
            case 0x3E: return "0x3E PACKET_VERSIONS";
            case 0x3F: return "0x3F PACKET_UPD_OBJCHUNK";
            case 0x40: return "0x40 PACKET_UPD_TERRCHUNK";
            case 0x41: return "0x41 PACKET_UPD_TILEDATA";
            case 0x42: return "0x42 PACKET_UPD_ART";
            case 0x43: return "0x43 PACKET_UPD_ANIM";
            case 0x44: return "0x44 PACKET_UPD_HUES";
            case 0x45: return "0x45 PACKET_VER_OK";
            case 0x46: return "0x46 PACKET_NEW_ART";
            case 0x47: return "0x47 PACKET_NEW_TERR";
            case 0x48: return "0x48 PACKET_NEW_ANIM";
            case 0x49: return "0x49 PACKET_NEW_HUES";
            case 0x4A: return "0x4A PACKET_DESTROY_ART";
            case 0x4B: return "0x4B PACKET_CHECK_VER";
            case 0x4C: return "0x4C PACKET_SCRIPT_NAMES";
            case 0x4D: return "0x4D PACKET_SCRIPT_FILE";
            case 0x4E: return "0x4E PACKET_LIGHTCHANGE";
            case 0x4F: return "0x4F PACKET_Sunlight";
            case 0x50: return "0x50 PACKET_BOARDHEADER";
            case 0x51: return "0x51 PACKET_BOARDMSG";
            case 0x52: return "0x52 PACKET_POSTMSG";
            case 0x53: return "0x53 PACKET_LOGIN_REJECT";
            case 0x54: return "0x54 PACKET_SOUND";
            case 0x55: return "0x55 PACKET_LOGIN_COMPLETE";
            case 0x56: return "0x56 PACKET_MAP_COMMAND";
            case 0x57: return "0x57 PACKET_UPD_REGIONS";
            case 0x58: return "0x58 PACKET_NEW_REGION";
            case 0x59: return "0x59 PACKET_NEW_CONTEXTFX";
            case 0x5A: return "0x5A PACKET_UPD_CONTEXTFX";
            case 0x5B: return "0x5B PACKET_GAMETIME";
            case 0x5C: return "0x5C PACKET_RESTARTVER";
            case 0x5D: return "0x5D PACKET_PRELOGIN";
            case 0x5E: return "0x5E PACKET_SERVERLIST";
            case 0x5F: return "0x5F PACKET_SERVERADD";
            case 0x60: return "0x60 PACKET_SERVERREMOVE";
            case 0x61: return "0x61 PACKET_DESTROY_STATIC";
            case 0x62: return "0x62 PACKET_MOVESTATIC";
            case 0x63: return "0x63 PACKET_AREA_LOAD";
            case 0x64: return "0x64 PACKET_AREA_LOAD_REQ";
            case 0x65: return "0x65 PACKET_WEATHERCHANGE";
            case 0x66: return "0x66 PACKET_BOOKPAGE";
            case 0x67: return "0x67 PACKET_SIMPED";
            case 0x68: return "0x68 PACKET_SCRIPT_LS_ATTACH";
            case 0x69: return "0x69 PACKET_FRIENDS";
            case 0x6A: return "0x6A PACKET_FRIENDNOTIFY";
            case 0x6B: return "0x6B PACKET_KEY_USE";
            case 0x6C: return "0x6C PACKET_TARGET";
            case 0x6D: return "0x6D PACKET_MUSIC";
            case 0x6E: return "0x6E PACKET_ANIM";
            case 0x6F: return "0x6F PACKET_TRADE";
            case 0x70: return "0x70 PACKET_EFFECT";
            case 0x71: return "0x71 PACKET_BBOARD";
            case 0x72: return "0x72 PACKET_COMBAT";
            case 0x73: return "0x73 PACKET_PING";
            case 0x74: return "0x74 PACKET_SHOP_DATA";
            case 0x75: return "0x75 PACKET_RENAME_MOB";
            case 0x76: return "0x76 PACKET_SERVERCHANGE";
            case 0x77: return "0x77 PACKET_NAKED_MOB";
            case 0x78: return "0x78 PACKET_EQUIPPED_MOB";
            case 0x79: return "0x79 PACKET_RESOURCE_QUERY";
            case 0x7A: return "0x7A PACKET_RESOURCE_DATA";
            case 0x7B: return "0x7B PACKET_SEQUENCE";
            case 0x7C: return "0x7C PACKET_OBJPICKER";
            case 0x7D: return "0x7D PACKET_PICKEDOBJ";
            case 0x7E: return "0x7E PACKET_GODVIEW_QUERY";
            case 0x7F: return "0x7F PACKET_GODVIEW_DATA";
            case 0x80: return "0x80 PACKET_ACCT_LOGIN_REQ";
            case 0x81: return "0x81 PACKET_ACCT_LOGIN_OK";
            case 0x82: return "0x82 PACKET_ACCT_LOGIN_FAIL";
            case 0x83: return "0x83 PACKET_ACCT_DEL_CHAR";
            case 0x84: return "0x84 PACKET_CHG_CHAR_PW";
            case 0x85: return "0x85 PACKET_CHG_CHAR_RESULT";
            case 0x86: return "0x86 PACKET_ALL_CHARACTERS";
            case 0x87: return "0x87 PACKET_SEND_RESOURCES";
            case 0x88: return "0x88 PACKET_OPEN_PAPERDOLL";
            case 0x89: return "0x89 PACKET_CORPSE_EQ";
            case 0x8A: return "0x8A PACKET_TRIGGEREDIT";
            case 0x8B: return "0x8B PACKET_DISPLAY_SIGN";
            case 0x8C: return "0x8C PACKET_USER_SERVER";
            case 0x8D: return "0x8D PACKET_UNUSED3";
            case 0x8E: return "0x8E PACKET_MOVE_CHARACTER";
            case 0x8F: return "0x8F PACKET_UNUSED4";
            case 0x90: return "0x90 PACKET_OPEN_COURSEGUMP";
            case 0x91: return "0x91 PACKET_POSTLOGIN";
            case 0x92: return "0x92 PACKET_UPD_MULTI";
            case 0x93: return "0x93 PACKET_BOOKHDR";
            case 0x94: return "0x94 PACKET_UPD_SKILL";
            case 0x95: return "0x95 PACKET_HUEPICKER";
            case 0x96: return "0x96 PACKET_GAMECENT_MON";
            case 0x97: return "0x97 PACKET_PLAYERMOVE";
            case 0x98: return "0x98 PACKET_MOBNAME";
            case 0x99: return "0x99 PACKET_TARGET_MULTI";
            case 0x9A: return "0x9A PACKET_TEXT_ENTRY";
            case 0x9B: return "0x9B PACKET_REQUEST_ASSIST";
            case 0x9C: return "0x9C PACKET_ASSIST_REQUEST";
            case 0x9D: return "0x9D PACKET_GM_SINGLE";
            case 0x9E: return "0x9E PACKET_SHOP_SELL";
            case 0x9F: return "0x9F PACKET_SHOP_OFFER";
            case 0xA0: return "0xA0 PACKET_BRITANNIA_SELECT";
            case 0xA1: return "0xA1 PACKET_HP_HEALTH";
            case 0xA2: return "0xA2 PACKET_MANA_HEALTH";
            case 0xA3: return "0xA3 PACKET_FAT_HEALTH";
            case 0xA4: return "0xA4 PACKET_HARDWARE_INFO";
            case 0xA5: return "0xA5 PACKET_WEB_BROWSE";
            case 0xA6: return "0xA6 PACKET_MESSAGE";
            case 0xA7: return "0xA7 PACKET_REQ_TIP";
            case 0xA8: return "0xA8 PACKET_BRITANNIA_LIST";
            case 0xA9: return "0xA9 PACKET_CITIES_AND_CHARS";
            case 0xAA: return "0xAA PACKET_CURRENT_TARGET";
            case 0xAB: return "0xAB PACKET_STRING_QUERY";
            case 0xAC: return "0xAC PACKET_STRING_RESPONSE";
            case 0xAD: return "0xAD PACKET_SPEECH_UNICODE";
            case 0xAE: return "0xAE PACKET_TEXT_UNICODE";
            case 0xAF: return "0xAF PACKET_DEATH_ANIM";
            case 0xB0: return "0xB0 PACKET_GENERIC_GUMP";
            case 0xB1: return "0xB1 PACKET_GENGUMP_TRIG";
            case 0xB2: return "0xB2 PACKET_CHAT_MSG";
            case 0xB3: return "0xB3 PACKET_CHAT_TEXT";
            case 0xB4: return "0xB4 PACKET_TARGET_OBJLIST";
            case 0xB5: return "0xB5 PACKET_CHAT_OPEN";
            case 0xB6: return "0xB6 PACKET_HELP_REQUEST";
            case 0xB7: return "0xB7 PACKET_HELP_UNICODE_TEXT";
            case 0xB8: return "0xB8 PACKET_CHAR_PROFILE";
            case 0xB9: return "0xB9 PACKET_FEATURES";
            default: return $"0x{packetId:X2} INVALID";
            // ReSharper restore StringLiteralTypo
        }
    }
#endif
}