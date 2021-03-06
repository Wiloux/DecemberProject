/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID BEARTRAPSETUP = 3283823493U;
        static const AkUniqueID BEARTRAPSTOP = 3005731438U;
        static const AkUniqueID BEARTRAPTRIGGER = 4283543462U;
        static const AkUniqueID DEFEATEDSTATIC = 3844998465U;
        static const AkUniqueID ENDSCENE = 3129425356U;
        static const AkUniqueID FIREWORKSSETUP = 3701804816U;
        static const AkUniqueID FIREWORKSSTOP = 2740640909U;
        static const AkUniqueID FIREWORKSTRIGGER = 2187166475U;
        static const AkUniqueID FOOTSTEP1 = 2385628260U;
        static const AkUniqueID FOOTSTEP2 = 2385628263U;
        static const AkUniqueID FOOTSTEP3 = 2385628262U;
        static const AkUniqueID FOOTSTEP4 = 2385628257U;
        static const AkUniqueID GAMEOVERFADEOUT = 4108772633U;
        static const AkUniqueID ISBEINGCORRUPTED = 2661533922U;
        static const AkUniqueID PLAYCHASE = 3706411783U;
        static const AkUniqueID PLAYGAMEOVER = 2895517991U;
        static const AkUniqueID PLAYGETITEM = 2080376856U;
        static const AkUniqueID PLAYGIGGLES = 2627712155U;
        static const AkUniqueID PLAYHIGHLIGHT = 145167493U;
        static const AkUniqueID PLAYJERRYCAN = 448200913U;
        static const AkUniqueID PLAYKEYPRESS = 1168436793U;
        static const AkUniqueID PLAYKILLERFOOTSTEPS2 = 1927847363U;
        static const AkUniqueID PLAYMAINMENU = 3998606525U;
        static const AkUniqueID PLAYNSFOOTSTEP = 1928550884U;
        static const AkUniqueID PLAYORB = 2053742238U;
        static const AkUniqueID PLAYOVERWORLD_TENSE = 1809856591U;
        static const AkUniqueID PLAYSTATIC = 3467592801U;
        static const AkUniqueID STATICGAMEOVER = 3887894719U;
        static const AkUniqueID STOPALLSOUNDS = 1416937818U;
        static const AkUniqueID STOPCHASE = 2399390441U;
        static const AkUniqueID STOPGIGGLESCAUGHT = 206134053U;
        static const AkUniqueID STOPGIGGLESLOST = 237550319U;
        static const AkUniqueID STOPJERRYCAN = 2638603827U;
        static const AkUniqueID STOPORB = 2852051516U;
        static const AkUniqueID STOPOVERWORLD_TENSE = 4033460349U;
        static const AkUniqueID STOPSTATIC = 1707810303U;
        static const AkUniqueID TVHEADSTATICPLAY = 664857509U;
        static const AkUniqueID TVHEADSTATICSTOP = 2047933731U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace END
        {
            static const AkUniqueID GROUP = 529726532U;

            namespace STATE
            {
                static const AkUniqueID NO = 1668749452U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID YES = 979470758U;
            } // namespace STATE
        } // namespace END

        namespace GIGGLES
        {
            static const AkUniqueID GROUP = 4002248037U;

            namespace STATE
            {
                static const AkUniqueID CHASING = 1516571116U;
                static const AkUniqueID FINISHED = 3683254051U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID PATROL = 3497767639U;
                static const AkUniqueID WORKING = 1992928594U;
            } // namespace STATE
        } // namespace GIGGLES

        namespace ISBEINGCORRUPTED
        {
            static const AkUniqueID GROUP = 2661533922U;

            namespace STATE
            {
                static const AkUniqueID NO = 1668749452U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID YES = 979470758U;
            } // namespace STATE
        } // namespace ISBEINGCORRUPTED

        namespace OVERWORLD
        {
            static const AkUniqueID GROUP = 1562068129U;

            namespace STATE
            {
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID NONTENSE = 2619280431U;
                static const AkUniqueID TENSE = 391998042U;
            } // namespace STATE
        } // namespace OVERWORLD

        namespace PAUSEMENU
        {
            static const AkUniqueID GROUP = 3494343696U;

            namespace STATE
            {
                static const AkUniqueID NO = 1668749452U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID YES = 979470758U;
            } // namespace STATE
        } // namespace PAUSEMENU

        namespace SPOTTED
        {
            static const AkUniqueID GROUP = 3214304800U;

            namespace STATE
            {
                static const AkUniqueID NO = 1668749452U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID YES = 979470758U;
            } // namespace STATE
        } // namespace SPOTTED

    } // namespace STATES

    namespace SWITCHES
    {
        namespace GIGGLES
        {
            static const AkUniqueID GROUP = 4002248037U;

            namespace SWITCH
            {
                static const AkUniqueID CHASING = 1516571116U;
                static const AkUniqueID FINISHED = 3683254051U;
                static const AkUniqueID SILENCE = 3041563226U;
                static const AkUniqueID WORKING = 1992928594U;
            } // namespace SWITCH
        } // namespace GIGGLES

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID KILLERPROXIMITY = 110585135U;
        static const AkUniqueID MUSICVOLUME = 2346531308U;
        static const AkUniqueID SFXVOLUME = 988953028U;
        static const AkUniqueID SPATIALIZATION1 = 2155950306U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID GAMEOVER = 4158285989U;
        static const AkUniqueID MAIN = 3161908922U;
        static const AkUniqueID MAINMENU = 3604647259U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MASTERNOREVERB = 3104833122U;
        static const AkUniqueID MUSIC = 3991942870U;
    } // namespace BUSSES

    namespace AUX_BUSSES
    {
        static const AkUniqueID NOREVERB = 3840845152U;
        static const AkUniqueID OVERWORLDMUSIC = 1872467674U;
    } // namespace AUX_BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
