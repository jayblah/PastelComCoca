﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KoreanCommon;
using LeagueSharp;
using LeagueSharp.Common;

namespace KoreanChoGath
{
    static class CustomMenu
    {
        static public void Load(CommonChampion champion)
        {
            LoadHarasMenu(champion);
            LoadLaneClear(champion);
            LoadOptionsMenu(champion);
        }

        private static void LoadOptionsMenu(CommonChampion champion)
        {
            CommonMenu mainMenu = champion.MainMenu;

            mainMenu.MiscMenu.AddItem(
                new MenuItem(KoreanUtils.ParamName(mainMenu, "flashult"), "Flash + Ult (if killabe)").SetValue(
                    new KeyBind('T', KeyBindType.Press)));

            MenuItem autoStackPassive = mainMenu.MiscMenu.AddItem(
                new MenuItem(KoreanUtils.ParamName(mainMenu, "autostackpassive"), "Auto stack passive").SetValue(true));

            autoStackPassive.ValueChanged += delegate(object sender, OnValueChangeEventArgs e)
            {
                if (e.GetNewValue<bool>())
                {
                    Orbwalking.BeforeAttack += ((ChoGath)champion).stackPassive.StackR;
                }
                else
                {
                    Orbwalking.BeforeAttack -= ((ChoGath)champion).stackPassive.StackR;
                }
            };

            mainMenu.MiscMenu.AddItem(
                new MenuItem(KoreanUtils.ParamName(mainMenu, "koreanprediction"), "Use korean prediction").SetValue(true));
        }

        static private void LoadHarasMenu(CommonChampion champion)
        {
            CommonMenu mainMenu = champion.MainMenu;

            mainMenu.HarasMenu.Items.Remove(KoreanUtils.GetParam(mainMenu, "useetoharas"));
        }

        static private void LoadLaneClear(CommonChampion champion)
        {
            CommonMenu mainMenu = champion.MainMenu;

            mainMenu.LaneClearMenu.AddItem(
                new MenuItem(KoreanUtils.ParamName(mainMenu, "minminionstoq"), "Q must hit at least").SetValue(
                    new Slider(3, 1, 6)));
            mainMenu.LaneClearMenu.AddItem(
                new MenuItem(KoreanUtils.ParamName(mainMenu, "minminionstow"), "W must hit at least").SetValue(
                    new Slider(3, 1, 6)));
        }
    }
}
