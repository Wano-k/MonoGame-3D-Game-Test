﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test;

namespace RPG_Paper_Maker
{
    [Serializable]
    public abstract class SuperListItem
    {
        public int Id;
        public string Name = "";

        public abstract SuperListItem CreateCopy();
    }

    [Serializable]
    public abstract class ComboxBoxSpecialTilesetItem : SuperListItem
    {
        public SystemGraphic Graphic;
    }

    [Serializable]
    public class SuperListItemName : SuperListItem
    {
        public Dictionary<string, string> Names;

        public SuperListItemName(int id) : this(id, WANOK.GetDefaultNames())
        {

        }

        public SuperListItemName(int id, Dictionary<string, string> names)
        {
            Id = id;
            Names = names;
            SetName();
        }

        public void SetName()
        {
            Name = Names[WANOK.CurrentLang];
        }

        public override SuperListItem CreateCopy()
        {
            return new SuperListItemName(Id, new Dictionary<string, string>(Names));
        }
    }

    [Serializable]
    public class SuperListItemNameWithoutLang : SuperListItem
    {

        public SuperListItemNameWithoutLang(int id) : this(id, "")
        {

        }

        public SuperListItemNameWithoutLang(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override SuperListItem CreateCopy()
        {
            return new SuperListItemNameWithoutLang(Id, Name);
        }
    }
}
